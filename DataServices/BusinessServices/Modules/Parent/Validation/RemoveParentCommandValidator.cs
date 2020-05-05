using FluentValidation;

namespace BusinessServices.Modules.ParentModule
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