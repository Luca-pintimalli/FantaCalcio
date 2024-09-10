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
        public async Task<Asta> AddAsta(int userId, AstaCreateUpdateDto astaDto)
        {
            var asta = new Asta
            {
                ID_Utente = userId,  // Inserisci l'ID utente recuperato dal JWT
                ID_TipoAsta = astaDto.ID_TipoAsta,
                NumeroSquadre = astaDto.NumeroSquadre,
                CreditiDisponibili = astaDto.CreditiDisponibili,
                ID_Modalita = astaDto.ID_Modalita,
                // Aggiungi i limiti per ruolo
                MaxPortieri = astaDto.MaxPortieri,
                MaxDifensori = astaDto.MaxDifensori,
                MaxCentrocampisti = astaDto.MaxCentrocampisti,
                MaxAttaccanti = astaDto.MaxAttaccanti
            };

            _dbContext.Aste.Add(asta);
            await _dbContext.SaveChangesAsync();

            return asta;
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

            // Aggiorna i limiti per ruolo
            astaEsistente.MaxPortieri = astaDto.MaxPortieri;
            astaEsistente.MaxDifensori = astaDto.MaxDifensori;
            astaEsistente.MaxCentrocampisti = astaDto.MaxCentrocampisti;
            astaEsistente.MaxAttaccanti = astaDto.MaxAttaccanti;

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
                .Include(a => a.TipoAsta)  // Include TipoAsta per caricare la descrizione
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
                TipoAstaDescrizione = asta.TipoAsta?.NomeTipoAsta,

                // Aggiungi i limiti per ruolo
                MaxPortieri = asta.MaxPortieri,
                MaxDifensori = asta.MaxDifensori,
                MaxCentrocampisti = asta.MaxCentrocampisti,
                MaxAttaccanti = asta.MaxAttaccanti
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
                TipoAstaDescrizione = a.TipoAsta?.NomeTipoAsta,

                // Aggiungi i limiti per ruolo
                MaxPortieri = a.MaxPortieri,
                MaxDifensori = a.MaxDifensori,
                MaxCentrocampisti = a.MaxCentrocampisti,
                MaxAttaccanti = a.MaxAttaccanti
            }).ToList();
        }


        // Metodo per gestire il prossimo giocatore in base al tipo d'asta
        public async Task<Giocatore> ProssimoGiocatoreAsync(int squadraId)
        {
            var squadra = await _dbContext.Squadre
                .Include(s => s.Asta)
                .FirstOrDefaultAsync(s => s.ID_Squadra == squadraId);

            if (squadra == null || squadra.Asta == null)
            {
                throw new Exception("Squadra o asta non trovata.");
            }

            var asta = squadra.Asta;

            // Verifica il tipo d'asta
            if (asta.ID_TipoAsta == 2) // Se l'asta è di tipo random (ID 2)
            {
                return await SelezionaGiocatoreRandomAsync(); // Nessun parametro necessario
            }
            else if (asta.ID_TipoAsta == 1) // Se l'asta è di tipo 'a chiamata' (ID 1)
            {
                throw new Exception("L'utente deve cercare il giocatore.");
            }

            throw new Exception("Tipo d'asta non riconosciuto.");
        }


        // Metodo per selezionare un giocatore casuale in base alla squadra
        public async Task<Giocatore> SelezionaGiocatoreRandomAsync()
        {
            // Seleziona tutti i giocatori che non sono stati già assegnati (o svincolati)
            var giocatoriDisponibili = await _dbContext.Giocatori
                .Where(g => !_dbContext.Operazioni.Any(o => o.ID_Giocatore == g.ID_Giocatore))
                .ToListAsync();

            if (!giocatoriDisponibili.Any())
            {
                throw new Exception("Nessun giocatore disponibile.");
            }

            // Seleziona un giocatore casuale dalla lista
            var random = new Random();
            return giocatoriDisponibili[random.Next(giocatoriDisponibili.Count)];
        }


        // Metodo per cercare un giocatore per cognome
        public async Task<Giocatore> CercaGiocatorePerCognomeAsync(int squadraId, string cognome)
        {
            var squadra = await _dbContext.Squadre
                .Include(s => s.Asta)
                .FirstOrDefaultAsync(s => s.ID_Squadra == squadraId);

            if (squadra == null || squadra.Asta == null)
            {
                throw new Exception("Squadra o asta non trovata.");
            }

            var giocatore = await _dbContext.Giocatori
                .FirstOrDefaultAsync(g => g.Cognome.ToLower() == cognome.ToLower()
                    && !_dbContext.Operazioni.Any(o => o.ID_Giocatore == g.ID_Giocatore && o.ID_Squadra == squadraId));

            if (giocatore == null)
            {
                throw new Exception($"Giocatore con cognome {cognome} non trovato o già assegnato.");
            }

            return giocatore;
        }

    }
}

