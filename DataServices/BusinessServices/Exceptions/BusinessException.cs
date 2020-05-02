using System.Collections.Generic;
using System;

namespace BusinessServices.Exceptions {
    public class BusinessException : Exception {

        public Dictionary<string,object> Properties = new Dictionary<string, object>();        
        public BusinessException (Type exceptionType) : base (nameof(BusinessServices)) {            
            Properties.Add(nameof(exceptionType),exceptionType);
        }
    }
}