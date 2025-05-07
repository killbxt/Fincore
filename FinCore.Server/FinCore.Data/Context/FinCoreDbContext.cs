using FinCore.Domain.Models.Cards;
using FinCore.Domain.Models.Clients;
using Microsoft.EntityFrameworkCore;
using Transaction = FinCore.Domain.Models.Transactions.Transaction;

namespace FinCore.Data.Context
{
    public class FinCoreDbContext : DbContext
    {
        public DbSet<Card> Cards { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public FinCoreDbContext(DbContextOptions<FinCoreDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Клиент    

            modelBuilder.Entity<Client>()
                .HasMany(c => c.Cards)
                .WithOne(c => c.Client)
                .HasForeignKey(c => c.ClientId);

            modelBuilder.Entity<Client>()
                .HasQueryFilter(c => !c.IsDeleted);

            modelBuilder.Entity<Client>()
                .HasIndex(c => c.Email)
                .IsUnique();

            //Карта

            modelBuilder.Entity<Card>()
                 .HasOne(c => c.Client)
                 .WithMany(c => c.Cards);

            modelBuilder.Entity<Card>()
                .Property(e => e.Type)
                .HasConversion<int>();

            modelBuilder.Entity<Card>()
                .Property(e => e.Status)

                .HasConversion<int>();
            modelBuilder.Entity<Card>()
                .HasQueryFilter(c => !c.IsDeleted);


            //Транзакция

            modelBuilder.Entity<Transaction>()
                .Property(e => e.Status)
                .HasConversion<int>();
            modelBuilder.Entity<Transaction>()
                .Property(e => e.Type)
                .HasConversion<int>();
            modelBuilder.Entity<Transaction>()
                .HasQueryFilter(t => !t.IsDeleted);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.CardFrom)
                .WithMany()
                .HasForeignKey(t => t.CardFromId);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.CardTo)
                .WithMany()
                .HasForeignKey(t => t.CardToId);
        }
    }
}
