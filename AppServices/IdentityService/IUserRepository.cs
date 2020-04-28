using System;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace IdentityService {
    public interface IUserRepository {
        Task<User> FindByPhoneAndPassword (string phone, string password);
        Guid CreateIdentity(CreateIdentityParameter createIdentityParameter);
        Guid ConfirmIdentity(ConfirmIdentityParameter confirmIdentityParameter);
    }

    public class UserRepository : IUserRepository {
        private AppDbContext _dbContext;
        private ApplicationOptions _options;
        public UserRepository (AppDbContext dbContext, IOptions<ApplicationOptions> options) {
            _dbContext = dbContext;
            _options = options?.Value ?? default (ApplicationOptions);
        }

        private string GenerateSalt(int length) {
            Random _rdm = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[_rdm.Next(s.Length)]).ToArray());
        }
        public string GenerateCode()
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();

            string code = _rdm.Next(_min, _max).ToString();
            try{
                while(_dbContext.Identities.Select(x=>x.Code).Contains(code))
                {
                    code = _rdm.Next(_min, _max).ToString();
                }
            }
            catch(Exception e)
            {
                throw e;
            }
            return code;
        }

        public static string HashedPassword(string phone,string password,string salt, string pepper) {
            var result = $"{phone}{password}{salt}{pepper}".Sha256();
            return result;
        }

        public Guid CreateIdentity(CreateIdentityParameter createIdentityParameter)
        {
            var userExist = _dbContext.Users.FirstOrDefault(u=>u.Email.Equals(createIdentityParameter.Email) || u.Phone.Equals(createIdentityParameter.Phone));
            if(userExist!=null) throw new ArgumentOutOfRangeException(nameof(createIdentityParameter));

            var identityExist = _dbContext.Identities.FirstOrDefault(u=>u.Email.Equals(createIdentityParameter.Email) || u.Phone.Equals(createIdentityParameter.Phone));
            var code = GenerateCode();
            if(identityExist!=null)
            {
                identityExist.Code=code;                
            }
            else {
                var salt = GenerateSalt(createIdentityParameter.Password.Length);
                
                var identity = new Identity {
                    Id=createIdentityParameter.Id,
                    Email = createIdentityParameter.Email,
                    Phone = createIdentityParameter.Phone,
                    Salt = salt,
                    Password = HashedPassword(createIdentityParameter.Phone,createIdentityParameter.Password,salt,_options.Pepper),
                    Code = code
                };

                _dbContext.Identities.Add(identity);
            }
            _dbContext.SaveChanges();
            return createIdentityParameter.Id;
        }

        public Guid ConfirmIdentity(ConfirmIdentityParameter confirmIdentityParameter)
        {
            var identity = _dbContext.Identities.SingleOrDefault(x=>x.Code.Equals(confirmIdentityParameter.Code) && x.Id.Equals(confirmIdentityParameter.Id));
            if(identity==null) throw new NullReferenceException(nameof(identity));
            _dbContext.Users.Add(new User {
                Id=identity.Id,
                Email=identity.Email,
                Password=identity.Password,
                Phone=identity.Phone,
                Salt=identity.Salt                        
            });
            _dbContext.Identities.Remove(identity);
            _dbContext.SaveChanges();
            return confirmIdentityParameter.Id;            
        }

        public async Task<User> FindByPhoneAndPassword (string phone, string password) {
            var user = (await _dbContext.Users.ToListAsync ())
                .Single (u => u.Phone.Equals (phone) &&
                    u.Password.Equals(
                        HashedPassword(u.Phone,password,u.Salt,_options.Pepper)
                    ));
            return user;
        }
    }
}