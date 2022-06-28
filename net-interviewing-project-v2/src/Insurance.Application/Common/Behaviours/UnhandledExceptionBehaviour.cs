using MediatR;
using Microsoft.Extensions.Logging;

namespace Insurance.Application.Common.Behaviours;

public class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly ILogger<TRequest> _logger;

    public UnhandledExceptionBehaviour(ILogger<TRequest> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        try
        {
            return await next();
        }
        catch (AggregateException ex)
        {
            var requestName = typeof(TRequest).Name;

            _logger.LogError(ex, "Insurance Request: Unhandled Exception for Request {Name} {@Request}", requestName, request);

            throw new AggregateException("can't fond product or product type Make Sure localhost:5002  is running");
        }
        catch (Exception ex)
        {
            var requestName = typeof(TRequest).Name;

            _logger.LogError(ex, "Insurance Request: Unhandled Exception for Request {Name} {@Request}", requestName, request);

            throw;
        }
    }
}
