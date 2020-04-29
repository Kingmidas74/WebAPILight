using System;
using System.Collections.Generic;
using System.Linq;
using BusinessServices.Base;
using DTO = BusinessServices.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DataAccess;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DataAccess.DataBaseEntities;

namespace BusinessServices.Services {
    public class ChildService<T> : BaseEntityService<DTO.Child<T>,T> {

        public ChildService (APIContext<T> DbContext, IMapper Mapper) : base (DbContext, Mapper) {

        }
        public override IEnumerable<DTO.Child<T>> FindAll()
        {
            return this.DbContext.Children.ProjectTo<DTO.Child<T>> (Mapper.ConfigurationProvider);
        }

        public override async Task<List<DTO.Child<T>>> FindAllAsync()
        {
            return await this.DbContext.Children.ProjectTo<DTO.Child<T>> (Mapper.ConfigurationProvider).ToListAsync();
        }

        public override DTO.Child<T> FindOne(T Id)
        {
            return Mapper.Map<DTO.Child<T>>(this.DbContext.Children.SingleOrDefault(x=>x.Id.Equals(Id)));
        }

        public override async Task<DTO.Child<T>> FindOneAsync(T Id)
        {
            return Mapper.Map<DTO.Child<T>>(await this.DbContext.Children.SingleOrDefaultAsync(x=>x.Id.Equals(Id)));
        }

        public DTO.Child<T> Create(DTO.Child<T> child)
        {
            var result = Mapper.Map<DTO.Child<T>>(DbContext.Children.Add(Mapper.Map<Child<T>>(child)));
            DbContext.SaveChanges();
            return result;
        }

        public async Task<DTO.Child<T>> CreateAsync(DTO.Child<T> child)
        {
            var result =  Mapper.Map<DTO.Child<T>>(await DbContext.Children.AddAsync(Mapper.Map<Child<T>>(child)));
            await DbContext.SaveChangesAsync();
            return result;
        }

        public void RemoveById (Guid Id) {
            this.DbContext.Children.Remove (this.DbContext.Children.SingleOrDefault (x => x.Id.Equals (Id)));
            this.DbContext.SaveChanges ();
        }
    }
}