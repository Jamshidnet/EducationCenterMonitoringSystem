using MonitoringSystem.Domein.Common.BaseEntities;
using System.ComponentModel.DataAnnotations.Schema;

namespace MonitoringSystem.Domein.Entities;

public  class Grade : BaseAuditableEntity
{
    public Guid SubjectId { get; set; }

    public Guid StudentId { get; set; }

    public decimal GradeNum { get; set; }


    [ForeignKey("SubjectId")]
    public Subject Subject { get; set; }

    [ForeignKey("StudentId")]
    public Student Student { get; set; }
}
