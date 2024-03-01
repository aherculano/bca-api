using Application.Errors;
using Application.FluentResults;
using Application.Requests.VehicleRequests;
using Application.Responses.VehicleResponses;
using Domain.Models.Vehicle;
using Domain.Repositories;
using FluentResults;
using MediatR;

namespace Application.Features.CreateVehicle;

public class CreateVehicleCommandHandler: IRequestHandler<CreateVehicleCommand, Result<VehicleResponse>>
{
    private readonly IVehicleRepository _repository;

    public CreateVehicleCommandHandler(IVehicleRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<Result<VehicleResponse>> Handle(CreateVehicleCommand request, CancellationToken cancellationToken)
    {
        var existingVehicle = await _repository
            .GetVehicleByUniqueIdentifierAsync(request.Request.UniqueIdentifier);

        if (existingVehicle.ThrowExceptionIfHasFailedResult().Value is not null)
        {
            return Result.Fail(new AlreadyExistsError("Vehicle Already Exists", $"One Vehicle Is Already Registered With That ${request.Request.UniqueIdentifier}"));
        }

        var domainVehicle = request.Request.MapToDomain();
        
        var createResult = await _repository.CreateVheicleAsync(domainVehicle);

        createResult.ThrowExceptionIfHasFailedResult();

        return Result.Ok(domainVehicle.MapToResponse());
    }
}