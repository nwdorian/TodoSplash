using FluentValidation;
using FluentValidation.Results;
using TodoSplash.Api.Data;
using TodoSplash.Api.Endpoints;

namespace TodoSplash.Api.Todos;

public static class Create
{
    public record Request(string Name);

    public record Response(int Id, string Name, bool IsComplete);

    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(t => t.Name).MaximumLength(100).WithMessage("Name can't be longer than 100 characters");
        }
    }

    public class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("todos", Handler).WithTags("Todos");
        }

        public static async Task<IResult> Handler(
            Request request,
            IValidator<Request> validator,
            TodoContext context,
            CancellationToken cancellationToken
        )
        {
            ValidationResult validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return TypedResults.BadRequest(validationResult.Errors);
            }

            Todo todo = new() { Name = request.Name };

            context.Todos.Add(todo);
            await context.SaveChangesAsync(cancellationToken);

            return TypedResults.Ok(new Response(todo.Id, todo.Name, todo.IsComplete));
        }
    }
}
