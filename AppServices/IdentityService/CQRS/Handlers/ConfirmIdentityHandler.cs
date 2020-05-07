using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.CQRS
{
    public class ConfirmIdentityHandler : IRequestHandler<ConfirmIdentityCommand, string>
    {
        private readonly AppDbContext IdentityDBContext;
        public ConfirmIdentityHandler(AppDbContext identityDBContext)
        {
            this.IdentityDBContext = identityDBContext;
        }

        public async Task<string> Handle(ConfirmIdentityCommand request, CancellationToken cancellationToken)
        {
            var identity = await IdentityDBContext.Identities.SingleAsync(x=>x.Code.Equals(request.Code) && x.Id.Equals(request.Id));
            
            await IdentityDBContext.Users.AddAsync(new User {
                Id=identity.Id,
                Email=identity.Email,
                Password=identity.Password,
                Phone=identity.Phone,
                Salt=identity.Salt                        
            });

            IdentityDBContext.Identities.Remove(identity);
            await IdentityDBContext.SaveChangesAsync();
            return request.Redirect;            
        }
    }
}