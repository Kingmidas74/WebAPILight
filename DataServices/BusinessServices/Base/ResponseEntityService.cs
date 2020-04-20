using Contracts.Shared.Interfaces;
using DataAccess.Interfaces;

namespace BusinessServices.Base {
    public class ResponseEntityService<TEntity, TKey> : BaseEntityService<TEntity>
        where TEntity : class, IResponse<TKey>, new () {
            public virtual TKey GetResponse (IEntityCommand<TEntity> command) {
                return (FindOne (command) ?? new TEntity ()).Token;
            }
        }
}