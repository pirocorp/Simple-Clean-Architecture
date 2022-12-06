namespace CleanArchitecture.Domain.Entities;

using System;
using System.Collections.Generic;

public class Department
{
    public Department()
    {
        this.Name = string.Empty;
        this.Employees = new HashSet<Employee>();
    }

    private Department(
        string name,
        DateTime createdAt)
    {
        this.CreatedAt = createdAt;
        this.IsActive = true;
        this.Name = name;

        this.Employees = new HashSet<Employee>();
    }

    public int Id { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public bool IsActive { get; private set; }

    public string Name { get; private set; }

    public ICollection<Employee> Employees { get; private set; }

    public static Department Create(string name, DateTime createdAt)
        => new (name, createdAt);

    public void Rename(string name)
        => this.Name = name;

    public void Delete()
        => this.IsActive = false;
}
