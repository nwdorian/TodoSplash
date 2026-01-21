using TodoSplash.Api;
using TodoSplash.Api.Data.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddDatabaseContext(builder.Configuration);

WebApplication app = builder.Build();

await app.ApplyMigrations();

await app.RunAsync();
