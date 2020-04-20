using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace DataAccess {
    public class DataBase : DbContext {
        public DataBase (DbContextOptions<DataBase> options) : base (options) { }

    }
}