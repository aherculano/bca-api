using System.Diagnostics.CodeAnalysis;
using Application.Features.CreateVehicle;
using Application.Features.GetVehicleByUniqueIdentifier;
using Application.Features.ListVehicles;
using Application.Requests.ListVehicleRequests;
using Application.Responses.VehicleResponses;
using FluentResults;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application.PipelineBehaviors;

[ExcludeFromCodeCoverage]
public static class MediatrPipelineExtensions
{
    public static IServiceCollection ConfigurePipelineBehavior(this IServiceCollection services)
    {
        return services
            .AddTransient<IPipelineBehavior<CreateVehicleCommand, Result<VehicleResponse>>,
                ValidationBehavior<CreateVehicleCommand, VehicleResponse>>()
            .AddTransient<IPipelineBehavior<GetVehicleByUniqueIdentifierQuery, Result<VehicleResponse>>,
                ValidationBehavior<GetVehicleByUniqueIdentifierQuery, VehicleResponse>>();
        
        //TODO: Validator cannot be added for return Result<IEnumerable<T>>, this has to be fixed
    }
}