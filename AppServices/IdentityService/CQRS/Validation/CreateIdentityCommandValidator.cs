using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Validators;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.CQRS
{
    public class CreateIdentityCommandValidator:AbstractValidator<CreateIdentityCommand>
    {
        private readonly AppDbContext IdentityDBContext;
        public CreateIdentityCommandValidator(AppDbContext identityDBContext)
        {
            IdentityDBContext = identityDBContext;

            RuleFor(x=>x.Id).NotEmpty();
            RuleFor(x=>x.Email).EmailAddress(EmailValidationMode.AspNetCoreCompatible).MustAsync(BeUniqueEmail);
            RuleFor(x=>x.Password).NotEmpty().Must(x=>Regex.IsMatch(x,@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$"));
            RuleFor(x=>x.Phone).NotEmpty().Must(x=>Regex.IsMatch(x,@"^\d*\(?\d{3}\)?-? *\d{3}-? *-?\d{4}$")).MustAsync(BeUniquePhone);            
        }

        public async Task<bool> BeUniqueEmail(string email, CancellationToken cancellationToken)
        {
            return await IdentityDBContext.Users
                .AllAsync(l => l.Email != email);
        }

        public async Task<bool> BeUniquePhone(string phone, CancellationToken cancellationToken)
        {
            return await IdentityDBContext.Users
                .AllAsync(l => l.Phone != phone);
        }
    }
}