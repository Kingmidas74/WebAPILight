using System;
using BusinessServices.Interfaces;

namespace BusinessServices.Models {
    public class BusinessEntity<T> : IBusinessEntity<T> {
        public T Id { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}