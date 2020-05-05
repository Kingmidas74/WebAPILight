using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessServices.Interfaces {
    public interface IBusinessEntityService<TBusinessEntity>
        where TBusinessEntity : class, new () {
            IEnumerable<TBusinessEntity> FindAll();
            Task<List<TBusinessEntity>> FindAllAsync();
            TBusinessEntity FindOne (Guid Id);
            Task<TBusinessEntity> FindOneAsync (Guid Id);
        }
}