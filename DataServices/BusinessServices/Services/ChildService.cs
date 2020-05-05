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
using BusinessServices.Specifications;

namespace BusinessServices.Services {
    public class ChildService : BaseEntityService<Child> {

        public ChildService (IAPIContext DbContext, IMapper Mapper) : base (DbContext, Mapper) {

        }
        public override IEnumerable<Child> FindAll()
        {
            return this.DbContext.Children;
        }

        public override async Task<List<Child>> FindAllAsync()
        {
            return await this.DbContext.Children.ToListAsync();
        }

        public override Child FindOne(Guid Id)
        {
            return this.DbContext.Children.Single(new FindByIdSpecification<Child>(Id).IsSatisfiedByExpression);
        }

        public override async Task<Child> FindOneAsync(Guid Id)
        {
            return await this.DbContext.Children.SingleAsync(new FindByIdSpecification<Child>(Id).IsSatisfiedByExpression);
        }

        public override Child Create(Child child)
        {
            DbContext.Children.Add(child);
            DbContext.SaveChanges();
            return FindOne(child.Id);
        }

        public override async Task<Child> CreateAsync(Child child)
        {
            await DbContext.Children.AddAsync(child);
            await DbContext.SaveChangesAsync();
            return await FindOneAsync(child.Id);
        }

        public override void RemoveById (Guid Id) {
            this.DbContext.Children.Remove (this.DbContext.Children.Single (new FindByIdSpecification<Child>(Id).IsSatisfiedByExpression));
            this.DbContext.SaveChanges ();
        }

        public override async Task RemoveByIdAsync (Guid Id) {
            this.DbContext.Children.Remove (await this.DbContext.Children.SingleAsync(new FindByIdSpecification<Child>(Id).IsSatisfiedByExpression));
            this.DbContext.SaveChanges ();
        }
    }
}