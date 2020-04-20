using System;
using DataAccess.Extensions;

namespace DataAccess.Interfaces {
    public interface IContext : IDisposable {
        DapperDataBase Database { get; }
        StoredProcedure<TParameterType> GetStoredProc<TParameterType> (string procedureName);
    }
}