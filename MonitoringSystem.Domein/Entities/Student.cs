using MonitoringSystem.Domein.Common.BaseEntities;

namespace MonitoringSystem.Domein.Entities
{
    public  class Student : BaseAuditableEntity
    {
        public string FirstName { get; set; }
        
        public string LastName { get; set; }

        public DateTime BirthDate { get; set; }
        
        public  string PhoneNumber { get; set; }

        public string Email { get; set; }

        public int StudentRageNumber { get; set; }

        public virtual ICollection<Grade> Grades { get; set; }
    }
}
