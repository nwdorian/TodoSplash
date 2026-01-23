using TodoSplash.Api.Core.Primitives;

namespace TodoSplash.Api.Todos;

public static class TodoErrors
{
    public static Error NotFoundById(int id) =>
        Error.NotFound("Todo.NotFoundById", $"The todo item with Id = {id} was not found.");
}
