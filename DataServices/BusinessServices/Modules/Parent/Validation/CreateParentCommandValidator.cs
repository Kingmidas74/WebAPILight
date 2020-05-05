using System;
using FluentValidation;

namespace BusinessServices.Modules.ParentModule
{
    public class CreateParentCommandValidator:AbstractValidator<CreateParentCommand>
    {
        public CreateParentCommandValidator()
        {
            RuleFor(x=>x.Id)
                .NotEmpty();
        }
    }
}