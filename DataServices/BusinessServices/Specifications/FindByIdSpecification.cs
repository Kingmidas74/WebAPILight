using System;
using System.Linq.Expressions;
using DataAccess;

namespace BusinessServices.Specifications
{
    public class FindByIdSpecification<T,L>:LinqSpecification<T>
        where T:DataBaseEntity<L>
    {
        private readonly L Id;
    
        public FindByIdSpecification(L Id)
        {
            this.Id=Id;
        }

        public override Expression<Func<T, bool>> AsExpression()
        {
            return entity => entity.Id.Equals(this.Id);
        }
    }

}