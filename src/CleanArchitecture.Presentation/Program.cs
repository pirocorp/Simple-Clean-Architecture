namespace CleanArchitecture.Presentation;

using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Infrastructure.Persistence;
using CleanArchitecture.Presentation.Extensions;

using Microsoft.EntityFrameworkCore;

public class Program
{
    private static string? sqlServerConnectionString;

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        ConfigureConfiguration(builder.Configuration);
        ConfigureServices(builder.Services, builder.Environment);

        var app = builder.Build();

        ConfigureMiddleware(app);
        ConfigureEndpoints(app);

        app.Run();
    }

    private static void ConfigureConfiguration(IConfiguration configuration)
    {
        sqlServerConnectionString = configuration.GetConnectionString("DefaultConnection");
    }

    private static void ConfigureServices(IServiceCollection services, IHostEnvironment env)
    {
        services
            .AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(sqlServerConnectionString));

        services.AddApplicationServices(
            typeof(IDbContext).Assembly,
            typeof(ApplicationDbContext).Assembly);

        services.AddInfrastructureServices();

        services.AddControllers();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }

    private static void ConfigureMiddleware(WebApplication app)
    {
        app.UseDatabaseMigrations<ApplicationDbContext>();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();
    }

    private static void ConfigureEndpoints(WebApplication app)
    {
        app.MapControllers();
    }
}
