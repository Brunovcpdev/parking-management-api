using Estacionamento.Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace Estacionamento.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
        }

        public DbSet<Veiculo> Veiculos => null!;
        public DbSet<Ticket> Tickets => null!;
        public DbSet<Vaga> Vagas => null!;
        public DbSet<TabelaPreco> TabelaPrecos => null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TabelaPreco>(entity =>
            {
                entity.Property(t => t.ValorFracao).HasPrecision(18, 2);
                entity.Property(t => t.ValorDiaria).HasPrecision(18, 2);
            });

            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.Property(t => t.ValorCobrado).HasPrecision(18, 2);
            });
        }
    }
}
