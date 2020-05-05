using System;

namespace Domain
{
    public class Child:Entity
    {
        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public string LastName { get; set; }

        public DateTime BirthDay { get; set; }

        public virtual Parent Parent { get; set; }
    }
}