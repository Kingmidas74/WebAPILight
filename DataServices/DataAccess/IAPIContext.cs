
using System.Threading;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DataAccess {
    public interface IAPIContext 
    {
        DbSet<Parent> Parents { get; }
        DbSet<Child> Children { get; }

        int SaveChanges();
        Task<int> SaveChangesAsync();

    }
}