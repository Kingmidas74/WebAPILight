using System;

namespace BusinessServices.Models
{
    public class Child<T> : BusinessEntity<T> {
        public T ParentId { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
    }
}