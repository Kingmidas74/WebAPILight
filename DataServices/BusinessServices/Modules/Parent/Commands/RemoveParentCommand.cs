using System;
using MediatR;

namespace BusinessServices.Modules.ParentModule
{
    public class RemoveParentCommand:IRequest
    {
        public Guid Id {get;set;}
    }
}