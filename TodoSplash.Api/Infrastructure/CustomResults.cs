using TodoSplash.Api.Core.Primitives;

namespace TodoSplash.Api.Infrastructure;

/// <summary>
/// Provides custom result helpers for converting <see cref="Result"/> errors
/// into standardized HTTP problem responses.
/// </summary>
public static class CustomResults
{
    /// <summary>
    /// Creates a standardized <see cref="IResult"/> representing an HTTP problem response
    /// based on the specified <see cref="Result"/>.
    /// </summary>
    /// <param name="result">
    /// The result containing the error information.
    /// Must represent a failure; otherwise an <see cref="InvalidOperationException"/> is thrown.
    /// </param>
    /// <returns>
    /// An <see cref="IResult"/> containing a problem details response with
    /// appropriate title, detail, type, status code, and error metadata.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the provided result represents a successful operation.
    /// </exception>
    public static IResult Problem(Result result)
    {
        if (result.IsSuccess)
        {
            throw new InvalidOperationException();
        }

        return Results.Problem(
            title: GetTitle(result.Error),
            type: GetType(result.Error.Type),
            statusCode: GetStatusCode(result.Error.Type),
            extensions: GetErrors(result)
        );

        static string GetTitle(Error error)
        {
            return error.Type switch
            {
                ErrorType.Validation => error.Code,
                ErrorType.Problem => error.Code,
                ErrorType.NotFound => error.Code,
                ErrorType.Conflict => error.Code,
                _ => "Server failure",
            };
        }

        static string GetType(ErrorType errorType)
        {
            return errorType switch
            {
                ErrorType.Validation => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                ErrorType.Problem => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                ErrorType.NotFound => "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                ErrorType.Conflict => "https://tools.ietf.org/html/rfc7231#section-6.5.8",
                _ => "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            };
        }

        static int GetStatusCode(ErrorType errorType)
        {
            return errorType switch
            {
                ErrorType.Validation or ErrorType.Problem => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status500InternalServerError,
            };
        }

        static Dictionary<string, object?>? GetErrors(Result result)
        {
            if (result.Error is ValidationError validationError)
            {
                return new Dictionary<string, object?>
                {
                    { "errors", validationError.Errors.Select(e => e.Description) },
                };
            }
            return new Dictionary<string, object?> { { "errors", new[] { result.Error.Description } } };
        }
    }
}
