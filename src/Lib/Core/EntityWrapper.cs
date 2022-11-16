using Lib.Extensions;
using Microsoft.Xrm.Sdk;

namespace Lib.Core
{
    public class EntityWrapper : IEntityWrapper<Entity>
    {
        public EntityWrapper(Entity target, Entity initial)
        {
            Initial = initial;
            Target = target;
        }

        public Entity Initial { get; }
        public Entity Target { get; }

        public Entity Latest => Get();
        private Entity Get()
        {
            var entity = Initial.Clone();
            foreach (var targetAttribute in Target.Attributes)
            {
                entity[targetAttribute.Key] = targetAttribute.Value;
            }

            return entity;
        }
    }
    public class EntityWrapper<TEntity> : EntityWrapper, IEntityWrapper<TEntity>
        where TEntity : Entity
    {
        public EntityWrapper(Entity target, Entity initial) : base(target, initial)
        {
        }

        public new TEntity Initial => base.Initial.ToEntity<TEntity>();
        public new TEntity Target => base.Target.ToEntity<TEntity>();
        public new TEntity Latest => base.Latest.ToEntity<TEntity>();
    }
}