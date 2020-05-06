using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BusinessServices.Specifications;
using Domain;
using DataAccess;
using System.Threading;

namespace BusinessServices.Services
{
    public class ParentService : BaseEntityService<Parent> {

        public ParentService (IAPIContext DbContext) : base (DbContext) {

        }

        public override IEnumerable<Parent> FindAll()
        {
            return this.DbContext.Parents;
        }

        public override async Task<List<Parent>> FindAllAsync(CancellationToken cancellationToken = default)
        {
            return await this.DbContext.Parents.Include(x=>x.Children).ToListAsync();
        }

        public override Parent FindOne(Guid Id)
        {
            return this.DbContext.Parents.Single(
                    new FindByIdSpecification<Parent>(Id).IsSatisfiedByExpression);
        }

        public override async Task<Parent> FindOneAsync(Guid Id, CancellationToken cancellationToken = default)
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

        public override async Task<Parent> CreateAsync(Parent parent, CancellationToken cancellationToken = default)
        {
            await DbContext.Parents.AddAsync(parent);
            await DbContext.SaveChangesAsync();            
            return await FindOneAsync(parent.Id);
        }

        public override void RemoveById (Guid Id) {
            this.DbContext.Parents.Remove(FindOne(Id));
            this.DbContext.SaveChanges ();
        }

        public override async Task RemoveByIdAsync (Guid Id, CancellationToken cancellationToken = default) {            
            this.DbContext.Parents.Remove(await FindOneAsync(Id));
            await this.DbContext.SaveChangesAsync ();
        }
    }
}