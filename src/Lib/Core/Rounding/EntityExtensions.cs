using System;
using System.Globalization;
using System.Linq;
using System.Numerics;
using Lib.Core.FeatureFlags;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Lib.Core.Rounding
{
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
