﻿namespace CleanArchitecture.Application.Common.Interfaces;

using CleanArchitecture.Domain.Entities;

using Microsoft.EntityFrameworkCore;

public interface IApplicationDbContext
{
    DbSet<Department> Departments { get; }

    DbSet<Employee> Employees { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
