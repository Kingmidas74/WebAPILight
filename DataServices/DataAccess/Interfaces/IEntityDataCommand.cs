using Domain.Interfaces;

namespace DataAccess.Interfaces {
    public interface IEntityDataCommand<TEntity, TParameter> : IParameteredCommand<TEntity, TParameter>
        where TEntity : class, new ()
    where TParameter : ICommandParameter { }
}