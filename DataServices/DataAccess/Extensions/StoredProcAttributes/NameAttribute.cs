using System;

namespace DataAccess.Extensions.StoredProcAttributes {
    public class NameAttribute : Attribute {
        public NameAttribute (string name) {
            Name = name;
        }

        public string Name { get; private set; }
    }
}