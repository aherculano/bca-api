using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Application.PipelineBehaviors;
using FluentValidation;
using Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

[ExcludeFromCodeCoverage]
public static class Initializer
{
    public static IServiceCollection ConfigureApplication(this IServiceCollection services)
    {
        services.ConfigureInfrastructure();
        services.AddMediatR(cfg => { cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()); });

        services.ConfigurePipelineBehavior()
            .AddValidatorsFromAssembly(typeof(Initializer).Assembly);
        return services;
    }
}