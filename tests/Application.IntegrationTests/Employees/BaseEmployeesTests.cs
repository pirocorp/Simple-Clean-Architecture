namespace Application.IntegrationTests.Employees;

using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Enums;

using System.Diagnostics.CodeAnalysis;

public class BaseEmployeesTests : BaseTests
{
    public IList<Employee> Employees { get; set; }

    public BaseEmployeesTests()
        : base()
    {
        this.InitializeEmployees();
    }

    protected Department CreateDepartment(string departmentName)
        => Department.Create(departmentName, this.DateTime);

    [MemberNotNull(nameof(Employees))]
    private void InitializeEmployees()
    {
        this.Employees = new List<Employee>
        {
            Employee.Create(
                "Zdravko Zdravkov",
                35,
                DateTime.Now, 
                "postakalka@abv.bg",
                "Valkendal 8, 1853 Strombeek-Bever, Grimbergen",
                Gender.Male,
                25_000,
                1),
            Employee.Create(
                "Asen Zlatarov",
                33,
                DateTime.Now, 
                "pirocorp@abv.bg",
                "Valkendal 8, 1853 Strombeek-Bever, Grimbergen",
                Gender.Male,
                15_000,
                1),
        };
    }
}
