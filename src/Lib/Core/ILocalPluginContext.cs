using System;
using System.Runtime.CompilerServices;
using Lib.Core.FeatureFlags;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.PluginTelemetry;

namespace Lib.Core
{
    public interface ILocalPluginContext
    {
        IOrganizationService InitiatingUserService { get; }
        IOrganizationService AdminService { get; }
        IOrganizationService PluginUserService { get; }
        IPluginExecutionContext PluginExecutionContext { get; }
        IServiceEndpointNotificationService NotificationService { get; }
        ITracingService TracingService { get; }
        IServiceProvider ServiceProvider { get; }
        IOrganizationServiceFactory OrgSvcFactory { get; }

        //ILogger Logger { get;  }
        void Trace(string message, [CallerMemberName] string method = null);

        Feature Feature { get; }
    }
}