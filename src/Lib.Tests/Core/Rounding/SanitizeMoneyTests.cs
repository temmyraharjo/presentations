using System;
using Lib.Core.FeatureFlags;
using Lib.Core.Rounding;
using Microsoft.Xrm.Sdk;
using Xunit;

namespace Lib.Tests.Core.Rounding
{
    public class SanitizeMoneyTests
    {
        [Fact]
        public void Entity_rounding_two_digit()
        {
            var feature = new Feature
                { IsRounding = true, RoundingDigit = 2, RoundingStrategy = MidpointRounding.ToEven };
            var entity = new Entity
            {
                Id = Guid.NewGuid(),
                ["attribute1"] = new Money(3.56879m),
                ["attribute2"] = new Money(5.4321m)
            };

            entity.SanitizeMoney(feature);

            Assert.Equal(3.57m, entity.GetAttributeValue<Money>("attribute1").Value);
            Assert.Equal(5.43m, entity.GetAttributeValue<Money>("attribute2").Value);
        }
    }
}
