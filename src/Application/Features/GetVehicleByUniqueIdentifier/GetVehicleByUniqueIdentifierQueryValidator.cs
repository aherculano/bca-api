using FluentValidation;

namespace Application.Features.GetVehicleByUniqueIdentifier;

public class GetVehicleByUniqueIdentifierQueryValidator : AbstractValidator<GetVehicleByUniqueIdentifierQuery>
{
    public GetVehicleByUniqueIdentifierQueryValidator()
    {
        RuleFor(x => x.UniqueIdentifier)
            .NotNull()
            .WithMessage("Vehicle Unique Identifier Cannot Be Null");

        RuleFor(x => x.UniqueIdentifier)
            .Must(x => x != Guid.Empty)
            .When(x => x is not null)
            .WithMessage("Invalid Vehicle Unique Identifier");
    }
}