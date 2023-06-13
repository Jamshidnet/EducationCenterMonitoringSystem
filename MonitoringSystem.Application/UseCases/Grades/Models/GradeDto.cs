namespace MonitoringSystem.Application.UseCases.Grades.Models
{
    public class GradeDto
    {
        public Guid Id { get; set; }
        public Guid SubjectId { get; set; }

        public Guid StudentId { get; set; }

        public decimal GradeNum { get; set; }
    }
}
