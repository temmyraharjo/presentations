using FakeXrmEasy.Plugins;
using Lib.Features;
using Microsoft.Xrm.Sdk;
using Xunit;

namespace Lib.Tests.Features
{
    public class DemoApiBusinessTests : TestBase
    {
        [Fact]
        public void Create_calculation()
        {
            var json = "{'Name': 'Test', 'Discount': 100, 'Qty': 2, 'PricePerUnit': 200}";
            var testContext = InitializeFakedContext();
            var pluginContext = testContext.GetDefaultPluginContext();
            pluginContext.InputParameters["Input"] = json;

            testContext.ExecutePluginWith<PluginExecutor<DemoApiBusiness, Entity>>(pluginContext);

            Assert.NotNull(pluginContext.OutputParameters["Output"]);
        }
    }
}
