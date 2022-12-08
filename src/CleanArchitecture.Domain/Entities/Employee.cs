namespace CleanArchitecture.Domain.Entities;

using System;

using CleanArchitecture.Domain.Enums;
using CleanArchitecture.Domain.ValueObjects;

public class Employee
{
    public Employee()
    {
        this.Address = Address.Empty;
        this.Email = string.Empty;
        this.Name = string.Empty;

        this.Department = null!;
    }

    private Employee(
        string name,
        int age,
        DateTime createdAt,
        string email,
        string address,
        Gender gender,
        decimal salary,
        int departmentId)
    {
        this.Name = name;
        this.Age = age;
        this.CreatedAt = createdAt;
        this.Email = email;
        this.Address = Address.From(address);
        this.Gender = gender;
        this.Salary = salary;
        this.DepartmentId = departmentId;

        this.IsActive = true;

        this.Department = null!;
    }

    public int Id { get; private set; }

    public Address Address { get; set; }

    public int Age { get; set; }

    public DateTime CreatedAt { get; private set; }

    public string Email { get; set; }

    public Gender Gender { get; private set; }

    public bool IsActive { get; private set; }

    public string Name { get; set; }

    public decimal Salary { get; set; }

    public int DepartmentId { get; set; }

    public Department Department { get; private set; }

    public static Employee Create(
        string name,
        int age,
        DateTime createdAt,
        string email,
        string address,
        Gender gender,
        decimal salary,
        int departmentId)
        => new (name, age, createdAt, email, address, gender, salary, departmentId);

    public void Fire() => this.IsActive = false;
}
