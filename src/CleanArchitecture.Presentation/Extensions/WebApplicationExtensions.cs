namespace CleanArchitecture.Presentation.Extensions;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public static class WebApplicationExtensions
{
    public static IApplicationBuilder UseDatabaseMigrations<TDbContext>(
        this WebApplication app)
        where TDbContext : DbContext
    {
        if (app.Environment.IsProduction())
        {
            return app;
        }

        using var serviceScope = app.Services.CreateScope();

        var dbContext = serviceScope.ServiceProvider.GetRequiredService<TDbContext>();
        dbContext.Database.Migrate();

        return app;
    }
}
