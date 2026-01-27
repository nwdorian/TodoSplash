using FluentValidation;
using FluentValidation.Results;
using TodoSplash.Api.Core.Primitives;

namespace TodoSplash.Api.Infrastructure;

public class ValidationFilter<TRequest>(IValidator<TRequest> validator) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        TRequest? request = context.Arguments.OfType<TRequest>().First();

        ValidationResult validationResult = await validator.ValidateAsync(request, context.HttpContext.RequestAborted);

        if (!validationResult.IsValid)
        {
            Result result = Result.Failure(
                new ValidationError(
                    validationResult
                        .Errors.Select(f => Error.Validation(code: f.ErrorCode, description: f.ErrorMessage))
                        .ToArray()
                )
            );
            return CustomResults.Problem(result);
        }

        return await next(context);
    }
}
