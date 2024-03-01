using FluentValidation;

namespace Application.Features.CreateAuction;

public class CreateAuctionCommandValidator : AbstractValidator<CreateAuctionCommand>
{
    public CreateAuctionCommandValidator()
    {
        RuleFor(x => x)
            .NotNull();

        RuleFor(x => x.VehicleUniqueIdentifier)
            .Must(x => x != Guid.Empty)
            .When(x => x is not null)
            .WithMessage("Invalid Vehicle Unique Identifier");
    }
}