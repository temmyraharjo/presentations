using System;
using FakeXrmEasy.Plugins;
using Lib.Entities;
using Lib.Features;
using Microsoft.Xrm.Sdk;
using Xunit;

namespace Lib.Tests.Features
{
    public class SetTotalTest : TestBase
    {
        [Fact]
        public void Create_calculation()
        {
            var target = new dev_Calculation
            {
                Id = Guid.NewGuid(),
                dev_Qty = 1,
                dev_Discount = new Money(100),
                dev_PricePerUnit = new Money(1000)
            };

            var testContext = InitializeFakedContext();
            var pluginContext = testContext.GetDefaultPluginContext();
            pluginContext.InputParameters["Target"] = target;

            testContext.ExecutePluginWith<PluginExecutor<SetTotal, dev_Calculation>>(pluginContext);

            Assert.Equal(900, target.dev_Total.Value);
        }
    }
}