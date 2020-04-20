using System;
using System.Data;
using Npgsql;

namespace DataAccess.Extensions {
    public class DapperDataBase : IDisposable {
        private Type _initializer;

        public DapperDataBase () { }

        public string ConnectionString { get; set; }

        public void Dispose () { }

        internal void SetInitializer<T> (object p) {
            _initializer = typeof (T);
        }

        public IDbConnection GetConnection () {
            return new NpgsqlConnection (DataAccessPolicy.ConnectionString);
        }
    }
}