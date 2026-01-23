using FluentValidation;
using TodoSplash.Api.Data;
using TodoSplash.Api.Endpoints;

namespace TodoSplash.Api.Todos;

public class Create(TodoContext context)
{
    public record Request(string Name);

    public record Response(int Id, string Name, bool IsComplete);

    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(t => t.Name)
                .NotEmpty()
                .WithMessage("Name is required.")
                .MaximumLength(100)
                .WithMessage("Name can't be longer than 100 characters");
        }
    }

    public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
    {
        Todo todo = new() { Name = request.Name };

        context.Todos.Add(todo);
        await context.SaveChangesAsync(cancellationToken);

        return new Response(todo.Id, todo.Name, todo.IsComplete);
    }

    public class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost(
                    TodoEndpoints.Routes.Create,
                    async (Request request, Create useCase, CancellationToken cancellationToken) =>
                        await useCase.Handle(request, cancellationToken)
                )
                .WithTags(TodoEndpoints.Tag)
                .WithRequestValidation<Request>();
        }
    }
}
