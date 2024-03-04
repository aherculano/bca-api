using Domain.Models.Auction;
using Domain.Models.Vehicle;
using Infrastructure.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Migrations;

public class MigrationsDbContext : DbContext
{
    public MigrationsDbContext(DbContextOptions<MigrationsDbContext> options) : base(options)
    {
    }

    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<Auction> Auctions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AuctionConfiguration());
        modelBuilder.ApplyConfiguration(new VehicleConfiguration());
    }
}