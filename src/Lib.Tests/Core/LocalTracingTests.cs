using System;
using System.Linq;
using Lib.Core;
using Lib.Core.FeatureFlags;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.PluginTelemetry;
using NSubstitute;
using Xunit;

namespace Lib.Tests.Core
{
    public class LocalTracingTests
    {
        [Fact]
        public void Check_feature_off()
        {
            var serviceProvider = Substitute.For<IServiceProvider>();
            var tracingService = Substitute.For<ITracingService>();
            var logger = Substitute.For<ILogger>();

            serviceProvider.GetService(typeof(IExecutionContext))
                .Returns(Substitute.For<IExecutionContext>());
            serviceProvider.GetService(typeof(ITracingService))
                .Returns(tracingService);
            serviceProvider.GetService(typeof(ILogger))
                .Returns(logger);
            serviceProvider.GetService(typeof(IPluginExecutionContext))
                .Returns(Substitute.For<IPluginExecutionContext>());
            serviceProvider.GetService(typeof(IOrganizationService))
                .Returns(Substitute.For<IOrganizationService>());
            serviceProvider.GetService(typeof(IOrganizationServiceFactory))
                .Returns(Substitute.For<IOrganizationServiceFactory>());

            var localPluginContext = new LocalPluginContext(serviceProvider, new Feature
            {
                IsLog = false,
                IsAzureApplicationInsight = false,
                IsPluginTraceLog = false
            });

            localPluginContext.Trace("Hello");

            Assert.False(tracingService.ReceivedCalls().Any());
            Assert.False(logger.ReceivedCalls().Any());
        }

        [Fact]
        public void Check_feature_on()
        {
            var serviceProvider = Substitute.For<IServiceProvider>();
            var tracingService = Substitute.For<ITracingService>();
            var logger = Substitute.For<ILogger>();

            serviceProvider.GetService(typeof(IExecutionContext))
                .Returns(Substitute.For<IExecutionContext>());
            serviceProvider.GetService(typeof(ITracingService))
                .Returns(tracingService);
            serviceProvider.GetService(typeof(ILogger))
                .Returns(logger);
            serviceProvider.GetService(typeof(IPluginExecutionContext))
                .Returns(Substitute.For<IPluginExecutionContext>());
            serviceProvider.GetService(typeof(IOrganizationService))
                .Returns(Substitute.For<IOrganizationService>());
            serviceProvider.GetService(typeof(IOrganizationServiceFactory))
                .Returns(Substitute.For<IOrganizationServiceFactory>());

            var localPluginContext = new LocalPluginContext(serviceProvider, new Feature
            {
                IsLog = true,
                IsAzureApplicationInsight = true,
                IsPluginTraceLog = true
            });

            localPluginContext.Trace("Hello");

            Assert.True(tracingService.ReceivedCalls().Any());
            Assert.True(logger.ReceivedCalls().Any());
        }
    }
}