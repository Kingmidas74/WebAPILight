using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DataAccess;
using Domain;

namespace BusinessServices.Services {
    public abstract class BaseEntityService<TEntity> : IBusinessEntityService<TEntity>
        where TEntity : IEntity, new () {

            protected IAPIContext DbContext { get; }

            public BaseEntityService (IAPIContext DbContext) {
                this.DbContext = DbContext;
            }
            public abstract IEnumerable<TEntity> FindAll();
            public abstract Task<List<TEntity>> FindAllAsync(CancellationToken cancellationToken = default);
            public abstract TEntity FindOne (Guid Id);
            public abstract Task<TEntity> FindOneAsync(Guid Id, CancellationToken cancellationToken = default);
            public abstract void RemoveById(Guid Id);
            public abstract Task RemoveByIdAsync(Guid Id, CancellationToken cancellationToken = default);
            public abstract TEntity Create(TEntity entity);
            public abstract Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default);
    }
}