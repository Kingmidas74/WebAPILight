using System;
using System.Collections.Generic;
using Contracts.Shared.Interfaces;
using DataAccess.Interfaces;

namespace BusinessServices.Interfaces {
    public interface IEntityService<TEntity>
        where TEntity : class, IEntity, new () {
            IList<TEntity> Find (IEntityCommand<TEntity> command);
            TEntity FindOne (IEntityCommand<TEntity> command);
        }
}