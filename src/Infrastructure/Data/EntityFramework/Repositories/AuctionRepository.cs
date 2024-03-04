using Domain.Models.Auction;
using Domain.Repositories;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.EntityFramework.Repositories;

public class AuctionRepository : IAuctionRepository
{
    private readonly AuctionDbContext _dbContext;

    public AuctionRepository(AuctionDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<Auction>> CreateAuctionAsync(Auction auction)
    {
        try
        {
            await _dbContext.Database.BeginTransactionAsync();

            await _dbContext.Auctions.AddAsync(auction);

            await _dbContext.SaveChangesAsync();
            await _dbContext.Database.CommitTransactionAsync();

            return Result.Ok(auction);
        }
        catch (Exception e)
        {
            await _dbContext.Database.CurrentTransaction?.RollbackAsync();
            return Result.Fail(new Error(e.Message).CausedBy(e));
        }
    }

    public async Task<Result<bool>> UpdateAuctionAsync(Auction auction)
    {
        try
        {
            await _dbContext.Database.BeginTransactionAsync();

            _dbContext.Auctions.Entry(auction).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync();
            await _dbContext.Database.CommitTransactionAsync();

            return Result.Ok(true);
        }
        catch (Exception e)
        {
            await _dbContext.Database.CurrentTransaction?.RollbackAsync();
            return Result.Fail(new Error(e.Message).CausedBy(e));
        }
    }

    public async Task<Result<IEnumerable<Auction>>> GetAuctionsByVehicleUniqueIdentifierAsync(
        Guid vehicleUniqueIdentifer)
    {
        try
        {
            var auctions = await _dbContext.Auctions
                .Where(auction => auction.VehicleUniqueIdentifier == vehicleUniqueIdentifer)
                .ToListAsync();

            return Result.Ok(auctions.AsEnumerable());
        }
        catch (Exception e)
        {
            return Result.Fail(new Error(e.Message).CausedBy(e));
        }
    }

    public async Task<Result<Auction>> GetAuctionByUniqueIdentifierAsync(Guid uniqueIdentifier)
    {
        try
        {
            var auction = await _dbContext.Auctions
                .FirstOrDefaultAsync(x => x.UniqueIdentifier == uniqueIdentifier);

            return Result.Ok(auction);
        }
        catch (Exception e)
        {
            return Result.Fail(new Error(e.Message).CausedBy(e));
        }
    }
}