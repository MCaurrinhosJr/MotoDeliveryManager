using MotoDeliveryManager.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace MotoDeliveryManager.Infra.Context
{
    public class MDMDbContext : DbContext
    {
        public MDMDbContext(DbContextOptions<MDMDbContext> options)
            : base(options)
        {
        }

        public DbSet<Moto> Motos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Entregador> Entregadores { get; set; }
        public DbSet<Locacao> Locacoes { get; set; }
        public DbSet<Notificacao> Notificacoes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Moto>().ToTable("Moto").HasKey(m => m.Id);
            modelBuilder.Entity<Pedido>().ToTable("Pedido").HasKey(p => p.Id);
            modelBuilder.Entity<Entregador>().ToTable("Entregador").HasKey(e => e.Id);
            modelBuilder.Entity<Locacao>().ToTable("Locacao").HasKey(l => l.Id);
            modelBuilder.Entity<Notificacao>().ToTable("Notificacao").HasKey(n => n.Id);

            modelBuilder.Entity<Pedido>()
                .HasOne(p => p.Entregador)
                .WithMany(e => e.Pedidos)
                .HasForeignKey(p => p.EntregadorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Moto>()
                .HasIndex(m => m.Placa)
                .IsUnique();

            modelBuilder.Entity<Entregador>()
                .HasIndex(e => e.CNPJ)
                .IsUnique();

            modelBuilder.Entity<Entregador>()
                .HasIndex(e => e.NumeroCNH)
                .IsUnique();

            modelBuilder.Entity<CNHImage>().HasNoKey();
            modelBuilder.Entity<Entregador>().Ignore(e => e.CNHImage);
        }
    }
}