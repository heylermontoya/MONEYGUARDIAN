using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MONEY_GUARDIAN.Domain.Entities;

namespace MONEY_GUARDIAN.Infrastructure.Context
{
    public class PersistenceContext : DbContext
    {
        private readonly IConfiguration _config;

        public PersistenceContext(DbContextOptions<PersistenceContext> options, IConfiguration config) : base(options)
        {
            _config = config;
        }

        public async Task CommitAsync()
        {
            await SaveChangesAsync().ConfigureAwait(false);
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<ExpenseType> ExpenseTypes => Set<ExpenseType>();
        public DbSet<MonetaryFund> MonetaryFunds => Set<MonetaryFund>();
        public DbSet<Budget> Budgets => Set<Budget>();
        public DbSet<ExpenseHeader> ExpenseHeaders => Set<ExpenseHeader>();
        public DbSet<ExpenseDetail> ExpenseDetails => Set<ExpenseDetail>();
        public DbSet<Deposit> Deposits => Set<Deposit>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                return;
            }

            modelBuilder.HasDefaultSchema(_config.GetValue<string>("SchemaName"));

            #region Models

            modelBuilder.Entity<ExpenseType>()
            .HasIndex(x => x.Code)
            .IsUnique();

            // Relationships
            modelBuilder.Entity<ExpenseDetail>()
                .HasOne(d => d.ExpenseHeader)
                .WithMany(h => h.Details)
                .HasForeignKey(d => d.ExpenseHeaderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed default admin user
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Name = "Administrator",
                    Username = "admin",
                    Password = "admin",
                    IsUserGoogle = false
                }
            );
            #endregion

            base.OnModelCreating(modelBuilder);
        }
    }
}
