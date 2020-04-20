using Contracts.Shared.Enums;

namespace Contracts.Shared.Interfaces {
    public interface IBusinessEntity<TKey> : IEntity {
        TKey Id { get; set; }

        EntityState State { get; set; }
    }
}