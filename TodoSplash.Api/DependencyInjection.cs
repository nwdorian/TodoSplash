using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TodoSplash.Api.Data;
using TodoSplash.Api.Infrastructure;

namespace TodoSplash.Api;

public static class DependencyInjection
{
    public static void AddDatabaseContext(this IServiceCollection services, IConfiguration configuration)
    {
        string? connectionString =
            configuration.GetConnectionString("TodoSplashDb")
            ?? throw new InvalidOperationException("Connection string 'TodoSplashDb' was not found.");

        services.AddDbContext<TodoContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });
    }

    public static void AddFluentValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(Program).Assembly);
    }

    public static void AddCustomExceptionHandler(this IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails(configure =>
        {
            configure.CustomizeProblemDetails = context =>
            {
                context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);
            };
        });
    }
}
