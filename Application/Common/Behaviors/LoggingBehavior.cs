using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            var stopwatch = Stopwatch.StartNew();

            _logger.LogInformation("Handling request: {RequestName} | Payload: {@Request}", requestName, request);

            try
            {
                var response = next();
                stopwatch.Stop();

                _logger.LogInformation("Handled request: {RequestName} | Response: {@Response} | Elapsed Time: {ElapsedMilliseconds} ms", requestName, response, stopwatch.ElapsedMilliseconds);

                return response;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex, "Error handling request: {RequestName} | Elapsed Time: {ElapsedMilliseconds} ms", requestName, stopwatch.ElapsedMilliseconds);
                throw;
            }
        }
    }
}
