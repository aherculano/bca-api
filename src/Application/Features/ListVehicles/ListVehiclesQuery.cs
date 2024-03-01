using Application.Requests.ListVehicleRequests;
using Application.Responses.VehicleResponses;
using FluentResults;
using MediatR;

namespace Application.Features.ListVehicles;

public class ListVehiclesQuery : IRequest<Result<VehicleListResponse>>
{
    public ListVehiclesQuery(ListVehicleRequest request)
    {
        Request = request;
    }

    public ListVehicleRequest Request { get; }
}