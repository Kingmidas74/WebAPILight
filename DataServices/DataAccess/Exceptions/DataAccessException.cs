using System;
using System.Collections.Generic;

namespace DataAccess
{
    public abstract class DataAccessException:Exception
    {
        public Dictionary<string,object> Properties = new Dictionary<string, object>();        
        public DataAccessException():base(nameof(DataAccess))
        {

        }
    }
}