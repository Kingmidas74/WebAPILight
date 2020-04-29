using System;

namespace BusinessServices.Exceptions {
    public class BusinessException : Exception {
        public BusinessException (string message) : base (message) {

        }
    }
}