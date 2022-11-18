using System;
using Lib.Core;
using Microsoft.Xrm.Sdk;

namespace Lib.Tests
{
    public class PluginExecutor<Business, TEntity> : PluginBase
        where Business : BusinessBase<TEntity>
        where TEntity : Entity
    {
        public PluginExecutor() : base(typeof(PluginExecutor<Business, TEntity>))
        {
        }

        protected override void ExecuteDataversePlugin(ILocalPluginContext localPluginContext)
        {
            var business = (Business)Activator.CreateInstance(typeof(Business), localPluginContext);
            business.Execute();
        }
    }
}