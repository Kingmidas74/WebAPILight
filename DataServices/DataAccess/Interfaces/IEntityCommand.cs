using System.Collections.Generic;

namespace DataAccess.Interfaces {
    public interface IEntityCommand<TEntity> where TEntity : class {
        string CommandName { get; }
        IList<TEntity> Execute ();
    }
}