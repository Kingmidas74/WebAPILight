using System;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.DataBaseEntities;
using DataAccess.Enums;
using Microsoft.EntityFrameworkCore;

namespace DataAccess {
    public class APIContext<T> : DbContext {
        private string SchemaName { get; set; } = "public";

        public DbSet<Parent<T>> Parents { get; set; }
        public DbSet<Child<T>> Children { get; set; }
        public APIContext (DbContextOptions<APIContext<T>> options) : base (options) { }

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
            
            modelBuilder.Entity<Parent<T>> (entity => {
                entity.HasKey (x => x.Id);
                entity.Property (e => e.EntityStatusId)
                    .HasConversion<int> ();
            });

            modelBuilder.Entity<Child<T>> (entity => {
                entity.Property<T> ($"{nameof(Parent<T>)}{nameof(Parent<T>.Id)}");
                entity.Property (e => e.EntityStatusId)
                    .HasConversion<int> ();
                entity.HasOne (c => c.Parent)
                    .WithMany (c => c.Children)
                    .HasForeignKey ($"{nameof(Parent<T>)}{nameof(Parent<T>.Id)}");
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
            var entries = ChangeTracker.Entries ().Where (x => x.Entity is DataBaseEntity<T> && (x.State == EntityState.Added || x.State == EntityState.Modified));
            foreach (var entry in entries) {
                if (entry.State == EntityState.Added) {
                    ((DataBaseEntity<T>) entry.Entity).CreatedDate = DateTime.UtcNow;
                    ((DataBaseEntity<T>) entry.Entity).EntityStatusId = EntityStatusId.Active;
                }
                ((DataBaseEntity<T>) entry.Entity).ModifiedDate = DateTime.UtcNow;
            }
        }
    }
}