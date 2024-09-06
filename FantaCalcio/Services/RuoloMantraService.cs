using FantaCalcio.Data;
using FantaCalcio.DTOs;
using FantaCalcio.Models;
using FantaCalcio.Services.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FantaCalcio.Services
{
    public class RuoloMantraService : IRuoloMantraService
    {
        private readonly AppDbContext _dbContext;

        public RuoloMantraService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Aggiungi un ruolo Mantra a un giocatore
        public async Task AddRuoloMantra(int idGiocatore, int idRuolo)
        {
            // Controlla se il ruolo è già assegnato
            var esiste = await _dbContext.RuoloMantra
                .AnyAsync(rm => rm.ID_Giocatore == idGiocatore && rm.ID_Ruolo == idRuolo);

            if (esiste)
            {
                throw new Exception("Il giocatore ha già questo ruolo Mantra assegnato.");
            }

            // Crea e aggiungi il nuovo ruolo Mantra
            var nuovoRuoloMantra = new RuoloMantra
            {
                ID_Giocatore = idGiocatore,
                ID_Ruolo = idRuolo
            };

            _dbContext.RuoloMantra.Add(nuovoRuoloMantra);
            await _dbContext.SaveChangesAsync();
        }

        // Rimuovi un ruolo Mantra da un giocatore
        public async Task RemoveRuoloMantra(int idGiocatore, int idRuolo)
        {
            var ruoloMantra = await _dbContext.RuoloMantra
                .FirstOrDefaultAsync(rm => rm.ID_Giocatore == idGiocatore && rm.ID_Ruolo == idRuolo);

            if (ruoloMantra == null)
            {
                throw new Exception("Il ruolo Mantra specificato non esiste per questo giocatore.");
            }

            _dbContext.RuoloMantra.Remove(ruoloMantra);
            await _dbContext.SaveChangesAsync();
        }

        // Ottieni tutti i ruoli Mantra di un giocatore
        public async Task<IEnumerable<RuoloMantraDTO>> GetAllRuoliMantra()
        {
            return await _dbContext.RuoloMantra
                .Include(rm => rm.Giocatore) // Carica il giocatore associato
                .Include(rm => rm.Ruolo) // Carica il ruolo associato
                .Select(rm => new RuoloMantraDTO
                {
                    ID = rm.ID,
                    ID_Giocatore = rm.ID_Giocatore,
                    NomeGiocatore = rm.Giocatore.Nome + " " + rm.Giocatore.Cognome,
                    ID_Ruolo = rm.ID_Ruolo,
                    NomeRuolo = rm.Ruolo.NomeRuolo
                })
                .ToListAsync();
        }


        public async Task<IEnumerable<RuoloMantraDTO>> GetRuoliMantraByGiocatoreId(int idGiocatore)
        {
            return await _dbContext.RuoloMantra
                .Include(rm => rm.Giocatore)  // Carica il giocatore associato
                .Include(rm => rm.Ruolo)  // Carica il ruolo associato
                .Where(rm => rm.ID_Giocatore == idGiocatore)  // Filtra per ID del giocatore
                .Select(rm => new RuoloMantraDTO
                {
                    ID = rm.ID,
                    ID_Giocatore = rm.ID_Giocatore,
                    NomeGiocatore = rm.Giocatore.Nome + " " + rm.Giocatore.Cognome,
                    ID_Ruolo = rm.ID_Ruolo,
                    NomeRuolo = rm.Ruolo.NomeRuolo
                })
                .ToListAsync();
        }


    }
}
