using System;
using Domain;
using MediatR;

namespace BusinessServices.MediatR
{
    public class GetParentByIdQuery:IRequest<Parent>
    {        
        public Guid Id {get;}

        public GetParentByIdQuery(Guid Id)
        {
            this.Id=Id;
        }
    }
}