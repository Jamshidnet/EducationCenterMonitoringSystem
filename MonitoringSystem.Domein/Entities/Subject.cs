using MonitoringSystem.Domein.Common.BaseEntities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;

namespace MonitoringSystem.Domein.Entities
{
    public  class Subject : BaseAuditableEntity
    {
        public string SubjectName { get; set; }

        public Guid TeacherId { get; set; }

      
        [ForeignKey("TeacherId")]
        public Teacher Teacher { get; set; }
       
        public virtual ICollection<Grade> Grades { get; set; }
    }
}
