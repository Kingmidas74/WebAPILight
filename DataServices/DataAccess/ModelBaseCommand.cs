using System.Collections.Generic;
using System.Linq;
using DataAccess.Interfaces;
using Domain.Exceptions;
using Domain.Interfaces;

namespace DataAccess {
    public class ModelBaseCommand<TEntity, TParameter> : IEntityDataCommand<TEntity, TParameter>
        where TEntity : class, new ()
    where TParameter : class, ICommandParameter {
        public ModelBaseCommand (IModel<TEntity> dbContext) {
            DBContext = dbContext;
        }
        public TParameter Parameter { get; set; }

        public string CommandName => typeof (TParameter).Name.Replace (nameof (Parameter), "");

        public IContext DBContext { get; set; }

        protected virtual string ProcedureName {
            get { return string.Format ("{0}{1}", typeof (TEntity).Name, CommandName); }
        }
        public IList<TEntity> Execute () {
            using (var dbContext = DBContext) {
                var storedProcedure = dbContext.GetStoredProc<TParameter> (ProcedureName);
                if (storedProcedure != null) {
                    var result = storedProcedure.CallStoredProc (Parameter);
                    return result?.Length > 0 ? result.Select (r => r as TEntity).ToList () : new List<TEntity> ();
                }
                throw new WebApiDomainExceptionBase (
                    string.Format (
                        "Method \"{0}\" for entity \"{1}\" with parameter \"{2}\" is not registered in \"{3}\"",
                        CommandName,
                        typeof (TEntity).Name,
                        typeof (TParameter),
                        typeof (IContext).Name));
            }
        }
    }
}