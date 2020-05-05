using System;
using MediatR;

namespace BusinessServices.MediatR
{
    public class RemoveParentCommand:IRequest
    {
        public Guid Id {get;set;}
    }
}