namespace CleanArchitecture.Infrastructure.Persistence.Configurations;

using CleanArchitecture.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> employee)
    {
        employee
            .HasKey(e => e.Id);

        employee
            .OwnsOne(
                e => e.Address,
                address =>
                {
                    address
                        .Property(e => e.Street)
                        .HasMaxLength(200);

                    address
                        .Property(e => e.Municipality)
                        .HasMaxLength(200);

                    address
                        .Property(e => e.Province)
                        .HasMaxLength(200);
                });

        employee
            .Property(e => e.Email)
            .HasMaxLength(200);

        employee
            .Property(e => e.Name)
            .HasMaxLength(200);

        employee
            .Property(e => e.Salary)
            .HasPrecision(12, 10);

        employee
            .HasOne(e => e.Department)
            .WithMany(d => d.Employees)
            .HasForeignKey(e => e.DepartmentId);
    }
}
