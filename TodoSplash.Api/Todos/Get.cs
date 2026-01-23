using Microsoft.EntityFrameworkCore;
using TodoSplash.Api.Data;
using TodoSplash.Api.Endpoints;

namespace TodoSplash.Api.Todos;

public class Get(TodoContext context)
{
    public record Response(int Id, string Name, bool IsComplete);

    public async Task<IReadOnlyList<Response>> Handle(CancellationToken cancellationToken)
    {
        IReadOnlyList<Response> todos = await context
            .Todos.AsNoTracking()
            .Select(t => new Response(t.Id, t.Name, t.IsComplete))
            .ToListAsync(cancellationToken);

        return todos;
    }

    public class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet(
                    TodoEndpoints.Routes.Get,
                    async (Get useCase, CancellationToken cancellationToken) => await useCase.Handle(cancellationToken)
                )
                .WithTags(TodoEndpoints.Tag);
        }
    }
}
