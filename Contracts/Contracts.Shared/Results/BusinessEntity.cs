using System;
using Contracts.Shared.Enums;
using Contracts.Shared.Interfaces;

namespace Contracts.Shared.Results {
    public class BusinessEntity : IBusinessEntity<Guid> {
        public Guid Id { get; set; }
        public EntityState State { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}