using System.Collections.Concurrent;
using DataAccess.Interfaces;

namespace DataAccess.Extensions {
    public class DbContext : IContext {
        public static ConcurrentDictionary<string, object> Procedures;
        private readonly DapperDataBase _database;

        public DbContext (string configSectionName) {
            _database = new DapperDataBase ();
        }

        public DapperDataBase Database {
            get { return _database; }
        }

        public void Dispose () {
            _database.Dispose ();
        }

        public StoredProcedure<ParameterType> GetStoredProc<ParameterType> (string procedureName) {
            var proc = Procedures.ContainsKey (procedureName) ? Procedures[procedureName] : null;
            return proc as StoredProcedure<ParameterType>;
        }
    }
}