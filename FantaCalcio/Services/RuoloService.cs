using System;
using FantaCalcio.Data;
using FantaCalcio.Models;
using FantaCalcio.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace FantaCalcio.Services
{
	public class RuoloService : IRuoloService 
	{
        private readonly AppDbContext _dbContext;

		public RuoloService(AppDbContext dbContext)
		{
            _dbContext = dbContext;
		}

        public async Task AddRuoloAsync(Ruolo ruolo)
        {
            _dbContext.Ruoli.Add(ruolo);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteRuoloAsync(int ID_Ruolo)
        {
            var ruolo = await _dbContext.Ruoli.FirstOrDefaultAsync(r => r.ID_Ruolo == ID_Ruolo);

            if(ruolo == null)
            {
                throw new Exception($"ID RUOLO :  {ID_Ruolo},  NON TROVATO");
            }

            _dbContext.Ruoli.Remove(ruolo);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Ruolo>> GetAllAsync() => await _dbContext.Ruoli.ToListAsync();


        public async Task<Ruolo> GetRuoloByIdAsync(int ID_Ruolo)
        {
            return await _dbContext.Ruoli.FindAsync(ID_Ruolo);
        }

        public async Task UpdateRuoloAsync(int ID_Ruolo, Ruolo ruoloAggiornato)
        {
            var ruoloEsistente = await _dbContext.Ruoli.FindAsync(ID_Ruolo);

            if(ruoloEsistente == null)
            {
                throw new KeyNotFoundException($"Ruolo con ID {ID_Ruolo} non trovato");
            }

            ruoloEsistente.NomeRuolo = ruoloAggiornato.NomeRuolo;

            await _dbContext.SaveChangesAsync();
        }
    }
}

