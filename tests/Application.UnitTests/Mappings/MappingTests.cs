﻿namespace Application.UnitTests.Mappings;

using System.Runtime.Serialization;

using AutoMapper;

using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Application.Departments.Commands.CreateDepartment;
using CleanArchitecture.Application.Departments.Commands.DeleteDepartment;
using CleanArchitecture.Application.Departments.Commands.RenameDepartment;
using CleanArchitecture.Application.Departments.Queries.GetDepartments;
using CleanArchitecture.Application.Departments.Queries.GetDepartmentsById;
using CleanArchitecture.Application.Employees;
using CleanArchitecture.Domain.Entities;

public class MappingTests
{
    private readonly IConfigurationProvider configuration;
    private readonly IMapper mapper;

    public MappingTests()
    {
        this.configuration = new MapperConfiguration(
            config 
                => config.AddProfile<MappingProfile>());

        this.mapper = this.configuration.CreateMapper();
    }

    [Test]
    public void ShouldHaveValidConfiguration()
    {
        this.configuration.AssertConfigurationIsValid();
    }

    [Test]
    [TestCase(typeof(Department), typeof(CreateDepartmentDetailsDto))]
    [TestCase(typeof(Department), typeof(DeleteDepartmentDetailsDto))]
    [TestCase(typeof(Department), typeof(RenameDepartmentDetailsDto))]
    [TestCase(typeof(Department), typeof(DepartmentFullDetailsDto))]
    [TestCase(typeof(Department), typeof(DepartmentListingDto))]
    [TestCase(typeof(Employee), typeof(EmployeeListingDto))]
    [TestCase(typeof(Employee), typeof(EmployeeDetailsDto))]
    [TestCase(typeof(Employee), typeof(EmployeeDto))]
    public void ShouldSupportMappingFromSourceToDestination(Type source, Type destination)
    {
        var instance = this.GetInstanceOf(source);

        this.mapper.Map(instance, source, destination);
    }

    private object GetInstanceOf(Type type)
        => type.GetConstructor(Type.EmptyTypes) != null 
            ? Activator.CreateInstance(type)! 
            : FormatterServices.GetUninitializedObject(type);
}
