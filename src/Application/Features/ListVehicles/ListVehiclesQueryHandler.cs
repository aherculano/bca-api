using Application.Responses.VehicleResponses;
using Domain.Errors;
using Domain.FluentResults;
using Domain.Models.Vehicle.ValueObjects;
using Domain.Repositories;
using FluentResults;
using MediatR;

namespace Application.Features.ListVehicles;

public class ListVehiclesQueryHandler : IRequestHandler<ListVehiclesQuery, Result<VehicleListResponse>>
{
    private readonly IVehicleRepository _repository;

    public ListVehiclesQueryHandler(IVehicleRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<VehicleListResponse>> Handle(ListVehiclesQuery request,
        CancellationToken cancellationToken)
    {
        var parseResult = Enum.TryParse<VehicleType>(request.Request.Type, out var vehicleType);
        
        var result = await _repository.ListVehiclesAsync(
            parseResult? vehicleType : null,
            request.Request.Manufacturer,
            request.Request.Model,
            request.Request.Year);
        result.ThrowExceptionIfHasFailedResult();

        if (result.Value is null || result.Value?.Count() == 0)
            return Result.Fail(new NotFoundError("No Vehicles Were Found",
                "There Are No Vehicles With The Specified Search Criteria"));

        return Result.Ok(new VehicleListResponse(result.Value.Select(x => x.MapToResponse())));
    }
}