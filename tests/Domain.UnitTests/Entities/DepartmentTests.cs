namespace Domain.UnitTests.Entities;

using CleanArchitecture.Domain.Entities;

using FluentAssertions;

public class DepartmentTests
{
    [Test]
    public void PublicConstructorWorksCorrectly()
    {
        var department = new Department();

        department.Name.Should().Be(string.Empty);
        department.Employees.Should().NotBeNull();
    }

    [Test]
    public void DepartmentIsCreatedCorrectly()
    {
        var name = "IT";
        var date = DateTime.Now;

        var department = Department.Create(name, date);

        department.Id.Should().Be(0);
        department.CreatedAt.Should().Be(date);
        department.IsActive.Should().Be(true);
        department.Name.Should().Be(name);
        department.Employees.Should().NotBeNull();
    }

    [Test]
    public void DepartmentIsRenamedCorrectly()
    {
        var name = "IT";
        var date = DateTime.Now;

        var department = Department.Create(name, date);
        department.Name.Should().Be(name);

        var newName = "HR";
        department.Rename(newName);

        department.Name.Should().Be(newName);
    }

    [Test]
    public void DepartmentDeleteWorksCorrectly()
    {
        var name = "IT";
        var date = DateTime.Now;

        var department = Department.Create(name, date);

        department.IsActive.Should().Be(true);

        department.Delete();

        department.IsActive.Should().Be(false);
    }
}
