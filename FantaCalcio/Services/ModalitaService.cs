using System;
using FantaCalcio.Data;
using FantaCalcio.Models;
using Microsoft.EntityFrameworkCore;

namespace FantaCalcio.Services.Interface
{
	public class ModalitaService : IModalitaService
	{
        private readonly AppDbContext _dbContext;

        public ModalitaService(AppDbContext dbContext)
		{
            _dbContext = dbContext;
        }

        public async Task AddModalita(Modalita modalita)
        {
            _dbContext.Modalita.Add(modalita);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteModalita(int ID_Modalita)
        {
            var modalita = await _dbContext.Modalita.FirstOrDefaultAsync(m => m.ID_Modalita == ID_Modalita);
            if(modalita == null)
            {
                throw new Exception($"Modalita con ID {ID_Modalita} non trovata.");
            }

            _dbContext.Modalita.Remove(modalita);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Modalita>> GetAll() => await _dbContext.Modalita.ToListAsync();

        public async Task<Modalita> GetModalitaById(int id)
        {
            return await _dbContext.Modalita.FindAsync(id);
        }

        public async Task UpdateModalita(int ID_Modalita, Modalita modalitaAggiornata)
        {
            //trova la modalità esistente
            var modalitaEsistente = await _dbContext.Modalita.FindAsync(ID_Modalita);

            if(modalitaEsistente == null)
            {
                throw new KeyNotFoundException($"Modalità con ID {ID_Modalita} non trovata");

            }

            modalitaEsistente.TipoModalita = modalitaAggiornata.TipoModalita;

            await _dbContext.SaveChangesAsync();
        }
    }
}

