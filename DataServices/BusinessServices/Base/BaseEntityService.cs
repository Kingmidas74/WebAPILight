using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BusinessServices.Interfaces;
using DataAccess;

namespace BusinessServices.Base {
    public abstract class BaseEntityService<TEntity,TKey> : IBusinessEntityService<TEntity,TKey>
        where TEntity : class, IBusinessEntity<TKey>, new () {

            protected APIContext<TKey> DbContext { get; }
            protected IMapper Mapper { get; }

            public BaseEntityService (APIContext<TKey> DbContext, IMapper Mapper) {
                this.DbContext = DbContext;
                this.Mapper = Mapper;
            }
            public abstract IEnumerable<TEntity> FindAll();
            public abstract Task<List<TEntity>> FindAllAsync();
            public abstract TEntity FindOne (TKey Id);
            public abstract Task<TEntity> FindOneAsync(TKey Id);
    }
}