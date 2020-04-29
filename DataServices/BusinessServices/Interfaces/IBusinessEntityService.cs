using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessServices.Interfaces {
    public interface IBusinessEntityService<TBusinessEntity,TKey>
        where TBusinessEntity : class, IBusinessEntity<TKey>, new () {
            IEnumerable<TBusinessEntity> FindAll();
            Task<List<TBusinessEntity>> FindAllAsync();
            TBusinessEntity FindOne (TKey Id);
            Task<TBusinessEntity> FindOneAsync (TKey Id);
        }
}