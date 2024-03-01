using Application.Requests.VehicleRequests;
using Application.Responses.VehicleResponses;
using FluentResults;
using MediatR;

namespace Application.Features.CreateVehicle;

public class CreateVehicleCommand : IRequest<Result<VehicleResponse>>
{
    public CreateVehicleCommand(VehicleRequest request)
    {
        Request = request;
    }

    public VehicleRequest Request { get; }
}