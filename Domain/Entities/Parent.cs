using System;
using System.Collections.Generic;

namespace Domain
{
    public class Parent:Entity {
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDay { get; set; }
        public virtual IEnumerable<Child> Children { get; set; }
    }
}