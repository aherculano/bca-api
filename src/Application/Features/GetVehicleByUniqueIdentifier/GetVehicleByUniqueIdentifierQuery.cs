using Application.Responses.VehicleResponses;
using FluentResults;
using MediatR;

namespace Application.Features.GetVehicleByUniqueIdentifier;

public class GetVehicleByUniqueIdentifierQuery : IRequest<Result<VehicleResponse>>
{
    public Guid UniqueIdentifier { get; }

    public GetVehicleByUniqueIdentifierQuery(Guid uniqueIdentifier)
    {
        UniqueIdentifier = uniqueIdentifier;
    }
}