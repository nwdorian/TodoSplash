using Microsoft.EntityFrameworkCore;
using TodoSplash.Api.Todos;

namespace TodoSplash.Api.Data;

public static class SeedData
{
    public static async Task Handle(TodoContext context)
    {
        if (await context.Todos.AnyAsync())
        {
            return;
        }

        await PopulateTestData(context);
    }

    private static async Task PopulateTestData(TodoContext context)
    {
        context.Todos.AddRange(GenerateTodos());
        await context.SaveChangesAsync();
    }

    private static List<Todo> GenerateTodos()
    {
        return new List<Todo>()
        {
            new() { Name = "Exercise", IsComplete = true },
            new() { Name = "Study", IsComplete = false },
            new() { Name = "Dishes", IsComplete = true },
            new() { Name = "Laundry", IsComplete = false },
        };
    }
}
