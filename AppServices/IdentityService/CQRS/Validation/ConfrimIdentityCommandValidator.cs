using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Validators;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace IdentityService.CQRS
{
    public class ConfirmIdentityCommandValidator:AbstractValidator<ConfirmIdentityCommand>
    {
        private readonly AppDbContext IdentityDBContext;
        private readonly ApplicationOptions Options;
        public ConfirmIdentityCommandValidator(AppDbContext identityDBContext, IOptions<ApplicationOptions> options)
        {
            IdentityDBContext = identityDBContext;
            Options = options.Value ?? throw new NullReferenceException(nameof(ApplicationOptions));

            RuleFor(x=>x.Id).NotEmpty().MustAsync(IdentityExist);
            RuleFor(x=>x.Code).NotEmpty().Must(IsWithin);
            RuleFor(x=>x.Redirect).NotEmpty().Must(x=>Uri.TryCreate(x, UriKind.Absolute, out Uri _));            
        }

        public async Task<bool> IdentityExist(Guid Id, CancellationToken cancellationToken)
        {
            return await IdentityDBContext.Identities.AnyAsync(x=>x.Id.Equals(Id));
        }

        public bool IsWithin(string code)
        {
            return Options.LowerBoundCode<=int.Parse(code) && int.Parse(code)<=Options.UpperBoundCode;
        }
    }
}