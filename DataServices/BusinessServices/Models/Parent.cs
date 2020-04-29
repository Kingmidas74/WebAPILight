using System;

namespace BusinessServices.Models {
    public class Parent<T> : BusinessEntity<T> {
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDay { get; set; }

    }
}