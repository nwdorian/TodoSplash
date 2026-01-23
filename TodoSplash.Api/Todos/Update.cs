using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TodoSplash.Api.Core.Primitives;
using TodoSplash.Api.Data;
using TodoSplash.Api.Endpoints;
using TodoSplash.Api.Infrastructure;

namespace TodoSplash.Api.Todos;

public class Update(TodoContext context)
{
    public record Request(string Name, bool IsComplete);

    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(t => t.Name)
                .NotEmpty()
                .WithMessage("Name is required.")
                .MaximumLength(100)
                .WithMessage("Name can't be longer than 100 characters.");
        }
    }

    public async Task<Result> Handle(int id, Request request, CancellationToken cancellationToken)
    {
        Todo? todo = await context.Todos.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        if (todo is null)
        {
            return TodoErrors.NotFoundById(id);
        }

        todo.Name = request.Name;
        todo.IsComplete = request.IsComplete;

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut(
                    TodoEndpoints.Routes.Update,
                    async (int id, Request request, Update useCase, CancellationToken cancellationToken) =>
                    {
                        Result result = await useCase.Handle(id, request, cancellationToken);
                        return result.Match(Results.NoContent, CustomResults.Problem);
                    }
                )
                .WithTags(TodoEndpoints.Tag)
                .WithRequestValidation<Request>();
        }
    }
}
