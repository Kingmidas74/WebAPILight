using System.Collections.Generic;
using System;

namespace Domain.Exceptions {
    public class DomainException : Exception {

        public Dictionary<string,object> Properties = new Dictionary<string, object>();        
        public DomainException (Type exceptionType) : base (nameof(Domain)) {            
            Properties.Add(nameof(exceptionType),exceptionType);
        }
    }
}