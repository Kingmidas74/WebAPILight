using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BusinessServices.Base;
using DTO = BusinessServices.Models;
using DataAccess;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DataAccess.DataBaseEntities;

namespace BusinessServices.Services
{
    public class ParentService<T> : BaseEntityService<DTO.Parent<T>,T> {

        public ParentService (APIContext<T> DbContext, IMapper Mapper) : base (DbContext, Mapper) {

        }

        public override IEnumerable<DTO.Parent<T>> FindAll()
        {
            return this.DbContext.Parents.ProjectTo<DTO.Parent<T>> (Mapper.ConfigurationProvider);
        }

        public override async Task<List<DTO.Parent<T>>> FindAllAsync()
        {
            return await this.DbContext.Parents.ProjectTo<DTO.Parent<T>> (Mapper.ConfigurationProvider).ToListAsync();
        }

        public override DTO.Parent<T> FindOne(T Id)
        {
            return Mapper.Map<DTO.Parent<T>>(this.DbContext.Parents.SingleOrDefault(x=>x.Id.Equals(Id)));
        }

        public override async Task<DTO.Parent<T>> FindOneAsync(T Id)
        {
            return Mapper.Map<DTO.Parent<T>>(await this.DbContext.Parents.SingleOrDefaultAsync(x=>x.Id.Equals(Id)));
        }

        public DTO.Parent<T> Create(DTO.Parent<T> parent)
        {
            DbContext.Parents.Add(Mapper.Map<Parent<T>>(parent));
            DbContext.SaveChanges();
            return FindOne(parent.Id);
        }

        public async Task<DTO.Parent<T>> CreateAsync(DTO.Parent<T> parent)
        {
            await DbContext.Parents.AddAsync(Mapper.Map<Parent<T>>(parent));
            await DbContext.SaveChangesAsync();            
            return await FindOneAsync(parent.Id);
        }

        public void RemoveById (Guid parentId) {
            this.DbContext.Parents.Remove (this.DbContext.Parents.SingleOrDefault (x => x.Id.Equals (parentId)));
            this.DbContext.SaveChanges ();
        }
    }
}