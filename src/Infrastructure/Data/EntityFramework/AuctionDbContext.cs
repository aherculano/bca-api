using Domain.Models.Auction;
using Domain.Models.Vehicle;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.EntityFramework;

public class AuctionDbContext : DbContext
{
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<Auction> Auctions { get; set; }
    
    public AuctionDbContext(DbContextOptions<AuctionDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AuctionConfiguration());
        modelBuilder.ApplyConfiguration(new VehicleConfiguration());
    }
}