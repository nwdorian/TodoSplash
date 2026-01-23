namespace TodoSplash.Api.Endpoints;

public static class TodoEndpoints
{
    public const string Tag = "Todos";

    public static class Routes
    {
        public const string Get = "todos";
        public const string GetById = "todos/{id}";
        public const string Create = "todos";
        public const string Delete = "todos/{id}";
        public const string Update = "todos/{id}";
    }
}
