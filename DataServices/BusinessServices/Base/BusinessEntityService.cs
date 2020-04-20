using System;
using Contracts.Shared.Interfaces;
using DataAccess.Interfaces;

namespace BusinessServices.Base {
    public class BusinessEntityService<TEntity, TKey> : BaseEntityService<TEntity>
        where TEntity : class, IBusinessEntity<TKey>, new ()
    where TKey : IComparable<TKey>, IEquatable<TKey> {
        public virtual TKey Execute (IEntityCommand<TEntity> command) {
            return (FindOne (command) ?? new TEntity ()).Id;
        }
    }
}