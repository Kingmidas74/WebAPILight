using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using WebAPIService.Exceptions;

namespace WebAPIService.MediatR
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            this.validators = validators;
        }

        //TODO: TResult<TResponse,Error>
        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var context = new ValidationContext(request);
            var failures = validators
                .Select(v=>v.Validate(context))
                .SelectMany(x=>x.Errors)
                .Where(x=>x!=null)
                .ToList();

            if(failures.Any()) 
                throw new ClientValidationException(new ValidationException(failures));

            return next();
        }
    }
}