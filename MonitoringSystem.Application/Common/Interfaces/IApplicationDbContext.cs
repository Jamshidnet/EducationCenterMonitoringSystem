using Microsoft.EntityFrameworkCore;
using MonitoringSystem.Domein.Entities;

namespace MonitoringSystem.Application.Common.Interfaces;

public  interface IApplicationDbContext
{
    DbSet<T> Set<T>() where T : class;
    DbSet<Student> Students { get; }
    DbSet<Subject> Subjects { get; }
    DbSet<Grade> Grades { get; }
    DbSet<Teacher> Teachers { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
