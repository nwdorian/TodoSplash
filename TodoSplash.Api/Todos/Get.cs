using Microsoft.EntityFrameworkCore;
using TodoSplash.Api.Data;
using TodoSplash.Api.Endpoints;

namespace TodoSplash.Api.Todos;

public static class Get
{
    public record Response(int Id, string Name, bool IsComplete);

    public class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("todos", Handler).WithTags("Todos");
        }

        public static async Task<IResult> Handler(TodoContext context, CancellationToken cancellationToken)
        {
            IReadOnlyList<Response> todos = await context
                .Todos.AsNoTracking()
                .Select(t => new Response(t.Id, t.Name, t.IsComplete))
                .ToListAsync(cancellationToken);

            return TypedResults.Ok(todos);
        }
    }
}
