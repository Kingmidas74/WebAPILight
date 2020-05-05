using System;
using System.Collections.Generic;
using System.Linq;
using BusinessServices.Base;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DataAccess;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Domain;

namespace BusinessServices.Services {
    public class ChildService : BaseEntityService<Child> {

        public ChildService (IAPIContext DbContext, IMapper Mapper) : base (DbContext, Mapper) {

        }
        public override IEnumerable<Child> FindAll()
        {
            return this.DbContext.Children.ProjectTo<Child> (Mapper.ConfigurationProvider);
        }

        public override async Task<List<Child>> FindAllAsync()
        {
            return await this.DbContext.Children.ProjectTo<Child> (Mapper.ConfigurationProvider).ToListAsync();
        }

        public override Child FindOne(Guid Id)
        {
            return Mapper.Map<Child>(this.DbContext.Children.SingleOrDefault(x=>x.Id.Equals(Id)));
        }

        public override async Task<Child> FindOneAsync(Guid Id)
        {
            return Mapper.Map<Child>(await this.DbContext.Children.SingleOrDefaultAsync(x=>x.Id.Equals(Id)));
        }

        public Child Create(Child child)
        {
            var result = Mapper.Map<Child>(DbContext.Children.Add(Mapper.Map<Child>(child)));
            DbContext.SaveChanges();
            return result;
        }

        public async Task<Child> CreateAsync(Child child)
        {
            var result =  Mapper.Map<Child>(await DbContext.Children.AddAsync(Mapper.Map<Child>(child)));
            await DbContext.SaveChangesAsync();
            return result;
        }

        public void RemoveById (Guid Id) {
            this.DbContext.Children.Remove (this.DbContext.Children.SingleOrDefault (x => x.Id.Equals (Id)));
            this.DbContext.SaveChanges ();
        }
    }
}