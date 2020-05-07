using MediatR;

namespace IdentityService.CQRS
{
    public class RemoveUserCommand:IRequest
    {
        public string Password {get;set;}
        public string Email {get;set;}
        public string Phone {get;set;}
    }
}