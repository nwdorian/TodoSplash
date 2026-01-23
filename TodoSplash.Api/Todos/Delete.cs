using Microsoft.EntityFrameworkCore;
using TodoSplash.Api.Core.Primitives;
using TodoSplash.Api.Data;
using TodoSplash.Api.Endpoints;
using TodoSplash.Api.Infrastructure;

namespace TodoSplash.Api.Todos;

public class Delete(TodoContext context)
{
    public async Task<Result> Handle(int id, CancellationToken cancellationToken)
    {
        Todo? todo = await context.Todos.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        if (todo is null)
        {
            return TodoErrors.NotFoundById(id);
        }

        context.Todos.Remove(todo);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete(
                    TodoEndpoints.Routes.Delete,
                    async (int id, Delete useCase, CancellationToken cancellationToken) =>
                    {
                        Result result = await useCase.Handle(id, cancellationToken);
                        return result.Match(Results.NoContent, CustomResults.Problem);
                    }
                )
                .WithTags(TodoEndpoints.Tag);
        }
    }
}
