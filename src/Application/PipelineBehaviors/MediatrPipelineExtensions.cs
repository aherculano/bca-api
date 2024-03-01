using System.Diagnostics.CodeAnalysis;
using Application.Features.CreateAuction;
using Application.Features.CreateVehicle;
using Application.Features.GetAuctionByUniqueIdentifier;
using Application.Features.GetVehicleByUniqueIdentifier;
using Application.Features.UpdateAuctionStatus;
using Application.Responses.AuctionResponses;
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
            .AddTransient<IPipelineBehavior<CreateAuctionCommand, Result<AuctionResponse>>,
                ValidationBehavior<CreateAuctionCommand, AuctionResponse>>()
            .AddTransient<IPipelineBehavior<GetVehicleByUniqueIdentifierQuery, Result<VehicleResponse>>,
                ValidationBehavior<GetVehicleByUniqueIdentifierQuery, VehicleResponse>>()
            .AddTransient<IPipelineBehavior<GetAuctionByUniqueIdentifierQuery, Result<AuctionResponse>>,
                ValidationBehavior<GetAuctionByUniqueIdentifierQuery, AuctionResponse>>()
            .AddTransient<IPipelineBehavior<UpdateAuctionStatusCommand, Result<string>>,
                ValidationBehavior<UpdateAuctionStatusCommand, string>>();

        //TODO: Validator cannot be added for return Result<IEnumerable<T>>, this has to be fixed
    }
}