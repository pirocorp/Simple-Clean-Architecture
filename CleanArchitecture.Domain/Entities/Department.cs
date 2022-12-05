namespace CleanArchitecture.Domain.Entities;

using System;
using System.Collections.Generic;

public class Department
{
    public Department()
    {
        this.Name = string.Empty;

        this.Employees = new List<Employee>();
    }

    public int Id { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool IsActive { get; set; }

    public string Name { get; set; }

    public ICollection<Employee> Employees { get; set; }
}
