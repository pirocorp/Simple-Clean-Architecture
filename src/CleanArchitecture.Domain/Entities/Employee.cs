namespace CleanArchitecture.Domain.Entities;

using System;

using CleanArchitecture.Domain.Enums;
using CleanArchitecture.Domain.ValueObjects;

public class Employee
{
    public Employee(
        int id, 
        Address address, 
        int age, 
        DateTime createdAt, 
        string email, 
        Gender gender, 
        bool isActive, 
        string name, 
        decimal salary, 
        int departmentId, 
        Department department)
    {
        Id = id;
        Address = address;
        Age = age;
        CreatedAt = createdAt;
        Email = email;
        Gender = gender;
        IsActive = isActive;
        Name = name;
        Salary = salary;
        DepartmentId = departmentId;
        Department = department;
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

        this.Department = null!;
    }

    public int Id { get; }

    public Address Address { get; private set; }

    public int Age { get; }

    public DateTime CreatedAt { get; }

    public string Email { get; private set; }

    public Gender Gender { get; }

    public bool IsActive { get; private set; }

    public string Name { get; }

    public decimal Salary { get; private set; }

    public int DepartmentId { get; private set; }

    public Department Department { get; }

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

    public void ChangeDepartment(int departmentId) => this.DepartmentId = departmentId;

    public void Fire() => this.IsActive = false;

    public void UpdateFile(string address, string email)
    {
        this.Address = Address.From(address);
        this.Email = email;
    }

    public void UpdateSalary(decimal newSalary) => this.Salary = newSalary;
}
