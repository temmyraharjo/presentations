using System;
using Lib.Core.FeatureFlags;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.PluginTelemetry;

namespace Lib.Core
{
    public class LocalTracingService : ITracingService
    {
        private readonly ITracingService _tracingService;
        private readonly ILogger _logger;

        private DateTime _previousTraceTime;

        public LocalTracingService(IServiceProvider serviceProvider, Feature feature)
        {
            DateTime utcNow = DateTime.UtcNow;

            var context = (IExecutionContext)serviceProvider.GetService(typeof(IExecutionContext));

            DateTime initialTimestamp = context.OperationCreatedOn;

            if (initialTimestamp > utcNow)
            {
                initialTimestamp = utcNow;
            }
            _tracingService = feature.IsLog && feature.IsPluginTraceLog ? 
                (ITracingService)serviceProvider.GetService(typeof(ITracingService)) : null;
            _logger = feature.IsLog && feature.IsAzureApplicationInsight
                ? (ILogger)serviceProvider.GetService(typeof(ILogger))
                : null;

            _previousTraceTime = initialTimestamp;
        }

        public void Trace(string message, params object[] args)
        {
            var utcNow = DateTime.UtcNow;

            // The duration since the last trace.
            var deltaMilliseconds = utcNow.Subtract(_previousTraceTime).TotalMilliseconds;

            if (_tracingService != null)
            {
                _tracingService.Trace($"[+{deltaMilliseconds:N0}ms] - {string.Format(message, args)}");
            }

            if (_logger != null)
            {
                _logger.LogInformation($"[+{deltaMilliseconds:N0}ms] - {string.Format(message, args)}");
            }

            _previousTraceTime = utcNow;
        }
    }
}