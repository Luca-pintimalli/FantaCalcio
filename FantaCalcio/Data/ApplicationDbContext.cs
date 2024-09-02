using System;
using FantaCalcio.Models;
using Microsoft.EntityFrameworkCore;

namespace FantaCalcio.Data
{
	public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> opt) : base(opt) { }

        public DbSet<Utente> Utenti { get; set; }
        public DbSet<Giocatore> Giocatori { get; set; }
        public DbSet<Ruolo> Ruoli { get; set; }
        public DbSet<Modalita> Modalità { get; set; }
        public DbSet<Asta> Aste { get; set; }
        public DbSet<Squadra> Squadre { get; set; }
        public DbSet<Operazione> Operazioni { get; set; }
        public DbSet<Giocatore_Ruolo_Modalita> GiocatoreRuoloModalità { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Giocatore_Ruolo_Modalita>()
                .HasKey(grm => new { grm.ID_Giocatore, grm.ID_Ruolo, grm.ID_Asta });
        }
    }
}