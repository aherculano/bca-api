using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.PipelineBehaviors;

public class LoggingPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly ILogger<LoggingPipelineBehavior<TRequest, TResponse>> _logger;

    public LoggingPipelineBehavior(ILogger<LoggingPipelineBehavior<TRequest,TResponse>> logger)
    {
        _logger = logger;
    }
    
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation($"Handling {typeof(TRequest).Name}");
        
            var response = await next();
        
            _logger.LogInformation($"Handled {typeof(TResponse).Name}");

            return response;
        }
        catch (Exception e)
        {
            _logger.LogError($"Error handling {typeof(TRequest).Name}", e.Message);
            throw;
        }
    }
}