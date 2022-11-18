using System;
using System.Runtime.CompilerServices;
using Lib.Core.FeatureFlags;
using Lib.Core.Rounding;
using Lib.Extensions;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Extensions;
using Newtonsoft.Json;

//using Microsoft.Xrm.Sdk.PluginTelemetry;

namespace Lib.Core
{
    public class LocalPluginContext : ILocalPluginContext
    {
        public IOrganizationService InitiatingUserService { get; }
        public IOrganizationService AdminService { get; }
        public IOrganizationService PluginUserService { get; }
        public IPluginExecutionContext PluginExecutionContext { get; }
        public IServiceEndpointNotificationService NotificationService { get; }
        public ITracingService TracingService { get; }
        public IServiceProvider ServiceProvider { get; }
        public IOrganizationServiceFactory OrgSvcFactory { get; }
        //public ILogger Logger { get; }

        public LocalPluginContext(IServiceProvider serviceProvider, Feature feature = null)
        {
            ServiceProvider = serviceProvider ?? throw new InvalidPluginExecutionException(nameof(serviceProvider));

            //Logger = serviceProvider.Get<ILogger>();

            PluginExecutionContext = serviceProvider.Get<IPluginExecutionContext>();

            NotificationService = serviceProvider.Get<IServiceEndpointNotificationService>();

            OrgSvcFactory = serviceProvider.Get<IOrganizationServiceFactory>();

            Feature = feature ?? GetFeature(OrgSvcFactory.CreateOrganizationService(null));

            PluginUserService = GetOrganizationService(PluginExecutionContext.UserId); // User that the plugin is registered to run as, Could be same as current user.

            InitiatingUserService = GetOrganizationService(PluginExecutionContext.InitiatingUserId); //User who's action called the plugin.

            AdminService = GetOrganizationService();

            TracingService = new LocalTracingService(serviceProvider, Feature);
        }

        private IOrganizationService GetOrganizationService(Guid? userId = null)
        {
            return new RoundingService(OrgSvcFactory.CreateOrganizationService(userId), Feature);
        }

        private Feature GetFeature(IOrganizationService service)
        {
            var feature = service.GetEnvironmentVariable("dev_FeatureFlag");
            if (feature == null) return new Feature();

            var json = feature.Attributes.Contains("ev.value")
                ? feature.GetAttributeValue<AliasedValue>("ev.value")?.Value as string :
                  feature.DefaultValue;
            return string.IsNullOrEmpty(json) ? new Feature() : JsonConvert.DeserializeObject<Feature>(json);
        }

        public void Trace(string message, [CallerMemberName] string method = null)
        {
            if (string.IsNullOrWhiteSpace(message) || TracingService == null)
            {
                return;
            }

            TracingService.Trace(method != null ? $"[{method}] - {message}" : $"{message}");
        }

        public Feature Feature { get; }
    }
}