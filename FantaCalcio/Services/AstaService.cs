using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FantaCalcio.Data;
using FantaCalcio.Models;
using FantaCalcio.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace FantaCalcio.Services
{
    public class AstaService : IAstaService
    {
        private readonly AppDbContext _dbContext;

        public AstaService(AppDbContext appDbContext)
        {
            _dbContext = appDbContext;
        }

        public async Task AddAsta(Asta asta)
        {
            // Verifica che l'ID_Modalita sia valido
            var modalita = await _dbContext.Modalita.FindAsync(asta.ID_Modalita);
            if (modalita == null)
            {
                throw new Exception($"Modalità con ID {asta.ID_Modalita} non trovata");
            }

            _dbContext.Aste.Add(asta);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsta(int ID_Asta)
        {
            var asta = await _dbContext.Aste.Include(a => a.Modalita).FirstOrDefaultAsync(a => a.ID_Asta == ID_Asta);
            if (asta == null)
            {
                throw new Exception($"Asta con ID {ID_Asta} non trovata");
            }

            _dbContext.Aste.Remove(asta);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Asta>> GetAll() => await _dbContext.Aste
            .Include(a => a.Modalita)  // Include Modalita per ottenere le informazioni complete
            .ToListAsync();

        public async Task<Asta> GetAstaById(int ID_Asta)
        {
            return await _dbContext.Aste
                .Include(a => a.Modalita)  // Include Modalita per ottenere le informazioni complete
                .FirstOrDefaultAsync(a => a.ID_Asta == ID_Asta);
        }

        public async Task UpdateAsta(int ID_Asta, Asta astaAggiornata)
        {
            var astaEsistente = await _dbContext.Aste
                .Include(a => a.Modalita)  // Include Modalita per ottenere le informazioni complete
                .FirstOrDefaultAsync(a => a.ID_Asta == ID_Asta);

            if (astaEsistente == null)
            {
                throw new KeyNotFoundException($"Asta con ID {ID_Asta} non trovata");
            }

            // Verifica che l'ID_Modalita sia valido
            var modalita = await _dbContext.Modalita.FindAsync(astaAggiornata.ID_Modalita);
            if (modalita == null)
            {
                throw new Exception($"Modalità con ID {astaAggiornata.ID_Modalita} non trovata");
            }

            astaEsistente.CreditiDisponibili = astaAggiornata.CreditiDisponibili;
            astaEsistente.NumeroSquadre = astaAggiornata.NumeroSquadre;
            astaEsistente.ID_Modalita = astaAggiornata.ID_Modalita;  // Aggiorna il riferimento alla Modalità
            astaEsistente.TipoAsta = astaAggiornata.TipoAsta;

            await _dbContext.SaveChangesAsync();
        }
    }
}
