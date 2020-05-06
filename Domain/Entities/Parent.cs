using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain
{
    public class Parent:Entity {
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDay { get; set; }
        public virtual ICollection<Child> Children { get; set; }
    }
}