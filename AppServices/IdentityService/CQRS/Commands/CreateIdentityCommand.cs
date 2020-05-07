using System;
using MediatR;

namespace IdentityService.CQRS
{
    public class CreateIdentityCommand:IRequest<Guid>
    {
        public Guid Id {get;set;}
        public string Password {get;set;}
        public string Email {get;set;}
        public string Phone {get;set;}
    }
}