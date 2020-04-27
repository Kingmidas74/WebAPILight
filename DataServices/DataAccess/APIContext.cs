using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using DataAccess.DataBaseEntities;
using DataAccess.Enums;

namespace DataAccess
{
    public class APIContext<T> : DbContext
    {
        private string SchemaName {get;set;} = "public";

        public DbSet<Parent<T>> Parents { get; set;}
        public DbSet<Child<T>> Children { get; set;}
        public APIContext (DbContextOptions<APIContext<T>> options) : base(options){ }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {           
            modelBuilder.HasDefaultSchema(schema: SchemaName);
            modelBuilder
                .Entity<DataBaseEntity<T>>()
                .Property(e => e.StatusId)
                .HasConversion<int>();
                
            modelBuilder.Entity<Parent<T>>();
                    
            modelBuilder.Entity<Child<T>>()
                    .Property<int>($"{nameof(Parent<T>)}{nameof(Parent<T>.Id)}");
            modelBuilder.Entity<Child<T>>()
                    .HasOne(c=>c.Parent)
                    .WithMany(c=>c.Children)
                    .HasForeignKey($"{nameof(Parent<T>)}{nameof(Parent<T>.Id)}");
            
            modelBuilder
                .Entity<EntityStatus>().HasData(
                    Enum.GetValues(typeof(EntityStatusId))
                        .Cast<EntityStatusId>()
                        .Select(e => new EntityStatus()
                        {
                            EntityStatusId = e,
                            Value = e.ToString()
                        })
                );
            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            AddAuitInfo();
            return base.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            AddAuitInfo();
            return await base.SaveChangesAsync();
        }

        private void AddAuitInfo()
        {
            var entries = ChangeTracker.Entries().Where(x => x.Entity is DataBaseEntity<T> && (x.State == EntityState.Added || x.State == EntityState.Modified));
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    ((DataBaseEntity<T>)entry.Entity).CreatedDate = DateTime.UtcNow;
                }
            ((DataBaseEntity<T>)entry.Entity).ModifiedDate = DateTime.UtcNow;
            }
        }
    }
}