using System;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Lib.Core
{
    public abstract class BusinessBase<TEntity>
        where TEntity: Entity
    {
        public ILocalPluginContext Context { get; }
        public IEntityWrapper<TEntity> Wrapper { get; }
        public IOrganizationService Service => Context.InitiatingUserService;

        public BusinessBase(ILocalPluginContext context)
        {
            Context = context;

            var target = GetTarget();
            var initial = GetInitial(target.LogicalName, target.Id);
            Wrapper = new EntityWrapper<TEntity>(target, initial);
        }

        private TEntity GetInitial(string logicalName, Guid id)
        {
            var initial =
                (Context.PluginExecutionContext.Stage == 40 && Context.PluginExecutionContext.PostEntityImages.Any()
                    ? Context.PluginExecutionContext.PostEntityImages.FirstOrDefault().Value
                    : Context.PluginExecutionContext.Stage < 30 &&
                      Context.PluginExecutionContext.PreEntityImages.Any()
                        ? Context.PluginExecutionContext.PreEntityImages.FirstOrDefault().Value
                        : null) ??
                Service.Retrieve(logicalName, id, new ColumnSet(true));

            return initial.ToEntity<TEntity>();
        }

        private TEntity GetTarget()
        {
            TEntity target;
            switch (Context.PluginExecutionContext.MessageName)
            {
                case "Create":
                case "Update":
                    target = ((Entity)Context.PluginExecutionContext.InputParameters["Target"]).ToEntity<TEntity>();
                    break;
                case "Delete":
                    var entityRef = (EntityReference)Context.PluginExecutionContext.InputParameters["Target"];
                    target = Service.Retrieve(entityRef.LogicalName, entityRef.Id, new ColumnSet(true))
                        .ToEntity<TEntity>();
                    break;
                    default:
                        throw new InvalidPluginExecutionException(
                            $"Plugin step {Context.PluginExecutionContext.MessageName} is not supported.");
            }

            return target;
        }

        public virtual void Execute()
        {
            HandleExecute();
        }

        public abstract void HandleExecute();
    }
}
