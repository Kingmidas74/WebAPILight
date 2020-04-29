using System;
using BusinessServices.Models;
using MediatR;

namespace WebAPIService.MediatR
{
    public class GetParentByIdQuery<T>:IRequest<Parent<T>>
    {        
        public T Id {get;}

        public GetParentByIdQuery(T Id)
        {
            this.Id=Id;
        }
    }
}