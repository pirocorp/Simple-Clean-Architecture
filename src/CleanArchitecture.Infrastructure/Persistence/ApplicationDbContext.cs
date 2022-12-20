namespace CleanArchitecture.Infrastructure.Persistence;

using System.Reflection;

using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;

using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext, IDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    { }

    public DbSet<Department> Departments => this.Set<Department>();

    public DbSet<Employee> Employees => this.Set<Employee>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }
}
