using Application.Errors;
using Application.FluentResults;
using Application.Requests.ListVehicleRequests;
using Application.Responses.VehicleResponses;
using Domain.Models.Vehicle;
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
    
    public async Task<Result<IEnumerable<VehicleResponse>>> Handle(ListVehiclesQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.ListVehiclesAsync(BuildFilter(request.Request));
        result.ThrowExceptionIfHasFailedResult();

        if (result.Value is null || result.Value?.Count() == 0)
        {
            return Result.Fail(new VehicleNotFoundError());
        }

        return Result.Ok(result.Value.Select(x => x.MapToResponse()));
    }

    private Func<Vehicle, bool> BuildFilter(ListVehicleRequest request)
    {
        return (x => (request.Type == null || x.Type.Equals(request.Type, StringComparison.InvariantCultureIgnoreCase))
            && (request.Manufacturer == null || x.Manufacturer.Equals(request.Manufacturer, StringComparison.InvariantCultureIgnoreCase)
            && (request.Model == null || x.Model.Equals(request.Model, StringComparison.InvariantCultureIgnoreCase))
            && (request.Year == null || x.Year == request.Year)));
    }
}