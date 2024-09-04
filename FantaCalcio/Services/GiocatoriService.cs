using System;
using FantaCalcio.Data;
using FantaCalcio.Models;
using FantaCalcio.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace FantaCalcio.Services
{
	public class GiocatoriService : IGiocatoreService
	{
        private readonly AppDbContext _dbContext;

		public GiocatoriService(AppDbContext dbContext)
		{
            _dbContext = dbContext;
        }

        public async Task AddGiocatore(Giocatore giocatore)
        {
            _dbContext.Giocatori.Add(giocatore);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteGiocatore(int ID_Giocatore)
        {
            var giocatore = await _dbContext.Giocatori.FirstOrDefaultAsync(g => g.ID_Giocatore == ID_Giocatore);
            if (giocatore == null)
            {
                throw new Exception($"Giocatore con ID {ID_Giocatore} non trovato.");
            }

            _dbContext.Giocatori.Remove(giocatore);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Giocatore>> GetAll() => await _dbContext.Giocatori.ToListAsync();

       

        public async Task<Giocatore> GetGiocatoreById(int id)
        {
            return await _dbContext.Giocatori.FindAsync(id);
        }

        public async Task UpdateGiocatore(int id, Giocatore giocatoreAggiornato)
        {
            // Trova il giocatore esistente
            var giocatoreEsistente = await _dbContext.Giocatori.FindAsync(id);

            if (giocatoreEsistente == null)
            {
                throw new Exception($"Giocatore con ID {id} non trovato.");
            }

            // Aggiorna le proprietà necessarie
            giocatoreEsistente.Nome = giocatoreAggiornato.Nome;
            giocatoreEsistente.Cognome = giocatoreAggiornato.Cognome;
            giocatoreEsistente.Foto = giocatoreAggiornato.Foto;
            giocatoreEsistente.SquadraAttuale = giocatoreAggiornato.SquadraAttuale;
            giocatoreEsistente.GoalFatti = giocatoreAggiornato.GoalFatti;
            giocatoreEsistente.GoalSubiti = giocatoreAggiornato.GoalSubiti;
            giocatoreEsistente.Assist = giocatoreAggiornato.Assist;
            giocatoreEsistente.PartiteGiocate = giocatoreAggiornato.PartiteGiocate;

            // Salva le modifiche
            await _dbContext.SaveChangesAsync();
        }
    }
}

