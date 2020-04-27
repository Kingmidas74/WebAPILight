using System.Collections.Generic;
using Contracts.Shared.Interfaces;

namespace BusinessServices.Interfaces {
    public interface IBusinessEntityService<TEntity>
        where TEntity : class, IEntity, new () {
            IEnumerable<TEntity> Find ();
            TEntity FindOne ();
        }
}