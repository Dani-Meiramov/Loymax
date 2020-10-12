using System;
using System.Threading;
using System.Threading.Tasks;
using Loymax.Domain.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Loymax.Domain.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Client> Clients { get; set; }

        public override int SaveChanges()
        {
            UpdateCreateAndModifyProperties();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken
            = new CancellationToken())
        {
            UpdateCreateAndModifyProperties();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateCreateAndModifyProperties()
        {
            ChangeTracker.DetectChanges();

            foreach (var entry in ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Detached:
                        break;
                    case EntityState.Unchanged:
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        if (entry.Entity is BaseEntity trackDeleted)
                        {
                            trackDeleted.ModifyDateTime = DateTime.UtcNow;
                            trackDeleted.IsDeleted = true;
                        }
                        break;
                    case EntityState.Modified:
                        if (entry.Entity is BaseEntity trackModified)
                        {
                            trackModified.ModifyDateTime = DateTime.UtcNow;
                        }
                        break;
                    case EntityState.Added:
                        if (entry.Entity is BaseEntity trackAdded)
                        {
                            trackAdded.CreateDateTime = DateTime.UtcNow;
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}