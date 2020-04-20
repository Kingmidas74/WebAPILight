using System;
using System.Data;
using System.Linq;
using Dapper;
using DataAccess.Exceptions;
using DataAccess.Interfaces;

namespace DataAccess.Extensions {
    public class StoredProcedure<TParameterType> {
        private readonly IContext _dbContext;
        private readonly string _procName;
        private readonly Type _returnType;

        public StoredProcedure (Type returnType, IContext dbContext, string procName) {
            _returnType = returnType;
            _dbContext = dbContext;
            _procName = procName;
        }

        public object[] CallStoredProc (TParameterType parameter) {
            try {
                using (var db = _dbContext.Database.GetConnection ()) {
                    return db.Query (_returnType, $"SELECT * FROM {_procName}({String.Join(",",typeof(TParameterType).GetProperties().Select(x=>x.Name.TitleToUnder()+" := "+'@'+x.Name).ToList())})", parameter, commandType : CommandType.Text)?.ToArray ();
                }
            }
            catch(Exception e)
            {
                throw new ProcedureNotFoundException(e.Message);
            }
        }
    }
}