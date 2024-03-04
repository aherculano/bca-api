using System.Diagnostics.CodeAnalysis;
using AutoFixture;
using Domain.Models.Auction;
using Domain.Models.Vehicle;
using Infrastructure.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace UnitTests.Infrastructure.Data.EntityFramework;

[ExcludeFromCodeCoverage]
public class RepositoryTestsBase : TestsBase, IDisposable
{
    public RepositoryTestsBase()
    {
        var options = new DbContextOptionsBuilder<AuctionDbContext>()
            .ConfigureWarnings(warnings =>
            {
                warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning);
                warnings.Ignore(InMemoryEventId.ChangesSaved);
            })
            .UseInMemoryDatabase("Auctions")
            .Options;
        AuctionDbContext = new AuctionDbContext(options);

        SeedVehicles();
        SeedAuctions();
    }

    public AuctionDbContext AuctionDbContext { get; }

    public void Dispose()
    {
        AuctionDbContext.Dispose();
    }

    private void SeedVehicles()
    {
        var suvs = Fixture.Build<Suv>().Without(x => x.Id).CreateMany(3);
        var sedans = Fixture.Build<Sedan>().Without(x => x.Id).CreateMany(3);
        var trucks = Fixture.Build<Truck>().Without(x => x.Id).CreateMany(3);
        AuctionDbContext.Vehicles.AddRange(suvs);
        AuctionDbContext.Vehicles.AddRange(sedans);
        AuctionDbContext.Vehicles.AddRange(trucks);
        AuctionDbContext.SaveChanges();
    }

    private void SeedAuctions()
    {
        var auctions = Fixture.Build<Auction>().Without(x => x.Id).CreateMany(3);
        AuctionDbContext.Auctions.AddRange(auctions);
        AuctionDbContext.SaveChanges();
    }
}