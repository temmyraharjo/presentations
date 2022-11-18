using System;
using FakeXrmEasy.Abstractions;
using FakeXrmEasy.Plugins;
using Lib.Core;
using Lib.Entities;
using Microsoft.Xrm.Sdk;
using NSubstitute;
using Xunit;

namespace Lib.Tests.Core
{
    public class LocalPluginContextTests : TestBase
    {
        private ILocalPluginContext GetContext(IXrmFakedContext testContext)
        {
            var orgServiceFactory = Substitute.For<IOrganizationServiceFactory>();

            orgServiceFactory.CreateOrganizationService(Arg.Any<Guid?>())
                .Returns(testContext.GetOrganizationService());
            var serviceProvider = Substitute.For<IServiceProvider>();
            serviceProvider.GetService(typeof(IOrganizationServiceFactory))
                .Returns(orgServiceFactory);
            serviceProvider.GetService(typeof(IExecutionContext))
                .Returns(Substitute.For<IExecutionContext>());
            serviceProvider.GetService(typeof(IPluginExecutionContext))
                .Returns(testContext.GetDefaultPluginContext());


            return new LocalPluginContext(serviceProvider);
        }

        [Fact]
        public void Check_get_feature_property()
        {
            var entityDefinition = new EnvironmentVariableDefinition
            {
                Id = Guid.NewGuid(),
                SchemaName = "dev_FeatureFlag",
                DefaultValue = @"{'IsLog': true, 'IsAzureApplicationInsight': true, 'IsPluginTraceLog': true, 'RoundingStrategy': 'ToEven'}"
            };
            var entityDefinitionValue = new EnvironmentVariableValue()
            {
                Id = Guid.NewGuid(),
                EnvironmentVariableDefinitionId = entityDefinition.ToEntityReference(),
                Value = @"{'IsLog': true, 'IsAzureApplicationInsight': true, 'IsPluginTraceLog': false}"
            };

            var testContext = InitializeFakedContext(entityDefinition, entityDefinitionValue);

            var pluginBase = GetContext(testContext);
            Assert.True(pluginBase.Feature.IsLog);
            Assert.True(pluginBase.Feature.IsAzureApplicationInsight);
            Assert.False(pluginBase.Feature.IsPluginTraceLog);
            Assert.Equal((int)MidpointRounding.ToEven, (int)pluginBase.Feature.RoundingStrategy);
        }

        [Fact]
        public void Check_get_feature_property_without_environment_variable_value()
        {
            var entityDefinition = new EnvironmentVariableDefinition
            {
                Id = Guid.NewGuid(),
                SchemaName = "dev_FeatureFlag",
                DefaultValue = @"{'IsLog': true, 'IsAzureApplicationInsight': true, 'IsPluginTraceLog': true}"
            };

            var testContext = InitializeFakedContext(entityDefinition);

            var pluginBase = GetContext(testContext);
            Assert.True(pluginBase.Feature.IsLog);
            Assert.True(pluginBase.Feature.IsAzureApplicationInsight);
            Assert.True(pluginBase.Feature.IsPluginTraceLog);
        }
    }
}
