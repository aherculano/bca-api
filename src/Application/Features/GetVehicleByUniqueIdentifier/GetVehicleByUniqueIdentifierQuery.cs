using Application.Responses.VehicleResponses;
using FluentResults;
using MediatR;

namespace Application.Features.GetVehicleByUniqueIdentifier;

public class GetVehicleByUniqueIdentifierQuery : IRequest<Result<VehicleResponse>>
{
    public GetVehicleByUniqueIdentifierQuery(Guid uniqueIdentifier)
    {
        UniqueIdentifier = uniqueIdentifier;
    }

    public Guid UniqueIdentifier { get; }
}