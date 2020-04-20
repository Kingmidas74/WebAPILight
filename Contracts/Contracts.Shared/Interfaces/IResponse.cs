namespace Contracts.Shared.Interfaces {
    public interface IResponse<TKey> : IEntity {
        TKey Token { get; set; }
    }
}