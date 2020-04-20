namespace DataAccess.Interfaces {
    public interface IModel<TEntity> : IContext
    where TEntity : class, new () { }
}