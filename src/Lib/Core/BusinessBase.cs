using System;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Lib.Core
{
    public abstract class BusinessBase<TEntity>
        where TEntity : Entity
    {
        public ILocalPluginContext Context { get; }
        private IEntityWrapper<TEntity> _wrapper;

        public IEntityWrapper<TEntity> Wrapper
        {
            get
            {
                if (_wrapper == null)
                {
                    var target = Context.GetTarget<TEntity>();
                    var initial = GetInitial(target.LogicalName, target.Id);
                    _wrapper = new EntityWrapper<TEntity>(target, initial);
                }

                return _wrapper;
            }
        }

        public IOrganizationService Service => Context.InitiatingUserService;

        protected BusinessBase(ILocalPluginContext context)
        {
            Context = context;
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
                (Context.PluginExecutionContext.Stage < 30 && Context.PluginExecutionContext.MessageName == "Create" ? 
                    (Entity)Context.PluginExecutionContext.InputParameters["Target"] :
                    Service.Retrieve(logicalName, id, new ColumnSet(true)));

            return initial.ToEntity<TEntity>();
        }

        public virtual void Execute()
        {
            HandleExecute();
        }

        public abstract void HandleExecute();
    }
}
