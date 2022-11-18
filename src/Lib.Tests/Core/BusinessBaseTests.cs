using System;
using FakeXrmEasy.Plugins;
using Lib.Core;
using Lib.Entities;
using Lib.Extensions;
using Xunit;

namespace Lib.Tests.Core
{
    public class Business : BusinessBase<Contact>
    {
        public Business(ILocalPluginContext context) : base(context)
        {
        }

        public override void HandleExecute()
        {
            Context.PluginExecutionContext.OutputParameters["Result"] =
                $"{Wrapper.Latest.FirstName} {Wrapper.Latest.LastName}";
        }
    }

    public class BusinessBaseTests : TestBase
    {
        [Fact]
        public void Business_on_pre_create()
        {
            var target = new Contact
            {
                FirstName = "FirstName",
                LastName = "LastName"
            };

            var testContext = InitializeFakedContext();
            var pluginExecutionContext = testContext.GetDefaultPluginContext();
            pluginExecutionContext.MessageName = "Create";
            pluginExecutionContext.Depth = 20;
            pluginExecutionContext.InputParameters["Target"] = target;
            
            testContext.ExecutePluginWith<PluginExecutor<Business, Contact>>(pluginExecutionContext);
            Assert.Equal("FirstName LastName",
                pluginExecutionContext.OutputParameters["Result"].ToString());
        }

        [Fact]
        public void Business_on_pre_update()
        {
            var target = new Contact
            {
                Id = Guid.NewGuid(),
                FirstName = "FirstName"
            };

            var initial = target.Clone();
            initial.LastName = "LastName";

            var testContext = InitializeFakedContext(initial);
            var pluginExecutionContext = testContext.GetDefaultPluginContext();
            pluginExecutionContext.MessageName = "Update";
            pluginExecutionContext.Depth = 20;
            pluginExecutionContext.InputParameters["Target"] = target;

            testContext.ExecutePluginWith<PluginExecutor<Business, Contact>>(pluginExecutionContext);
            Assert.Equal("FirstName LastName",
                pluginExecutionContext.OutputParameters["Result"].ToString());
        }
    }
}
