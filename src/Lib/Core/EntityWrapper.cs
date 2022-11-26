using System.ComponentModel;
using System.Linq;
using Lib.Extensions;
using Microsoft.Xrm.Sdk;

namespace Lib.Core
{
    public class EntityWrapper : IEntityWrapper<Entity>
    {
        public EntityWrapper(Entity target, Entity initial)
        {
            Initial = target;
            Target = initial;
        }

        public Entity Initial { get; }
        public Entity Target { get; }

        public Entity Latest => Get(Initial, Target);

        protected Entity Get(Entity initial, Entity target)
        {
            var entity = initial.Clone();
            foreach (var targetAttribute in target.Attributes)
            {
                entity[targetAttribute.Key] = targetAttribute.Value;
            }

            return entity;
        }
    }

    public class EntityWrapper<TEntity> : EntityWrapper, IEntityWrapper<TEntity>
        where TEntity : Entity
    {
        public EntityWrapper(TEntity target, TEntity initial) : base(target, initial)
        {
            IsAssignableNotifyPropertyChanged =
                typeof(TEntity).GetInterfaces().Any(x => x == typeof(INotifyPropertyChanged));

            if (!IsAssignableNotifyPropertyChanged)
            {
                Initial = initial;
                Target = target;
                return;
            }

            var targetWithPropertyChanged = (INotifyPropertyChanged)target;
            targetWithPropertyChanged.PropertyChanged += LatestPropertyChanged_PropertyChanged;
            Initial = initial.ToEntity<TEntity>();
            Target = targetWithPropertyChanged as TEntity;
            _latest = Get(Initial, Target).ToEntity<TEntity>();
        }

        private void LatestPropertyChanged_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Latest[e.PropertyName.ToLower()] = ((TEntity)sender)[e.PropertyName.ToLower()];
        }

        public bool IsAssignableNotifyPropertyChanged { get; }

        public new TEntity Initial { get; }

        public new TEntity Target { get; }

        private readonly TEntity _latest;

        public new TEntity Latest
        {
            get
            {
                if (IsAssignableNotifyPropertyChanged)
                {
                    return _latest;
                }

                return Get(Initial, Target) as TEntity;
            }
        }
    }
}