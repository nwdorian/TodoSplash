namespace TodoSplash.Api.Core.Primitives;

/// <summary>
/// Contains extension methods for the result class.
/// </summary>
public static class ResultExtensions
{
    /// <summary>
    /// Matches the success status of the result to the corresponding functions.
    /// </summary>
    /// <typeparam name="TOut">The result type.</typeparam>
    /// <param name="result">The result.</param>
    /// <param name="onSuccess">The on-success function.</param>
    /// <param name="onFailure">The on-failure function.</param>
    /// <returns>
    /// The result of the on-success function if the result is a success result,
    /// otherwise the result of the failure result.
    /// </returns>
    public static TOut Match<TOut>(this Result result, Func<TOut> onSuccess, Func<Result, TOut> onFailure)
    {
        return result.IsSuccess ? onSuccess() : onFailure(result);
    }

    /// <summary>
    ///Matches the success status of the result to the corresponding functions.
    /// </summary>
    /// <typeparam name="TIn">The result type.</typeparam>
    /// <typeparam name="TOut">The output result type.</typeparam>
    /// <param name="result">The result.</param>
    /// <param name="onSuccess">The on-success function.</param>
    /// <param name="onFailure">The on-failure function.</param>
    /// <returns>
    /// The result of the on-success function if the result is a success result,
    /// otherwise the result of the failure result.
    /// </returns>
    public static TOut Match<TIn, TOut>(
        this Result<TIn> result,
        Func<TIn, TOut> onSuccess,
        Func<Result<TIn>, TOut> onFailure
    )
    {
        return result.IsSuccess ? onSuccess(result.Value) : onFailure(result);
    }
}
