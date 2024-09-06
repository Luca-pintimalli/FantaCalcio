using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FantaCalcio.Data;
using FantaCalcio.DTOs;
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

        // Aggiungi una nuova asta
        public async Task AddAsta(int userId, AstaCreateUpdateDto astaDto)
        {
            var asta = new Asta
            {
                ID_Utente = userId,  // Inserisci l'ID utente recuperato dal JWT
                ID_TipoAsta = astaDto.ID_TipoAsta,
                NumeroSquadre = astaDto.NumeroSquadre,
                CreditiDisponibili = astaDto.CreditiDisponibili,
                ID_Modalita = astaDto.ID_Modalita
            };

            _dbContext.Aste.Add(asta);
            await _dbContext.SaveChangesAsync();
        }


        // Aggiorna un'asta esistente
        public async Task UpdateAsta(int ID_Asta, int userId, AstaCreateUpdateDto astaDto)
        {
            var astaEsistente = await _dbContext.Aste.FirstOrDefaultAsync(a => a.ID_Asta == ID_Asta);

            if (astaEsistente == null)
            {
                throw new KeyNotFoundException($"Asta con ID {ID_Asta} non trovata.");
            }

            // Aggiorna i campi con i dati della DTO
            astaEsistente.ID_TipoAsta = astaDto.ID_TipoAsta;
            astaEsistente.NumeroSquadre = astaDto.NumeroSquadre;
            astaEsistente.CreditiDisponibili = astaDto.CreditiDisponibili;
            astaEsistente.ID_Modalita = astaDto.ID_Modalita;
            astaEsistente.ID_Utente = userId;  // Assegna l'ID utente dal token JWT

            await _dbContext.SaveChangesAsync();
        }

        // Elimina un'asta
        public async Task DeleteAsta(int ID_Asta)
        {
            var asta = await _dbContext.Aste.FirstOrDefaultAsync(a => a.ID_Asta == ID_Asta);
            if (asta == null)
            {
                throw new KeyNotFoundException($"Asta con ID {ID_Asta} non trovata.");
            }

            _dbContext.Aste.Remove(asta);
            await _dbContext.SaveChangesAsync();
        }

        // Ottieni un'asta tramite ID
        public async Task<AstaDto> GetAstaById(int ID_Asta)
        {
            var asta = await _dbContext.Aste
                .Include(a => a.Modalita)
                .Include(a => a.Utente)
                .FirstOrDefaultAsync(a => a.ID_Asta == ID_Asta);

            if (asta == null)
            {
                return null;
            }

            return new AstaDto
            {
                ID_Asta = asta.ID_Asta,
                ID_Utente = asta.ID_Utente,
                ID_TipoAsta = asta.ID_TipoAsta,
                NumeroSquadre = asta.NumeroSquadre,
                CreditiDisponibili = asta.CreditiDisponibili,
                ID_Modalita = asta.ID_Modalita,
                NomeUtente = asta.Utente?.Nome,
                NomeModalita = asta.Modalita?.TipoModalita,
                TipoAstaDescrizione = asta.TipoAsta?.NomeTipoAsta
            };
        }

        // Ottieni tutte le aste
        public async Task<IEnumerable<AstaDto>> GetAll()
        {
            var aste = await _dbContext.Aste
                .Include(a => a.Modalita)  // Include Modalita
                .Include(a => a.Utente)    // Include Utente
                .Include(a => a.TipoAsta)  // Include TipoAsta per caricare la descrizione
                .ToListAsync();

            return aste.Select(a => new AstaDto
            {
                ID_Asta = a.ID_Asta,
                ID_Utente = a.ID_Utente,
                ID_TipoAsta = a.ID_TipoAsta,
                NumeroSquadre = a.NumeroSquadre,
                CreditiDisponibili = a.CreditiDisponibili,
                ID_Modalita = a.ID_Modalita,
                NomeUtente = a.Utente?.Nome,
                NomeModalita = a.Modalita?.TipoModalita,
                TipoAstaDescrizione = a.TipoAsta?.NomeTipoAsta  // Aggiungi la descrizione di TipoAsta
            }).ToList();
        }
    }
}
