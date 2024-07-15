using Xunit.Abstractions;

namespace CodeForcer.Tests.Features.Students;

public class GetAllStudentsTests(IntegrationTestWebAppFactory factory, ITestOutputHelper testOutputHelper) :
    IntegrationTestBase(factory)
{
    private readonly ITestOutputHelper _testOutputHelper = testOutputHelper;

    [Fact]
    public async Task ShouldReturnEmptyList_WhenNoStudents()
    {
        //Arrange skipped
        
        //Act
        var response = await Client.GetAsync("/students");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseStudents = await response.Content.ReadFromJsonAsync<List<StudentResponse>>();
        responseStudents.Should().BeEmpty();
    }

    [Fact]
    public async Task ShouldReturnStudents_WhenThereAreStudents()
    {
        //Arrange
        var students = Fakers.StudentsFaker.GenerateBetween(3, 52);
        _testOutputHelper.WriteLine($"Сгенерировал {students.Count} студентов");
        
        foreach (var student in students)
            await StudentsRepository.Add(student);
        
        //Act
        var response = await Client.GetAsync("/students");
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var responseStudents = await response.Content.ReadFromJsonAsync<List<StudentResponse>>();
        _testOutputHelper.WriteLine($"Достал {responseStudents!.Count} студентов");
        responseStudents.Should().BeEquivalentTo(students);
    }
}