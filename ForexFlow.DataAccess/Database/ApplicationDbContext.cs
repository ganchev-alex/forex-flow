using ForexFlow.Model;
using ForexFlow.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace ForexFlow.DataAccess.Database
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<SingleCurrencyAmount> SingleCurrencyAmounts { get; set; }
        public DbSet<Invoice> Invoices { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SingleCurrencyAmount>()
                .HasOne(s => s.Currency)
                .WithMany()
                .HasForeignKey(s => s.CurrencyCode)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Invoice>()
                .Property(i => i.InvoiceItems)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, new JsonSerializerOptions()),
                    v => JsonSerializer.Deserialize<List<Item>>(v, new JsonSerializerOptions()) ?? new List<Item>()
                );
        }

    }
}

