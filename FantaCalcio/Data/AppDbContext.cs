using FantaCalcio.Models;
using Microsoft.EntityFrameworkCore;

namespace FantaCalcio.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt) { }

        // Definizione dei DbSet per ogni modello. Ogni DbSet rappresenta una tabella nel database
        public DbSet<Utente> Utenti { get; set; }
        public DbSet<Giocatore> Giocatori { get; set; }
        public DbSet<Ruolo> Ruoli { get; set; }
        public DbSet<Modalita> Modalita { get; set; }
        public DbSet<Asta> Aste { get; set; }
        public DbSet<Squadra> Squadre { get; set; }
        public DbSet<Operazione> Operazioni { get; set; }
        public DbSet<TipoAsta> TipoAsta { get; set; }  
        public DbSet<RuoloMantra> RuoloMantra { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relazione 1:N tra Utente e Asta
            modelBuilder.Entity<Asta>()
                .HasOne(a => a.Utente)
                .WithMany(u => u.Aste)
                .HasForeignKey(a => a.ID_Utente)
                .OnDelete(DeleteBehavior.NoAction);  // No cascading delete

            // Relazione 1:N tra Modalita e Asta
            modelBuilder.Entity<Asta>()
                .HasOne(a => a.Modalita)
                .WithMany(m => m.Aste)
                .HasForeignKey(a => a.ID_Modalita)
                .OnDelete(DeleteBehavior.NoAction);  // No cascading delete

            // Relazione 1:N tra TipoAsta e Asta
            modelBuilder.Entity<Asta>()
                .HasOne(a => a.TipoAsta)
                .WithMany(ta => ta.Aste)
                .HasForeignKey(a => a.ID_TipoAsta)
                .OnDelete(DeleteBehavior.NoAction);  // No cascading delete

            // Relazione 1:N tra Squadra e Asta
            modelBuilder.Entity<Squadra>()
                .HasOne(s => s.Asta)
                .WithMany(a => a.Squadre)
                .HasForeignKey(s => s.ID_Asta)
                .OnDelete(DeleteBehavior.NoAction);  // No cascading delete

            // Relazione 1:N tra Giocatore e Operazione (Cascading delete per eliminare operazioni legate al giocatore)
            modelBuilder.Entity<Operazione>()
                .HasOne(o => o.Giocatore)
                .WithMany(g => g.Operazioni)
                .HasForeignKey(o => o.ID_Giocatore)
                .OnDelete(DeleteBehavior.Cascade);  // Cascading delete abilitato

            // Relazione 1:N tra Squadra e Operazione
            modelBuilder.Entity<Operazione>()
                .HasOne(o => o.Squadra)
                .WithMany(s => s.Operazioni)
                .HasForeignKey(o => o.ID_Squadra)
                .OnDelete(DeleteBehavior.NoAction);  // No cascading delete

            // Relazione 1:N tra Modalita e Ruolo
            modelBuilder.Entity<Ruolo>()
                .HasOne(r => r.Modalita)
                .WithMany(m => m.Ruoli)
                .HasForeignKey(r => r.ID_Modalita)
                .OnDelete(DeleteBehavior.NoAction);  // No cascading delete

            // Relazione 1:N tra Giocatore e RuoloMantra (Cascading delete per eliminare i RuoliMantra legati al giocatore)
            modelBuilder.Entity<RuoloMantra>()
                .HasOne(rm => rm.Giocatore)
                .WithMany(g => g.RuoliMantra)
                .HasForeignKey(rm => rm.ID_Giocatore)
                .OnDelete(DeleteBehavior.Cascade);  // Cascading delete abilitato

            // Relazione 1:N tra Ruolo e RuoloMantra
            modelBuilder.Entity<RuoloMantra>()
                .HasOne(rm => rm.Ruolo)
                .WithMany(r => r.RuoliMantra)
                .HasForeignKey(rm => rm.ID_Ruolo)
                .OnDelete(DeleteBehavior.NoAction);  // No cascading delete
        }

    }
}
