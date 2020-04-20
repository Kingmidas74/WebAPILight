using System;
using System.Collections.Generic;
using System.Linq;
using BusinessServices.Interfaces;
using Contracts.Shared.Interfaces;
using DataAccess.Interfaces;

namespace BusinessServices.Base {
    public class BaseEntityService<TEntity> : IEntityService<TEntity>
        where TEntity : class, IEntity, new () {
            
        public virtual IList<TEntity> Find (IEntityCommand<TEntity> command) {
                return command.Execute ();
            }

            public virtual TEntity FindOne (IEntityCommand<TEntity> command) {
                return Find (command).FirstOrDefault ();
            }
        }
}