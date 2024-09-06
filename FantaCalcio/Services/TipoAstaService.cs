using System;
using FantaCalcio.Data;
using FantaCalcio.Models;
using FantaCalcio.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace FantaCalcio.Services
{
	public class TipoAstaService : ITipoAstaService
	{
        private readonly AppDbContext _dbContext;

		public TipoAstaService(AppDbContext appDbContext)
		{
            _dbContext = appDbContext;
		}

        public async Task AddTipoAstaAsync(TipoAsta tipoAsta)
        {
            _dbContext.TipoAsta.Add(tipoAsta);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteTipoAstaAsync(int ID_TipoAsta)
        {
            var tipoAsta = await _dbContext.TipoAsta.FirstOrDefaultAsync(t => t.ID_TipoAsta == ID_TipoAsta);
            if(tipoAsta == null)
            {
                throw new Exception($"TipoAsta con ID {ID_TipoAsta} non trovato");
            }

            _dbContext.TipoAsta.Remove(tipoAsta);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<TipoAsta>> GetAll() => await _dbContext.TipoAsta.ToListAsync();      

        public async Task<TipoAsta> GetTipoAstaById(int ID_TipoAsta)
        {
            return await _dbContext.TipoAsta.FindAsync(ID_TipoAsta);
        }

        public async Task UpdateTipoAstaAsync(int ID_TipoAsta, TipoAsta tipoAstaAggiornato)
        {
            var tipoAstaEsistente = await _dbContext.TipoAsta.FindAsync(ID_TipoAsta);

            if(tipoAstaEsistente == null)
            {
                throw new KeyNotFoundException($"TipoAsta con ID {ID_TipoAsta} non trovato");
            }

            tipoAstaEsistente.NomeTipoAsta = tipoAstaAggiornato.NomeTipoAsta;

            await _dbContext.SaveChangesAsync();
        }
    }
}

