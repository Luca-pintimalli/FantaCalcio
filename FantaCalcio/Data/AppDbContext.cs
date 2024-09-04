using FantaCalcio.Models;
using Microsoft.EntityFrameworkCore;

namespace FantaCalcio.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt) { }

        public DbSet<Utente> Utenti { get; set; }
        public DbSet<Giocatore> Giocatori { get; set; }
        public DbSet<Ruolo> Ruoli { get; set; }
        public DbSet<Modalita> Modalita { get; set; }
        public DbSet<Asta> Aste { get; set; }
        public DbSet<Squadra> Squadre { get; set; }
        public DbSet<Operazione> Operazioni { get; set; }
        public DbSet<GiocatoreRuoloModalita> GiocatoreRuoloModalita { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurazione per Asta
            modelBuilder.Entity<Asta>()
                .HasKey(a => a.ID_Asta);

            modelBuilder.Entity<Asta>()
                .Property(a => a.ID_Utente)
                .HasColumnName("ID_Utente");

            modelBuilder.Entity<Asta>()
                .HasOne(a => a.Utente)
                .WithMany(u => u.Aste)
                .HasForeignKey(a => a.ID_Utente);

            modelBuilder.Entity<Asta>()
                .Property(a => a.ID_Modalita)
                .HasColumnName("ID_Modalita");

            modelBuilder.Entity<Asta>()
                .HasOne(a => a.Modalita)
                .WithMany(m => m.Aste)
                .HasForeignKey(a => a.ID_Modalita);

            // Configurazione per Squadra
            modelBuilder.Entity<Squadra>()
                .HasKey(s => s.ID_Squadra);

            modelBuilder.Entity<Squadra>()
                .Property(s => s.ID_Asta)
                .HasColumnName("ID_Asta");

            modelBuilder.Entity<Squadra>()
                .HasOne(s => s.Asta)
                .WithMany(a => a.Squadre)
                .HasForeignKey(s => s.ID_Asta)
                .OnDelete(DeleteBehavior.Cascade);

            // Configurazioni per GiocatoreRuoloModalita
            modelBuilder.Entity<GiocatoreRuoloModalita>()
                .HasKey(grm => new { grm.ID_Giocatore, grm.ID_Ruolo, grm.ID_Asta });

            modelBuilder.Entity<GiocatoreRuoloModalita>()
                .HasOne(grm => grm.Giocatore)
                .WithMany(g => g.GiocatoreRuoloModalitas)
                .HasForeignKey(grm => grm.ID_Giocatore);

            modelBuilder.Entity<GiocatoreRuoloModalita>()
                .HasOne(grm => grm.Ruolo)
                .WithMany(r => r.GiocatoreRuoloModalitas)
                .HasForeignKey(grm => grm.ID_Ruolo);

            modelBuilder.Entity<GiocatoreRuoloModalita>()
                .HasOne(grm => grm.Asta)
                .WithMany(a => a.GiocatoreRuoloModalitas)
                .HasForeignKey(grm => grm.ID_Asta);
        }
    }
}