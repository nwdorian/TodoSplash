using Microsoft.EntityFrameworkCore;
using TodoSplash.Api.Models;

namespace TodoSplash.Api.Data;

public class TodoContext(DbContextOptions<TodoContext> options) : DbContext(options)
{
    public DbSet<Todo> Todos { get; set; }
}
