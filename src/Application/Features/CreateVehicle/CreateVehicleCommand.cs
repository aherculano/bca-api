using Application.Requests.VehicleRequests;
using Application.Responses.VehicleResponses;
using Domain.Models.Vehicle;
using FluentResults;
using MediatR;

namespace Application.Features.CreateVehicle;

public class CreateVehicleCommand : IRequest<Result<VehicleResponse>>
{
    public VehicleRequest Request { get; }

    public CreateVehicleCommand(VehicleRequest request)
    {
        Request = request;
    }
}