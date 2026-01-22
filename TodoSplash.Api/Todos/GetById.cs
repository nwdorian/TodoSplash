using Microsoft.EntityFrameworkCore;
using TodoSplash.Api.Data;
using TodoSplash.Api.Endpoints;

namespace TodoSplash.Api.Todos;

public static class GetById
{
    public record Response(int Id, string Name, bool IsComplete);

    public class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("todos/{id}", Handler).WithTags("Todos");
        }

        public static async Task<IResult> Handler(int id, TodoContext context, CancellationToken cancellationToken)
        {
            Todo? todo = await context.Todos.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

            if (todo is null)
            {
                return TypedResults.NotFound();
            }

            return TypedResults.Ok(new Response(todo.Id, todo.Name, todo.IsComplete));
        }
    }
}
