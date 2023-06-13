namespace MonitoringSystem.Application.UseCases.Teachers.Models
{
    public class TeacherDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime BirthDate { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

    }
}
