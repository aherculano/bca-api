using Application.Requests.VehicleRequests;
using Application.Responses.VehicleResponses;
using Domain.Errors;
using Domain.FluentResults;
using Domain.Repositories;
using Domain.Validators;
using FluentResults;
using MediatR;

namespace Application.Features.CreateVehicle;

public class CreateVehicleCommandHandler : IRequestHandler<CreateVehicleCommand, Result<VehicleResponse>>
{
    private readonly IVehicleRepository _repository;
    private readonly IValidatorService _validator;

    public CreateVehicleCommandHandler(
        IVehicleRepository repository,
        IValidatorService validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<Result<VehicleResponse>> Handle(CreateVehicleCommand request, CancellationToken cancellationToken)
    {
        var existingVehicle = await _repository
            .GetVehicleByUniqueIdentifierAsync(request.Request.UniqueIdentifier);

        if (existingVehicle.ThrowExceptionIfHasFailedResult().Value is not null)
            return Result.Fail(new AlreadyExistsError("Vehicle Already Exists",
                $"One Vehicle Is Already Registered With That ${request.Request.UniqueIdentifier}"));

        var domainVehicle = request.Request.MapToDomain();

        var validationResult = _validator.ValidateVehicle(domainVehicle);

        if (!validationResult.IsValid) return Result.Fail(new ValidationError(validationResult.Errors));

        var createResult = await _repository.CreateVheicleAsync(domainVehicle);

        createResult.ThrowExceptionIfHasFailedResult();

        return Result.Ok(domainVehicle.MapToResponse());
    }
}