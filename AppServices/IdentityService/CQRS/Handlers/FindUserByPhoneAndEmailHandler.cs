using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.CQRS
{
    public class FindUserByPhoneAndEmailHandler : IRequestHandler<FindUserByPhoneAndPasswordQuery, User>
    {
        private readonly AppDbContext IdentityDBContext;

        public FindUserByPhoneAndEmailHandler(AppDbContext identityDBContext)
        {
            this.IdentityDBContext = identityDBContext;
        }
        public async Task<User> Handle(FindUserByPhoneAndPasswordQuery request, CancellationToken cancellationToken)
        {
            return await this.IdentityDBContext.Users.SingleAsync(x=>x.Phone.Equals(request.Phone));
        }
    }
}