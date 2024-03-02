using System.Diagnostics.CodeAnalysis;
using Infrastructure.Data.EntityFramework;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

[ExcludeFromCodeCoverage]
public static class Initializer
{
    public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services)
    {
        services.ConfigureSql();
        return services;
    }
}