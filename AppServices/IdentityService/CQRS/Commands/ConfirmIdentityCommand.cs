using System;
using MediatR;

namespace IdentityService.CQRS
{
    public class ConfirmIdentityCommand:IRequest<string>
    {
        public Guid Id {get;set;}
        public string Code {get;set;}
        public string Redirect {get;set;}
    }
}