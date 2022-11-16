using System;
using Lib.Core.FeatureFlags;
using Lib.Core.Rounding;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using NSubstitute;
using Xunit;

namespace Lib.Tests.Core.Rounding
{
    public class RoundingServiceTests
    {
        [Fact]
        public void Retrieve_multiple_rounding()
        {
            var feature = new Feature
                { IsRounding = true, RoundingDigit = 2, RoundingStrategy = MidpointRounding.ToEven };
            var service = Substitute.For<IOrganizationService>();
            var entity = new Entity
            {
                Id = Guid.NewGuid(),
                ["attribute1"] = new Money(3.56879m),
                ["attribute2"] = new Money(5.4321m)
            };
            service.RetrieveMultiple(Arg.Any<QueryBase>()).Returns(new EntityCollection
            {
                Entities = { entity }
            });

            var roundingService = new RoundingService(service, feature);
            var result = roundingService.RetrieveMultiple(new QueryExpression());
            Assert.Equal(entity.Id, result.Entities[0].Id);
            Assert.Equal(3.57m, result.Entities[0].GetAttributeValue<Money>("attribute1").Value);
            Assert.Equal(5.43m, result.Entities[0].GetAttributeValue<Money>("attribute2").Value);
        }
    }
}
