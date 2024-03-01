using Application.Responses.VehicleResponses;
using Domain.Errors;
using Domain.FluentResults;
using Domain.Repositories;
using FluentResults;
using MediatR;

namespace Application.Features.ListVehicles;

public class ListVehiclesQueryHandler : IRequestHandler<ListVehiclesQuery, Result<IEnumerable<VehicleResponse>>>
{
    private readonly IVehicleRepository _repository;

    public ListVehiclesQueryHandler(IVehicleRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<IEnumerable<VehicleResponse>>> Handle(ListVehiclesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _repository.ListVehiclesAsync(
            request.Request.Type,
            request.Request.Manufacturer,
            request.Request.Model,
            request.Request.Year);
        result.ThrowExceptionIfHasFailedResult();

        if (result.Value is null || result.Value?.Count() == 0)
            return Result.Fail(new NotFoundError("No Vehicles Were Found",
                "There Are No Vehicles With The Specified Search Criteria"));

        return Result.Ok(result.Value.Select(x => x.MapToResponse()));
    }
}