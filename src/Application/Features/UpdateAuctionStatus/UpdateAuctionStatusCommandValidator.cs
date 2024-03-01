using Domain.Models.Auction;
using FluentValidation;

namespace Application.Features.UpdateAuctionStatus;

public class UpdateAuctionStatusCommandValidator: AbstractValidator<UpdateAuctionStatusCommand>
{
    public UpdateAuctionStatusCommandValidator()
    {
        RuleFor(x => x)
            .NotNull();
        RuleFor(x => x.UniqueIdentifier)
            .Must(x => x != Guid.Empty)
            .WithMessage("Invalid Auction Unique Identifier");
        RuleFor(x => x.AuctionStatus)
            .NotEmpty()
            .WithMessage("AuctionStatus Cannot Be Empty");
        RuleFor(x => x.AuctionStatus)
            .Must(HaveValidStatus)
            .When(x => x.AuctionStatus != string.Empty);
    }

    private bool HaveValidStatus(string status)
    {
        switch (status)
        {
            case var _ when status.Equals(AuctionStatus.Open.ToString(), StringComparison.InvariantCultureIgnoreCase)
                || status.Equals(AuctionStatus.Closed.ToString(), StringComparison.InvariantCultureIgnoreCase):
                return true;
            default:
                return false;
        }
    }
}