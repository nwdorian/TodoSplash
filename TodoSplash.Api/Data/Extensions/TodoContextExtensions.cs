using Microsoft.EntityFrameworkCore;

namespace TodoSplash.Api.Data.Extensions;

public static class TodoContextExtensions
{
    public static async Task ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        using TodoContext context = scope.ServiceProvider.GetRequiredService<TodoContext>();

        await context.Database.MigrateAsync();
    }

    public static async Task SeedDatabase(this WebApplication app)
    {
        if (app.Environment.IsProduction())
        {
            return;
        }

        using IServiceScope scope = app.Services.CreateScope();

        TodoContext context = scope.ServiceProvider.GetRequiredService<TodoContext>();

        await SeedData.Handle(context);
    }
}
