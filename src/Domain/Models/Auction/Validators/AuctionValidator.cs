using FluentValidation;

namespace Domain.Models.Auction.Validators;

public class AuctionValidator : AbstractValidator<Auction>
{
    public AuctionValidator()
    {
        RuleFor(x => x)
            .NotNull()
            .WithMessage("Auction Cannot Be Null");

        RuleFor(x => x.VehicleUniqueIdentifier)
            .Must(x => x != Guid.Empty)
            .When(x => x is not null)
            .WithMessage("Vehicle Unique Identifier Cannot Not Be Null");

        RuleFor(x => x.StartingBid)
            .GreaterThan(0)
            .When(x => x is not null)
            .WithMessage("Starting Bid Must Be Greater Then 0");
    }
}