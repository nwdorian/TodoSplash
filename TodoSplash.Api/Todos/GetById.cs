using Microsoft.EntityFrameworkCore;
using TodoSplash.Api.Core.Primitives;
using TodoSplash.Api.Data;
using TodoSplash.Api.Endpoints;
using TodoSplash.Api.Infrastructure;

namespace TodoSplash.Api.Todos;

public class GetById(TodoContext context)
{
    public record Response(int Id, string Name, bool IsComplete);

    public async Task<Result<Response>> Handle(int id, CancellationToken cancellationToken)
    {
        Todo? todo = await context.Todos.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

        if (todo is null)
        {
            return TodoErrors.NotFoundById(id);
        }

        return new Response(todo.Id, todo.Name, todo.IsComplete);
    }

    public class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet(
                    TodoEndpoints.Routes.GetById,
                    async (int id, GetById useCase, CancellationToken cancellationToken) =>
                    {
                        Result<Response> result = await useCase.Handle(id, cancellationToken);
                        return result.Match(Results.Ok, CustomResults.Problem);
                    }
                )
                .WithTags(TodoEndpoints.Tag);
        }
    }
}
