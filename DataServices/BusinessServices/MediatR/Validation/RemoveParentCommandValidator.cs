using FluentValidation;

namespace BusinessServices.MediatR
{
    public class RemoveParentCommandValidator:AbstractValidator<RemoveParentCommand>
    {
        public RemoveParentCommandValidator()
        {
            RuleFor(x=>x.Id)
                .NotEmpty();
        }
    }
}