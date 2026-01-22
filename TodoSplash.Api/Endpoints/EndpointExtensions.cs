using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TodoSplash.Api.Infrastructure;

namespace TodoSplash.Api.Endpoints;

public static class EndpointExtensions
{
    public static void AddEndpoints(this IServiceCollection services)
    {
        services.AddEndpoints(Assembly.GetExecutingAssembly());
    }

    public static void AddEndpoints(this IServiceCollection services, Assembly assembly)
    {
        ServiceDescriptor[] serviceDescriptors = assembly
            .DefinedTypes.Where(type =>
                type is { IsAbstract: false, IsInterface: false } && type.IsAssignableTo(typeof(IEndpoint))
            )
            .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))
            .ToArray();

        services.TryAddEnumerable(serviceDescriptors);
    }

    public static void MapEndpoints(this WebApplication app, RouteGroupBuilder? routeGroupBuilder = null)
    {
        IEnumerable<IEndpoint> endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();

        IEndpointRouteBuilder builder = routeGroupBuilder is null ? app : routeGroupBuilder;

        foreach (IEndpoint endpoint in endpoints)
        {
            endpoint.MapEndpoint(builder);
        }
    }

    public static RouteHandlerBuilder WithRequestValidation<TRequest>(this RouteHandlerBuilder app)
    {
        return app.AddEndpointFilter<ValidationFilter<TRequest>>().ProducesValidationProblem();
    }
}
