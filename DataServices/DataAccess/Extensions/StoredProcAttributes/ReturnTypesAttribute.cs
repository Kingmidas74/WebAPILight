using System;

namespace DataAccess.Extensions.StoredProcAttributes {
    public class ReturnTypesAttribute : Attribute {
        public ReturnTypesAttribute (Type type) {
            ReturnType = type;
        }

        public Type ReturnType { get; set; }
    }
}