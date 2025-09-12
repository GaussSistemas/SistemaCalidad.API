using Microsoft.EntityFrameworkCore;
using SistemaDeCalidad.API.Persistence.Entities;

namespace SistemaDeCalidad.API.Persistence.Context
{
    public class SistemaDeCalidadContext : DbContext
    {
        public SistemaDeCalidadContext(DbContextOptions<SistemaDeCalidadContext> options) : base(options)
        { }

        public DbSet<Message> Messages { get; set; }
        public DbSet<MessageComment> MessagesComments { get; set; }
        public DbSet<MessageLog> MessagesLogs { get; set; }
        public DbSet<MessageType> MessagesTypes { get; set; }
        public DbSet<MessageUser> MessagesUsers { get; set; }
        public DbSet<MessageView> MessagesViews { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleStep> RolesSteps { get; set; }
        public DbSet<Step> Steps { get; set; }
        public DbSet<User> Users { get; set; }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            ApplyAuditInformation();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }
        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            ApplyAuditInformation();
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken).ConfigureAwait(false);
        }
        private void ApplyAuditInformation()
        {
            var utcNow = DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries<Base>().Where(e => e.State == EntityState.Added ||e.State == EntityState.Modified))
            {
                if (entry.State == EntityState.Added)
                    entry.Entity.Created = utcNow;

                entry.Entity.Modified = utcNow;
            }
        }
    }
}
