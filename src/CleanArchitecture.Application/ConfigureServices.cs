namespace CleanArchitecture.Application;

using System.Reflection;

using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Services;

using Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddTransient<IDepartmentService, DepartmentService>();
        services.AddTransient<IEmployeeService, EmployeeService>();

        return services;
    }
}
