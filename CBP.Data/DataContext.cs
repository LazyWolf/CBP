using CBP.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CBP.Data
{
    public class DataContext(DbContextOptions<DataContext> options) : DbContext(options), IDataContext
    {
        public DbSet<Company> Companies { get; set; }
        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                Assembly.GetExecutingAssembly()
            );

            base.OnModelCreating(modelBuilder);
        }

        public sealed override int SaveChanges()
            => this.SaveChanges(acceptAllChangesOnSuccess: true);

        public sealed override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            PrepareAllModifiedEntitiesForSave();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public sealed override Task<int> SaveChangesAsync(CancellationToken cancellationToken) 
            => this.SaveChangesAsync(acceptAllChangesOnSuccess: true, cancellationToken);

        public sealed override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken)
        {
            PrepareAllModifiedEntitiesForSave();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess: true, cancellationToken);
        }

        /// <summary>
        /// Prepares all modified Entity object for save by populating Created/UpdatedBy and Created/UpdatedUtc columns.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns></returns>
        private void PrepareAllModifiedEntitiesForSave()
        {
            var modifiedEntries = ChangeTracker.Entries()
              .Where(x => x.State == EntityState.Added || x.State == EntityState.Modified);

            foreach (var entry in modifiedEntries)
            {
                // try and convert to an Auditable Entity
                var entity = entry.Entity as Entity;
                // call PrepareSave on the entity, telling it the state it is in
                entity?.PrepareSave(entry.State);
            }
        }
    }
}
