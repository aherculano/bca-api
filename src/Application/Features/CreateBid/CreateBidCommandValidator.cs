using FluentValidation;

namespace Application.Features.CreateBid;

public class CreateBidCommandValidator: AbstractValidator<CreateBidCommand>
{
    public CreateBidCommandValidator()
    {
        RuleFor(x => x.Request)
            .NotNull()
            .WithMessage("Bid Cannot be Null");

        RuleFor(x => x.UniqueIdentifier)
            .Must(x => x != Guid.Empty)
            .When(x => x is not null)
            .WithMessage("Invalid Auction Unique Identifier");
        
        RuleFor(x => x.Request.BidValue)
            .GreaterThan(0)
            .When(x => x?.Request is not null)
            .WithMessage("BidValue Must Be Positive");

        RuleFor(x => x.Request.BidderName)
            .NotEmpty()
            .When(x => x?.Request is not null)
            .WithMessage("BidderName Cannot Be Empty");
    }
}