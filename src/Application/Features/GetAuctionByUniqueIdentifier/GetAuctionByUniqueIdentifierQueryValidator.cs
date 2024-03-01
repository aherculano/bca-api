using FluentValidation;

namespace Application.Features.GetAuctionByUniqueIdentifier;

public class GetAuctionByUniqueIdentifierQueryValidator : AbstractValidator<GetAuctionByUniqueIdentifierQuery>
{
    public GetAuctionByUniqueIdentifierQueryValidator()
    {
        RuleFor(x => x.UniqueIdentifier)
            .NotNull()
            .WithMessage("Auction Unique Identifier Cannot Be Null");
        
        RuleFor(x => x.UniqueIdentifier)
            .Must(x => x != Guid.Empty)
            .When(x => x is not null)
            .WithMessage("Invalid Auction Unique Identifier");
    }
}