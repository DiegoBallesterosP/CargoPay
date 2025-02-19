using CargoPay.Entities;
using Microsoft.EntityFrameworkCore;

namespace CargoPay.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Card> Cards { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<PaymentFee> PaymentFees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Card>()
                .HasIndex(c => c.CardNumber)
                .IsUnique();
        }
    }
}
