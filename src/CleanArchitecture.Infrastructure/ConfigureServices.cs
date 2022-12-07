namespace CleanArchitecture.Infrastructure;

using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Infrastructure.Persistence;
using CleanArchitecture.Infrastructure.Services;

using Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddTransient<IDateTimeService, DateTimeService>();
        services.AddScoped<IDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        return services;
    }
}
