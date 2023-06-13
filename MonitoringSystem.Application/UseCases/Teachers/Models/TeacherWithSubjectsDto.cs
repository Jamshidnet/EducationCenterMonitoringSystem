using MonitoringSystem.Application.UseCases.Subjects.Models;

namespace MonitoringSystem.Application.UseCases.Teachers.Models
{
    public  class TeacherWithSubjectsDto : TeacherDto
    {
        public List<SubjectDto> Subjects { get; set; }
    }
}
