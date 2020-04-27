using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.DataBaseEntities {
    public class Parent<T> : DataBaseEntity<T> {
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDay { get; set; }

        public virtual IQueryable<Child<T>> Children { get; set; }

    }
}