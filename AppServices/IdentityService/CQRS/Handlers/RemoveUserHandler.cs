using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.CQRS
{
    public class RemoveUserHandler : IRequestHandler<RemoveUserCommand, Unit>
    {
        private readonly AppDbContext IdentityDBContext;

        public RemoveUserHandler(AppDbContext identityDBContext)
        {
            this.IdentityDBContext = identityDBContext;
        }
        public async Task<Unit> Handle(RemoveUserCommand request, CancellationToken cancellationToken)
        {
            var user = await this.IdentityDBContext.Users.SingleAsync(x=>x.Phone.Equals(request.Phone));
            this.IdentityDBContext.Remove(user);
            await this.IdentityDBContext.SaveChangesAsync();
            return new Unit();
        }
    }
}