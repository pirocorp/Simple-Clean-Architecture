namespace Domain.UnitTests.Entities;

using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Enums;
using CleanArchitecture.Domain.ValueObjects;

using FluentAssertions;

public class EmployeeTests
{
    [Test]
    public void PublicConstructorWorksCorrectly()
    {
        var employee = new Employee();

        employee.Address.ToString().Should().Be(Address.Empty.ToString());
        employee.Email.Should().Be(string.Empty);
        employee.Name.Should().Be(string.Empty);
        employee.Department.Should().Be(null);
    }

    [Test]
    public void EmployeeIsCreatedCorrectly()
    {
        var name = "Zdravko Zdravkov";
        var age = 30;
        var createdAt = DateTime.Now;
        var email = "postakalka.93@abv.bg";
        var address = "Valkendal 8, 1853 Strombeek-Bever, Grimbergen";
        var gender = Gender.Male;
        var salary = 25_000;
        var departmentId = 1;

        var employee = Employee.Create(
            name,
            age,
            createdAt,
            email,
            address,
            gender,
            salary,
            departmentId);

        employee.Address.ToString().Should().Be(address);
        employee.Age.Should().Be(age);
        employee.CreatedAt.Should().Be(createdAt);
        employee.Department.Should().Be(null);
        employee.DepartmentId.Should().Be(departmentId);
        employee.Email.Should().Be(email);
        employee.Gender.Should().Be(gender);
        employee.Id.Should().Be(0);
        employee.IsActive.Should().Be(true);
        employee.Name.Should().Be(name);
        employee.Salary.Should().Be(salary);
    }

    [Test]
    public void EmployeeFireWorksCorrectly()
    {
        var name = "Zdravko Zdravkov";
        var age = 30;
        var createdAt = DateTime.Now;
        var email = "postakalka.93@abv.bg";
        var address = "Valkendal 8, 1853 Strombeek-Bever, Grimbergen";
        var gender = Gender.Male;
        var salary = 25_000;
        var departmentId = 1;

        var employee = Employee.Create(
            name,
            age,
            createdAt,
            email,
            address,
            gender,
            salary,
            departmentId);

        employee.IsActive.Should().Be(true);
        employee.Fire();
        employee.IsActive.Should().Be(false);
    }
}
