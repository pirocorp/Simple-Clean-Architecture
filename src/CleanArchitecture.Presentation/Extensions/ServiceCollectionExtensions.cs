namespace CleanArchitecture.Presentation.Extensions;

using System.Reflection;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add all services in IServiceCollection Container from the target Assemblies
    /// </summary>
    /// <param name="services">IServiceCollection Container</param>
    /// <param name="assemblies">Assemblies from where to import dependencies</param>
    /// <returns>IServiceCollection Container with added services</returns>
    public static IServiceCollection AddServices(
        this IServiceCollection services, 
        params Assembly[] assemblies)
    {
        assemblies
            .SelectMany(a => a.GetTypes())
            .Where(t => t.IsClass
                        && t.GetInterfaces()
                            .Any(i => i.Name == $"I{t.Name}"))
            .Select(t => new 
            {
                Interface = t.GetInterface($"I{t.Name}"),
                Implementation = t
            })
            .Where(s => s.Interface is not null)
            .ToList()
            .ForEach(s => services.AddTransient(s.Interface!, s.Implementation));
           
        return services;
    }
}
