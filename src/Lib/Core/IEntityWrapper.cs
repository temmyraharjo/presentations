using Microsoft.Xrm.Sdk;

namespace Lib.Core
{
    public interface IEntityWrapper<TEntity> 
        where TEntity : Entity
    {
        TEntity Initial { get; }
        TEntity Target { get; }
        TEntity Latest { get; }
    }
}