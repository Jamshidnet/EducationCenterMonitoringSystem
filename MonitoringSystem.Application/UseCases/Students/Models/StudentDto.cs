using Microsoft.AspNetCore.Http;

namespace MonitoringSystem.Application.UseCases.Students.Models
{
    public  class StudentDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime BirthDate { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public int StudentRageNumber { get; set; }
        public string  Img { get; set;}

        public IFormFile? ImageFile { get; set; }

        public bool IsImgChange { get; set; }
    }
}
