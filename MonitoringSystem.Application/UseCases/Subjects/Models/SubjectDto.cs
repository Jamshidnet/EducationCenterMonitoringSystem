using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitoringSystem.Application.UseCases.Subjects.Models
{
    public class SubjectDto 
    {

        public Guid Id { get; set; }

        public string SubjectName { get; set; }
        
        public Guid TeacherId { get; set; }

    }
}
