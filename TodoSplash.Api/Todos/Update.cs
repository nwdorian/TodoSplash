using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using TodoSplash.Api.Data;
using TodoSplash.Api.Endpoints;

namespace TodoSplash.Api.Todos;

public static class Update
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

    public class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("todos/{id}", Handler).WithTags("Todos");
        }

        public static async Task<IResult> Handler(
            int id,
            Request request,
            TodoContext context,
            IValidator<Request> validator,
            CancellationToken cancellationToken
        )
        {
            ValidationResult validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return TypedResults.ValidationProblem(validationResult.ToDictionary());
            }

            Todo? todo = await context.Todos.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
            if (todo is null)
            {
                return TypedResults.NotFound();
            }

            todo.Name = request.Name;
            todo.IsComplete = request.IsComplete;

            await context.SaveChangesAsync(cancellationToken);

            return TypedResults.NoContent();
        }
    }
}
