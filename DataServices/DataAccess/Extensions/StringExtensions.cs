using System.Linq;

namespace DataAccess.Extensions {
    public static class StringExtensions {
        public static string TitleToUnder (this string source) {
            return string.Concat (source.Select ((x, i) => i > 0 && char.IsUpper (x) ? "_" + char.ToLower (x).ToString () : char.ToLower (x).ToString ()));
        }
    }
}