using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BusinessServices.Specifications;
using Domain;
using BusinessServices.Base;
using DataAccess;
using BusinessServices.Interfaces;

namespace BusinessServices.Services
{
    public class ParentService : BaseEntityService<Parent>, IParentService {

        public ParentService (IAPIContext DbContext, IMapper Mapper) : base (DbContext, Mapper) {

        }

        public override IEnumerable<Parent> FindAll()
        {
            return this.DbContext.Parents;
        }

        public override async Task<List<Parent>> FindAllAsync()
        {
            return await this.DbContext.Parents.ToListAsync();
        }

        public override Parent FindOne(Guid Id)
        {
            return Mapper.Map<Parent>(this.DbContext.Parents.Single(
                    new FindByIdSpecification<Parent>(Id).IsSatisfiedByExpression)
                );
        }

        public override async Task<Parent> FindOneAsync(Guid Id)
        {            
            return await this.DbContext.Parents.SingleAsync(
                    new FindByIdSpecification<Parent>(Id).IsSatisfiedByExpression);            
        }

        public override Parent Create(Parent parent)
        {
            DbContext.Parents.Add(parent);
            DbContext.SaveChanges();
            return FindOne(parent.Id);
        }

        public override async Task<Parent> CreateAsync(Parent parent)
        {
            await DbContext.Parents.AddAsync(parent);
            await DbContext.SaveChangesAsync();            
            return await FindOneAsync(parent.Id);
        }

        public override void RemoveById (Guid Id) {
            this.DbContext.Parents.Remove(
                this.DbContext.Parents.Single(
                    new FindByIdSpecification<Parent>(Id).IsSatisfiedByExpression));
            this.DbContext.SaveChanges ();
        }

        public override async Task RemoveByIdAsync (Guid Id) {            
            this.DbContext.Parents.Remove(
                await this.DbContext.Parents.SingleAsync(
                    new FindByIdSpecification<Parent>(Id).IsSatisfiedByExpression));
            await this.DbContext.SaveChangesAsync ();
        }
    }
}