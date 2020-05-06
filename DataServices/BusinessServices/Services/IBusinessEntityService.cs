using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain;

namespace BusinessServices.Services {
    public interface IBusinessEntityService<TBusinessEntity>
        where TBusinessEntity : IEntity, new () {
            IEnumerable<TBusinessEntity> FindAll();
            Task<List<TBusinessEntity>> FindAllAsync(CancellationToken cancellationToken = default);
            TBusinessEntity FindOne (Guid Id);
            Task<TBusinessEntity> FindOneAsync (Guid Id, CancellationToken cancellationToken = default);
            void RemoveById (Guid Id);
            Task RemoveByIdAsync (Guid Id, CancellationToken cancellationToken = default);
            TBusinessEntity Create(TBusinessEntity entity);
            Task<TBusinessEntity> CreateAsync(TBusinessEntity entity, CancellationToken cancellationToken = default);
        }
}