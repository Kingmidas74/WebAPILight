using Domain.Interfaces;

namespace DataAccess.Interfaces {
    public interface IParameteredCommand<TEntity, TParameter> : IEntityCommand<TEntity>
        where TEntity : class, new ()
    where TParameter : ICommandParameter {
        TParameter Parameter { get; set; }
    }
}