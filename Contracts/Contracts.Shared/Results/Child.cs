using System;
using Contracts.Shared.Enums;
using Contracts.Shared.Interfaces;

namespace Contracts.Shared.Results {
    public class Child : BusinessEntity {
        public Guid ParentId { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
    }
}