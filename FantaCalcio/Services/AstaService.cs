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

        public AstaService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Asta> AddAsta(int userId, AstaCreateUpdateDto astaDto)
        {
            var asta = new Asta
            {
                ID_Utente = userId,
                ID_TipoAsta = astaDto.ID_TipoAsta,
                NumeroSquadre = astaDto.NumeroSquadre,
                CreditiDisponibili = astaDto.CreditiDisponibili,
                ID_Modalita = astaDto.ID_Modalita,
                MaxPortieri = astaDto.MaxPortieri,
                MaxDifensori = astaDto.MaxDifensori,
                MaxCentrocampisti = astaDto.MaxCentrocampisti,
                MaxAttaccanti = astaDto.MaxAttaccanti
            };

            try
            {
                _dbContext.Aste.Add(asta);
                await _dbContext.SaveChangesAsync();
                Console.WriteLine($"Asta {asta.ID_Asta} creata correttamente.");

                // Creazione delle squadre
                for (int i = 1; i <= astaDto.NumeroSquadre; i++)
                {
                    var squadra = new Squadra
                    {
                        ID_Asta = asta.ID_Asta,
                        Nome = $"Squadra {i}",
                        Stemma = "/uploads/default-stemma.png",  // Percorso corretto per l'immagine di default
                        CreditiSpesi = 0
                    };

                    _dbContext.Squadre.Add(squadra);
                    Console.WriteLine($"Squadra {squadra.Nome} creata correttamente per l'asta {asta.ID_Asta}.");
                }

                await _dbContext.SaveChangesAsync();
                Console.WriteLine($"Tutte le squadre per l'asta {asta.ID_Asta} sono state create correttamente.");
            }
            catch (DbUpdateException dbEx)
            {
                Console.WriteLine($"Errore durante il salvataggio nel database: {dbEx.Message} - Inner Exception: {dbEx.InnerException?.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore generico durante la creazione dell'asta: {ex.Message}");
                throw;
            }

            return asta;
        }




        public async Task UpdateAsta(int ID_Asta, int userId, AstaCreateUpdateDto astaDto)
        {
            var astaEsistente = await _dbContext.Aste.Include(a => a.Squadre) // Include le squadre associate
                                                      .FirstOrDefaultAsync(a => a.ID_Asta == ID_Asta);

            if (astaEsistente == null)
            {
                throw new KeyNotFoundException($"Asta con ID {ID_Asta} non trovata.");
            }

            // Aggiorna i campi con i dati della DTO
            astaEsistente.ID_TipoAsta = astaDto.ID_TipoAsta;
            astaEsistente.NumeroSquadre = astaDto.NumeroSquadre;
            astaEsistente.CreditiDisponibili = astaDto.CreditiDisponibili;
            astaEsistente.ID_Modalita = astaDto.ID_Modalita;
            astaEsistente.ID_Utente = userId;

            // Aggiorna i limiti per ruolo
            astaEsistente.MaxPortieri = astaDto.MaxPortieri;
            astaEsistente.MaxDifensori = astaDto.MaxDifensori;
            astaEsistente.MaxCentrocampisti = astaDto.MaxCentrocampisti;
            astaEsistente.MaxAttaccanti = astaDto.MaxAttaccanti;

            // Se il numero di squadre è stato ridotto
            if (astaDto.NumeroSquadre < astaEsistente.Squadre.Count)
            {
                // Trova le squadre da rimuovere
                var squadreDaRimuovere = astaEsistente.Squadre
                                        .OrderByDescending(s => s.ID_Squadra) // Rimuovi le ultime squadre create
                                        .Take(astaEsistente.Squadre.Count - astaDto.NumeroSquadre)
                                        .ToList();

                // Rimuovi le squadre in eccesso
                _dbContext.Squadre.RemoveRange(squadreDaRimuovere);
            }

            // Se il numero di squadre è stato aumentato
            if (astaDto.NumeroSquadre > astaEsistente.Squadre.Count)
            {
                var squadreDaAggiungere = astaDto.NumeroSquadre - astaEsistente.Squadre.Count;
                for (int i = 1; i <= squadreDaAggiungere; i++)
                {
                    var nuovaSquadra = new Squadra
                    {
                        Nome = $"Squadra {astaEsistente.Squadre.Count + i}",
                        ID_Asta = astaEsistente.ID_Asta,
                        CreditiTotali = astaEsistente.CreditiDisponibili,
                        CreditiSpesi = 0
                    };
                    _dbContext.Squadre.Add(nuovaSquadra);
                }
            }

            // Salva le modifiche
            await _dbContext.SaveChangesAsync();
        }


        // Elimina un'asta e tutte le entità collegate (ad es. Squadre)
        public async Task DeleteAsta(int ID_Asta)
        {
            // Include le squadre collegate all'asta
            var asta = await _dbContext.Aste.Include(a => a.Squadre)
                                            .FirstOrDefaultAsync(a => a.ID_Asta == ID_Asta);
            if (asta == null)
            {
                throw new KeyNotFoundException($"Asta con ID {ID_Asta} non trovata.");
            }

            // Se ci sono squadre collegate all'asta, rimuovile
            if (asta.Squadre != null && asta.Squadre.Any())
            {
                _dbContext.Squadre.RemoveRange(asta.Squadre);
            }

            // Rimuovi l'asta
            _dbContext.Aste.Remove(asta);

            // Salva le modifiche nel database
            await _dbContext.SaveChangesAsync();
        }


        // Ottieni un'asta tramite ID
        public async Task<AstaDto> GetAstaById(int ID_Asta)
        {
            var asta = await _dbContext.Aste
                .Include(a => a.Modalita)
                .Include(a => a.Utente)
                .Include(a => a.TipoAsta)
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
                .Include(a => a.Modalita)
                .Include(a => a.Utente)
                .Include(a => a.TipoAsta)
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
                MaxPortieri = a.MaxPortieri,
                MaxDifensori = a.MaxDifensori,
                MaxCentrocampisti = a.MaxCentrocampisti,
                MaxAttaccanti = a.MaxAttaccanti
            }).ToList();
        }

        // Metodo per gestire il prossimo giocatore
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

            if (asta.ID_TipoAsta == 2) // Asta random
            {
                return await SelezionaGiocatoreRandomAsync();
            }
            else if (asta.ID_TipoAsta == 1) // Asta 'a chiamata'
            {
                throw new Exception("L'utente deve cercare il giocatore.");
            }

            throw new Exception("Tipo d'asta non riconosciuto.");
        }

        // Metodo per selezionare un giocatore casuale
        public async Task<Giocatore> SelezionaGiocatoreRandomAsync()
        {
            var giocatoriDisponibili = await _dbContext.Giocatori
                .Where(g => !_dbContext.Operazioni.Any(o => o.ID_Giocatore == g.ID_Giocatore))
                .ToListAsync();

            if (!giocatoriDisponibili.Any())
            {
                throw new Exception("Nessun giocatore disponibile.");
            }

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
