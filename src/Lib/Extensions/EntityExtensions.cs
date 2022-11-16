using System;
using Microsoft.Xrm.Sdk;

namespace Lib.Extensions
{
    public static class EntityExtensions
    {
        public static TEntity Clone<TEntity>(this TEntity entity)
            where TEntity : Entity
        {
            var clone = Activator.CreateInstance<TEntity>();
            clone.Id = entity.Id;
            clone.LogicalName = entity.LogicalName;
            clone.RowVersion = entity.RowVersion;

            foreach (var entityAttribute in entity.Attributes)
            {
                clone[entityAttribute.Key] = entityAttribute.Value;
            }

            return clone;
        }
    }
}
