using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BusinessServices.Interfaces;
using DataAccess;
using Domain;

namespace BusinessServices.Base {
    public abstract class BaseEntityService<TEntity> : IBusinessEntityService<TEntity>
        where TEntity : IEntity, new () {

            protected IAPIContext DbContext { get; }
            protected IMapper Mapper { get; }

            public BaseEntityService (IAPIContext DbContext, IMapper Mapper) {
                this.DbContext = DbContext;
                this.Mapper = Mapper;
            }
            public abstract IEnumerable<TEntity> FindAll();
            public abstract Task<List<TEntity>> FindAllAsync();
            public abstract TEntity FindOne (Guid Id);
            public abstract Task<TEntity> FindOneAsync(Guid Id);
            public abstract void RemoveById(Guid Id);
            public abstract Task RemoveByIdAsync(Guid Id);
            public abstract TEntity Create(TEntity entity);
            public abstract Task<TEntity> CreateAsync(TEntity entity);
    }
}