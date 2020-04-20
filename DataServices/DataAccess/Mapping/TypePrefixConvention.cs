using Dapper.FluentMap.Conventions;
using DataAccess.Extensions;

namespace DataAccess.Mapping {
    public class TypePrefixConvention : Convention {
        public TypePrefixConvention () {
            Properties ()
                .Configure (c => c.Transform (s => s.TitleToUnder ()));
        }
    }
}