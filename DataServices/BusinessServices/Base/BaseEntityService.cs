using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BusinessServices.Interfaces;
using Contracts.Shared.Interfaces;
using DataAccess;

namespace BusinessServices.Base {
    public abstract class BaseEntityService<TEntity> : IBusinessEntityService<TEntity>
        where TEntity : class, IEntity, new () {
            
        protected APIContext<Guid> DbContext {get;}
        protected IMapper Mapper { get; }

        public BaseEntityService(APIContext<Guid> DbContext, IMapper Mapper) 
        {
            this.DbContext = DbContext;
            this.Mapper = Mapper;
        }
        public abstract IEnumerable<TEntity> Find ();

        public abstract TEntity FindOne();
    }
}