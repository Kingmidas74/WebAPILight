using System;
using FluentValidation;

namespace BusinessServices.MediatR
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