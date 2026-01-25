using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TodoSplash.Api.Data;
using TodoSplash.Api.Infrastructure;
using TodoSplash.Api.Todos;

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

    public static void AddCorsVitePolicy(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(
                "AllowVite",
                policy => policy.WithOrigins("http://localhost:5173").AllowAnyMethod().AllowAnyHeader()
            );
        });
    }

    public static void AddTodoUseCases(this IServiceCollection services)
    {
        services.AddScoped<Get>();
        services.AddScoped<GetById>();
        services.AddScoped<Create>();
        services.AddScoped<Delete>();
        services.AddScoped<Update>();
    }
}
