using System;
using System.Globalization;
using System.Linq;
using System.Numerics;
using Lib.Core.FeatureFlags;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Lib.Core.Rounding
{
    public static class LocalPluginContextExtensions
    {
        public static void Rounding(this ILocalPluginContext context)
        {
            if (!context.Feature.IsRounding) return;
            var target = context.GetTarget<Entity>();

            target.SanitizeMoney(context.Feature);
        }

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
    public static class EntityExtensions
    {
        public static void SanitizeMoney(this Entity entity, Feature feature)
        {
            if (!feature.IsRounding) return;
            var attributes = entity.Attributes.ToArray();
            foreach (var attribute in attributes)
            {
                var valid = attribute.Value is Money;
                if (!valid) continue;

                var money = (Money)attribute.Value;
                if (money == null) continue;

                var numbers = money.Value
                    .ToString(CultureInfo.InvariantCulture)
                    .Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(BigInteger.Parse).ToArray();
                if (numbers.Length != 2) continue;
                if (numbers.LastOrDefault() == feature.RoundingDigit) continue;

                var result = Math.Round(money.Value, feature.RoundingDigit, feature.RoundingStrategy);
                entity[attribute.Key] = new Money(result);
            }
        }
    }
}
