using System.Diagnostics.CodeAnalysis;
using Domain.Repositories;
using Infrastructure.Data.EntityFramework.Repositories;
using Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Data.EntityFramework;

[ExcludeFromCodeCoverage]
public static class SqlExtensions
{
    public static IServiceCollection ConfigureSql(this IServiceCollection services)
    {
        var settings = services.BuildServiceProvider().GetService<SqlSettings>();
        services.AddDbContext<AuctionDbContext>(
            options => options.UseSqlServer(settings.ConnectionString));
        services.AddTransient<IVehicleRepository, VehicleRepository>();
        services.AddTransient<IAuctionRepository, AuctionRepository>();
        return services;
    }
}