using MediatR;

namespace IdentityService.CQRS
{
    public class FindUserByPhoneAndPasswordQuery:IRequest<User>
    {
        public string Phone {get;set;}
        public string Password {get;set;}
    }
}