using System.Diagnostics.CodeAnalysis;
using Domain.Repositories;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

[ExcludeFromCodeCoverage]
public static class Initializer
{
    public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IVehicleRepository, VehicleInMemoryRepository>();
        services.AddSingleton<IAuctionRepository, AuctionInMemoryRepository>();
        return services;
    }
}