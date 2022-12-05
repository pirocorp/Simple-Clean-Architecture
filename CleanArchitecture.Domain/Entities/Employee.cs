namespace CleanArchitecture.Domain.Entities;

using System;

using CleanArchitecture.Domain.Enums;
using CleanArchitecture.Domain.ValueObjects;

public class Employee
{
    public Employee()
    {
        this.Name = string.Empty;
        this.Email = string.Empty;
        this.Address = Address.Empty;

        this.Department = null!;
    }

    public int Id { get; set; }

    public Address Address { get; set; }

    public int Age { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string Email { get; set; }

    public Gender Gender { get; set; }

    public bool IsActive { get; set; }

    public string Name { get; set; }

    public decimal Salary { get; set; }

    public int DepartmentId { get; set; }

    public Department Department { get; set; }
}

