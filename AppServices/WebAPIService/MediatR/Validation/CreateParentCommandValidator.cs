using System;
using FluentValidation;

namespace WebAPIService.MediatR
{
    public class CreateParentCommandValidator:AbstractValidator<CreateParentCommand<Guid>>
    {
        public CreateParentCommandValidator()
        {
            RuleFor(x=>x.Id)
                .NotEmpty();
        }
    }
}