using Microsoft.EntityFrameworkCore;
using TodoSplash.Api.Data;
using TodoSplash.Api.Endpoints;

namespace TodoSplash.Api.Todos;

public static class Delete
{
    public class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("todos/{id}", Handler).WithTags("Todos");
        }

        public static async Task<IResult> Handler(int id, TodoContext context, CancellationToken cancellationToken)
        {
            Todo? todo = await context.Todos.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
            if (todo is null)
            {
                return TypedResults.NotFound();
            }

            context.Todos.Remove(todo);
            await context.SaveChangesAsync(cancellationToken);

            return TypedResults.NoContent();
        }
    }
}
