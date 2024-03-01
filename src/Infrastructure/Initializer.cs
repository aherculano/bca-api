using Domain.Repositories;
using Domain.Services.Auctions;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class Initializer
{
    public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IVehicleRepository, VehicleInMemoryRepository>();
        services.AddSingleton<IAuctionRepository, AuctionInMemoryRepository>();
        services.AddTransient<IAuctionService, AuctionService>();
        return services;
    }
}