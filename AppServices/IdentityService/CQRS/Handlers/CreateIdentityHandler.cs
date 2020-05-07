using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace IdentityService.CQRS
{
    public class CreateIdentityHandler : IRequestHandler<CreateIdentityCommand, Guid>
    {
        private readonly AppDbContext IdentityDBContext;
        private readonly UtilsService Utils;
        private readonly ApplicationOptions Options;
        public CreateIdentityHandler(AppDbContext identityDBContext, IOptions<ApplicationOptions> options, UtilsService utils)        
        {
            this.IdentityDBContext = identityDBContext;
            this.Utils = utils;
            this.Options = options?.Value ?? throw new NullReferenceException(nameof(ApplicationOptions));
        }
        public async Task<Guid> Handle(CreateIdentityCommand request, CancellationToken cancellationToken)
        {
            Guid result;
            var code = this.Utils.GenerateCode(Options.LowerBoundCode, Options.UpperBoundCode);
            var identityExist = await IdentityDBContext.Identities.FirstOrDefaultAsync(u=>u.Email.Equals(request.Email) || u.Phone.Equals(request.Phone));            
            if(identityExist!=null)
            {                
                identityExist.Code=code;                
                result = identityExist.Id;
            }
            else {
                var salt = this.Utils.GenerateSalt(request.Password.Length);
                
                var identity = new Identity {
                    Id=request.Id,
                    Email = request.Email,
                    Phone = request.Phone,
                    Salt = salt,
                    Password = this.Utils.HashedPassword(request.Phone,request.Password,salt,Options.Pepper),
                    Code = code
                };

                await IdentityDBContext.Identities.AddAsync(identity);

                result = identity.Id;
            }
            await IdentityDBContext.SaveChangesAsync();
            return result;
        }
    }
}