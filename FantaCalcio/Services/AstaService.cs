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
                // Log: Creazione Asta
                Console.WriteLine($"Creazione asta: ID_Utente={userId}, CreditiDisponibili={astaDto.CreditiDisponibili}, NumeroSquadre={astaDto.NumeroSquadre}");

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
                        Stemma = "/uploads/default-stemma.png",
                        CreditiTotali = astaDto.CreditiDisponibili,
                        CreditiSpesi = 0
                    };

                    _dbContext.Squadre.Add(squadra);
                    Console.WriteLine($"Squadra {squadra.Nome} creata correttamente con CreditiTotali={squadra.CreditiTotali}, CreditiSpesi={squadra.CreditiSpesi} per l'asta {asta.ID_Asta}.");
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
                        CreditiSpesi = 0,
                        Stemma = "/uploads/default-stemma.png" // Imposta il valore di default
                    };
                    _dbContext.Squadre.Add(nuovaSquadra);
                }
            }

            // Quando aggiorni squadre esistenti, verifica il campo Stemma
            foreach (var squadra in astaEsistente.Squadre)
            {
                if (string.IsNullOrEmpty(squadra.Stemma))
                {
                    squadra.Stemma = "/uploads/default-stemma.png"; // Imposta il valore di default solo se non esiste già
                }
            }

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
            return await SelezionaGiocatoreRandomAsync(squadraId);
        }
        public async Task<Giocatore> SelezionaGiocatoreRandomAsync(int idAsta)
        {
            // Recupera i giocatori che non sono stati assegnati né svincolati nell'asta corrente
            var giocatoriDisponibili = await _dbContext.Giocatori
                .Include(g => g.RuoliMantra)
                .ThenInclude(rm => rm.Ruolo)
                .Where(g => !_dbContext.Operazioni
                    .Any(o => o.ID_Giocatore == g.ID_Giocatore && o.ID_Asta == idAsta && (o.StatoOperazione == "Assegnato" || o.StatoOperazione == "Svincolato")))
                .ToListAsync();

            if (!giocatoriDisponibili.Any())
            {
                throw new Exception("Nessun giocatore disponibile per questa asta.");
            }

            var random = new Random();
            return giocatoriDisponibili[random.Next(giocatoriDisponibili.Count)];
        }




        // Metodo per cercare un giocatore per cognome
        public async Task<Giocatore> CercaGiocatoreAsync(int idAsta, string? nome, string? cognome)
        {
            // Verifica che almeno uno dei parametri (nome o cognome) sia presente
            if (string.IsNullOrEmpty(nome) && string.IsNullOrEmpty(cognome))
            {
                throw new Exception("Devi fornire almeno il nome o il cognome per la ricerca.");
            }

            // Cerca un giocatore per nome o cognome, escludendo quelli già assegnati nell'asta corrente
            var giocatore = await _dbContext.Giocatori
                .Where(g => (string.IsNullOrEmpty(nome) || g.Nome == nome) &&
                            (string.IsNullOrEmpty(cognome) || g.Cognome == cognome) &&
                            !_dbContext.Operazioni
                                .Any(o => o.ID_Giocatore == g.ID_Giocatore && o.Squadra.ID_Asta == idAsta))
                .FirstOrDefaultAsync();

            if (giocatore == null)
            {
                throw new Exception("Giocatore non trovato o già assegnato in questa asta.");
            }

            return giocatore;
        }



    }
}