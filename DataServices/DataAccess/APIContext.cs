using System;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DataAccess {
    public class APIContext : DbContext,IAPIContext {
        private string SchemaName { get; set; } = "public";

        public DbSet<Parent> Parents { get; set; }
        public DbSet<Child> Children { get; set; }
        public APIContext (DbContextOptions<APIContext> options) : base (options) { }

        protected override void OnModelCreating (ModelBuilder modelBuilder) {
            modelBuilder.HasDefaultSchema (schema: SchemaName);

            modelBuilder.Entity<EntityStatus> (entity => {
                entity.HasData (
                    Enum.GetValues (typeof (EntityStatusId))
                    .Cast<EntityStatusId> ()
                    .Select (e => new EntityStatus () {
                        EntityStatusId = e,
                            Value = e.ToString ()
                    })
                );
            });
            
            modelBuilder.Entity<Parent> (entity => {
                entity.HasKey (x => x.Id);
                entity.Property (e => e.Status)
                    .HasConversion<int> ();
            });

            modelBuilder.Entity<Child> (entity => {
                entity.Property ($"{nameof(Parent)}{nameof(Parent.Id)}");
                entity.Property (e => e.Status)
                    .HasConversion<int> ();
                entity.HasOne (c => c.Parent)
                    .WithMany (c => c.Children)
                    .HasForeignKey ($"{nameof(Parent)}{nameof(Parent.Id)}");
            });

            base.OnModelCreating (modelBuilder);
        }

        public override int SaveChanges () {
            AddAuitInfo ();
            return base.SaveChanges ();
        }

        public async Task<int> SaveChangesAsync () {
            AddAuitInfo ();
            return await base.SaveChangesAsync ();
        }

        private void AddAuitInfo () {
            var entries = ChangeTracker.Entries ().Where (x => x.Entity is IEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));
            foreach (var entry in entries) {
                if (entry.State == EntityState.Added) {
                    ((IEntity) entry.Entity).CreatedDate = DateTime.UtcNow;
                    ((IEntity) entry.Entity).Status = EntityStatusId.Active;
                }
                ((IEntity) entry.Entity).ModifiedDate = DateTime.UtcNow;
            }
        }
    }
}