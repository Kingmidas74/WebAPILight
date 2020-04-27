using System;

namespace DataAccess.DataBaseEntities {
    public class Child<T> : DataBaseEntity<T> {
        public string FirstName { get; set; }
        
        public string SecondName { get; set; }

        public string LastName { get; set; }

        public DateTime BirthDay { get; set; }

        public virtual Parent<T> Parent { get; set; }
    }
}