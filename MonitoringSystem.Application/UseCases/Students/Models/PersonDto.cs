using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitoringSystem.Application.UseCases.Students.Models
{
    public class PersonDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Status status { get; set;  }
        public string PhoneNumber { get; set; }
    }
    public enum Status : byte
    {
        teacher, student
    }
}
