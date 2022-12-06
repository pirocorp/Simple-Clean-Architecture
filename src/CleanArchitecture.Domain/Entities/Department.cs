namespace CleanArchitecture.Domain.Entities;

using System;
using System.Collections.Generic;

public class Department
{
    public Department(
        int id, 
        DateTime createdAt, 
        bool isActive, 
        string name, 
        ICollection<Employee> employees)
    {
        Id = id;
        CreatedAt = createdAt;
        IsActive = isActive;
        Name = name;
        Employees = employees;
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

    public int Id { get; }

    public DateTime CreatedAt { get; }

    public bool IsActive { get; private set; }

    public string Name { get; private set; }

    public ICollection<Employee> Employees { get; }

    public static Department Create(string name, DateTime createdAt)
        => new (name, createdAt);

    public void Rename(string name)
        => this.Name = name;

    public void Delete()
        => this.IsActive = false;
}
