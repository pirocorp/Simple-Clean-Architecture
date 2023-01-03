namespace CleanArchitecture.Application.Common.Behaviors;

using MediatR;
using Microsoft.Extensions.Logging;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        this.logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        this.logger
            .LogInformation("Request: {Name} {@Request}",
                requestName, request);

        var response = await next();
        var responseName = response?.GetType().Name;

        this.logger.LogInformation("Handled {Name} {@Response}",
            responseName, response);

        return response;
    }
}
