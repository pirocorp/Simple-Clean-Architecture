namespace CleanArchitecture.Infrastructure.Persistence.Configurations;

using System;

using CleanArchitecture.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> department)
    {
        department
            .HasKey(d => d.Id);

        department
            .Property(d => d.Name)
            .IsRequired()
            .HasMaxLength(200);

        department
            .HasIndex(d => d.Name)
            .IsUnique();
    }
}

