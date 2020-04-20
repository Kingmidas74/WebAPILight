using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace IdentityService
{
    public interface IUserRepository
    {
        Task<User> FindByPhoneAndPassword(string phone, string password);
    }

    public class UserRepository : IUserRepository
    {
        private AppDbContext _dbContext;
        private ApplicationOptions _options;
        public UserRepository(AppDbContext dbContext, IOptions<ApplicationOptions> options)
        {
            _dbContext = dbContext;
            _options=options?.Value ?? default(ApplicationOptions);
        }
        public async Task<User> FindByPhoneAndPassword(string phone, string password)
        {
            var user = (await _dbContext.Users.ToListAsync())
            .Single(u=>u.Phone.Equals(phone)
                && u.Password.Equals($"{phone}{password}{u.Salt}{_options.Pepper}".Sha256()));
            return user;
        }
    }
}