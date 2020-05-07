using System;
using System.Linq;
using IdentityServer4.Models;

namespace IdentityService
{
    public sealed class UtilsService
    {
        private readonly AppDbContext IdentityDBContext;
        public UtilsService(AppDbContext identityDBContext)
        {
            IdentityDBContext = identityDBContext;
        }
        internal string GenerateSalt(int length) {
            var _rdm = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                                .Select(s => s[_rdm.Next(s.Length)]).ToArray());
        }
        internal string GenerateCode(int min, int max)
        {
            var rdm = new Random();
            string code = string.Empty;

            do {
                code = rdm.Next(min, max).ToString();
            }
            while(IdentityDBContext.Identities.Select(x=>x.Code).Contains(code));            
            return code;
        }

        internal string HashedPassword(string phone,string password,string salt, string pepper) => $"{phone}{password}{salt}{pepper}".Sha256();
    }
}