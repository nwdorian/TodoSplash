namespace TodoSplash.Api.Todos;

public class Todo
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public bool IsComplete { get; set; }
}
