using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;

namespace BusinessServices.Services {
    public interface IBusinessEntityService<TBusinessEntity>
        where TBusinessEntity : IEntity, new () {
            IEnumerable<TBusinessEntity> FindAll();
            Task<List<TBusinessEntity>> FindAllAsync();
            TBusinessEntity FindOne (Guid Id);
            Task<TBusinessEntity> FindOneAsync (Guid Id);
            void RemoveById (Guid Id);
            Task RemoveByIdAsync (Guid Id);
            TBusinessEntity Create(TBusinessEntity entity);
            Task<TBusinessEntity> CreateAsync(TBusinessEntity entity);
        }
}