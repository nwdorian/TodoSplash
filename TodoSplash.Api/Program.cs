using TodoSplash.Api;
using TodoSplash.Api.Data.Extensions;
using TodoSplash.Api.Endpoints;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddDatabaseContext(builder.Configuration);
builder.Services.AddTodoUseCases();
builder.Services.AddCustomExceptionHandler();
builder.Services.AddFluentValidation();
builder.Services.AddCorsVitePolicy();
builder.Services.AddEndpoints();

WebApplication app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseCors("AllowVite");
app.UseExceptionHandler();
app.UseStatusCodePages();
app.MapEndpoints();

await app.ApplyMigrations();

await app.RunAsync();
