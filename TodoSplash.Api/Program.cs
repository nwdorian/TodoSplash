using TodoSplash.Api;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddDatabaseContext(builder.Configuration);

WebApplication app = builder.Build();
await app.RunAsync();
