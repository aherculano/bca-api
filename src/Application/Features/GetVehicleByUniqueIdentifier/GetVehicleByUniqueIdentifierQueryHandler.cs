using Application.Responses.VehicleResponses;
using Domain.Errors;
using Domain.FluentResults;
using Domain.Repositories;
using FluentResults;
using MediatR;

namespace Application.Features.GetVehicleByUniqueIdentifier;

public class
    GetVehicleByUniqueIdentifierQueryHandler : IRequestHandler<GetVehicleByUniqueIdentifierQuery,
    Result<VehicleResponse>>
{
    private readonly IVehicleRepository _repository;

    public GetVehicleByUniqueIdentifierQueryHandler(IVehicleRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<VehicleResponse>> Handle(GetVehicleByUniqueIdentifierQuery request,
        CancellationToken cancellationToken)
    {
        var vehicleResult = await _repository.GetVehicleByUniqueIdentifierAsync(request.UniqueIdentifier);

        vehicleResult.ThrowExceptionIfHasFailedResult();

        return vehicleResult.Value is not null
            ? Result.Ok(vehicleResult.Value.MapToResponse())
            : Result.Fail(new NotFoundError("Vehicle Not Found",
                $"There Is No Vehicle Registered With That ${request.UniqueIdentifier}"));
    }
}