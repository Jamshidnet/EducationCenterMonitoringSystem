using MonitoringSystem.Application.UseCases.Grades.Models;

namespace MonitoringSystem.Application.UseCases.Students.Models;

public class GetStudentsWithGrades : StudentDto
{
    public ICollection<GradeDto>? Grades { get; set; }
}
