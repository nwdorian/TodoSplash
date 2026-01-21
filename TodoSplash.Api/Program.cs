using TodoSplash.Api;
using TodoSplash.Api.Data.Extensions;
using TodoSplash.Api.Endpoints;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddDatabaseContext(builder.Configuration);

builder.Services.AddEndpoints();

WebApplication app = builder.Build();

app.MapEndpoints();

await app.ApplyMigrations();

await app.RunAsync();
