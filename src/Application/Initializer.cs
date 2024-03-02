using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Application.PipelineBehaviors;
using Domain.Services.Auctions;
using Domain.Validators;
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
        services.AddTransient<IAuctionService, AuctionService>();
        services.AddTransient<IValidatorService, ValidatorService>();
        services.AddMediatR(cfg => { cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()); });

        services.ConfigurePipelineBehavior();
        services.AddValidatorsFromAssembly(typeof(Initializer).Assembly);
        return services;
    }
}