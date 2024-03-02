using FluentValidation;

namespace Domain.Models.Auction.ValueObjects.Validators;

public class BidValidator : AbstractValidator<Bid>
{
    public BidValidator()
    {
        RuleFor(x => x)
            .NotNull()
            .WithMessage("Bid Must Not Be Null");

        RuleFor(x => x.BidderName)
            .NotEmpty()
            .When(x => x is not null)
            .WithMessage("Bidder Name Cannot Be Null");

        RuleFor(x => x.BidValue)
            .GreaterThan(0)
            .When(x => x is not null)
            .WithMessage("Bidder Name Needs To Be Greater Then 0");
    }
}