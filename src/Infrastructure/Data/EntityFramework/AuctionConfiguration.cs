using Domain.Models.Auction;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityFramework;

public class AuctionConfiguration : IEntityTypeConfiguration<Auction>
{
    public void Configure(EntityTypeBuilder<Auction> builder)
    {
        builder.OwnsMany(auction => auction.Bids);
        builder.Property(auction => auction.Status)
            .HasConversion(
                status => status.ToString(),
                status => (AuctionStatus)Enum.Parse(typeof(AuctionStatus), status));
    }
}