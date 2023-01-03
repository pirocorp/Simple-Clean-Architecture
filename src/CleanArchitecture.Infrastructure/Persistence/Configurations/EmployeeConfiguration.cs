namespace CleanArchitecture.Infrastructure.Persistence.Configurations;

using CleanArchitecture.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using static CleanArchitecture.Domain.Common.DataConstants.Address;
using static CleanArchitecture.Domain.Common.DataConstants.Employee;

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
                        .HasMaxLength(STREET_MAX_LENGTH);

                    address
                        .Property(e => e.Municipality)
                        .HasMaxLength(MUNICIPALITY_MAX_LENGTH);

                    address
                        .Property(e => e.Province)
                        .HasMaxLength(PROVINCE_MAX_LENGTH);
                });

        employee
            .Property(e => e.Email)
            .HasMaxLength(EMAIL_MAX_LENGTH);

        employee
            .Property(e => e.Name)
            .HasMaxLength(NAME_MAX_LENGTH);

        employee
            .Property(e => e.Salary)
            .HasPrecision(SALARY_PRECISION, SALARY_SCALE);

        employee
            .HasOne(e => e.Department)
            .WithMany(d => d.Employees)
            .HasForeignKey(e => e.DepartmentId);
    }
}
