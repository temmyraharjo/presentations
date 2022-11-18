using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Lib.Core
{
    public static class LocalPluginExecutionContextExtensions
    {
        public static TEntity GetTarget<TEntity>(this ILocalPluginContext context)
            where TEntity : Entity
        {
            TEntity target;
            switch (context.PluginExecutionContext.MessageName)
            {
                case "Create":
                case "Update":
                    target = ((Entity)context.PluginExecutionContext.InputParameters["Target"]).ToEntity<TEntity>();
                    break;
                case "Delete":
                    var entityRef = (EntityReference)context.PluginExecutionContext.InputParameters["Target"];
                    target = context.InitiatingUserService.Retrieve(entityRef.LogicalName, entityRef.Id, new ColumnSet(true))
                        .ToEntity<TEntity>();
                    break;
                default:
                    throw new InvalidPluginExecutionException(
                        $"Plugin step {context.PluginExecutionContext.MessageName} is not supported.");
            }

            return target;
        }
    }
}