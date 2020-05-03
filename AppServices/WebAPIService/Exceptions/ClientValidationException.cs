using System.Collections.Generic;
using System;
using FluentValidation;
using System.Linq;

namespace WebAPIService.Exceptions
{
    public class ClientValidationException:Exception
    {
        private ValidationException validationException;
        public Dictionary<string,string> Messages {get;set;} = new Dictionary<string, string>();

        public ClientValidationException(ValidationException validationException)
        {
            this.validationException = validationException;
            Messages = validationException.Errors.ToDictionary(x=>x.PropertyName, x=>x.ErrorMessage);
        }

        
    }
}