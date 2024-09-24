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
    public class GiocatoriService : IGiocatoreService
    {
        private readonly AppDbContext _dbContext;

        public GiocatoriService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Aggiungi un giocatore senza gestire i ruoli Mantra
        public async Task AddGiocatore(GiocatoreCreateUpdateDto giocatoreDto, string filePath)
        {
            var giocatore = new Giocatore
            {
                Nome = giocatoreDto.Nome,
                Cognome = giocatoreDto.Cognome,
                Foto = filePath, // Utilizza il percorso dell'immagine fornito
                SquadraAttuale = giocatoreDto.SquadraAttuale,
                GoalFatti = giocatoreDto.GoalFatti,
                GoalSubiti = giocatoreDto.GoalSubiti,
                Assist = giocatoreDto.Assist,
                PartiteGiocate = giocatoreDto.PartiteGiocate,
                RuoloClassic = giocatoreDto.RuoloClassic
            };

            _dbContext.Giocatori.Add(giocatore);
            await _dbContext.SaveChangesAsync();
        }





        public async Task UpdateGiocatore(int id, GiocatoreCreateUpdateDto giocatoreDto, IFormFile file)
        {
            var giocatoreEsistente = await _dbContext.Giocatori.FindAsync(id);

            if (giocatoreEsistente == null)
            {
                throw new KeyNotFoundException($"Giocatore con ID {id} non trovato.");
            }

            // Aggiorna i campi del giocatore esistente
            giocatoreEsistente.Nome = giocatoreDto.Nome ?? giocatoreEsistente.Nome;
            giocatoreEsistente.Cognome = giocatoreDto.Cognome ?? giocatoreEsistente.Cognome;
            giocatoreEsistente.SquadraAttuale = giocatoreDto.SquadraAttuale ?? giocatoreEsistente.SquadraAttuale;
            giocatoreEsistente.GoalFatti = giocatoreDto.GoalFatti >= 0 ? giocatoreDto.GoalFatti : giocatoreEsistente.GoalFatti;
            giocatoreEsistente.GoalSubiti = giocatoreDto.GoalSubiti >= 0 ? giocatoreDto.GoalSubiti : giocatoreEsistente.GoalSubiti;
            giocatoreEsistente.Assist = giocatoreDto.Assist >= 0 ? giocatoreDto.Assist : giocatoreEsistente.Assist;
            giocatoreEsistente.PartiteGiocate = giocatoreDto.PartiteGiocate >= 0 ? giocatoreDto.PartiteGiocate : giocatoreEsistente.PartiteGiocate;
            giocatoreEsistente.RuoloClassic = giocatoreDto.RuoloClassic ?? giocatoreEsistente.RuoloClassic;
            giocatoreEsistente.StatoGiocatore = giocatoreDto.StatoGiocatore ?? giocatoreEsistente.StatoGiocatore; // Aggiornamento stato

            // Aggiorna il percorso dell'immagine solo se è stato fornito un nuovo file
            if (file != null && file.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                giocatoreEsistente.Foto = $"/uploads/{uniqueFileName}";
            }

            // Salva le modifiche nel database
            await _dbContext.SaveChangesAsync();
        }





        // Ottenere tutti i giocatori
        public async Task<IEnumerable<GiocatoreDto>> GetAll(string ruolo = null, string search = null)
        {
            var query = _dbContext.Giocatori
                .Include(g => g.RuoliMantra)
                .ThenInclude(rm => rm.Ruolo)
                .AsQueryable();

            if (!string.IsNullOrEmpty(ruolo))
            {
                query = query.Where(g => g.RuoloClassic.ToUpper() == ruolo.ToUpper());
            }

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(g => g.Nome.ToUpper().Contains(search.ToUpper()) ||
                                         g.Cognome.ToUpper().Contains(search.ToUpper()));
            }

            return await query
                .Select(g => new GiocatoreDto
                {
                    ID_Giocatore = g.ID_Giocatore,
                    Nome = g.Nome,
                    Cognome = g.Cognome,
                    SquadraAttuale = g.SquadraAttuale,
                    GoalFatti = g.GoalFatti,
                    GoalSubiti = g.GoalSubiti,
                    Assist = g.Assist,
                    PartiteGiocate = g.PartiteGiocate,
                    RuoloClassic = g.RuoloClassic,
                    RuoliMantra = g.RuoliMantra.Select(rm => new RuoloMantraDTO
                    {
                        ID = rm.ID,
                        ID_Giocatore = rm.ID_Giocatore,
                        NomeGiocatore = g.Nome + " " + g.Cognome,
                        ID_Ruolo = rm.ID_Ruolo,
                        NomeRuolo = rm.Ruolo.NomeRuolo
                    }).ToList()
                })
                .OrderBy(g => g.RuoloClassic == "PORTIERE" ? 1 :
                              g.RuoloClassic == "DIFENSORE" ? 2 :
                              g.RuoloClassic == "CENTROCAMPISTA" ? 3 :
                              g.RuoloClassic == "ATTACCANTE" ? 4 : 5)
                .ToListAsync();
        }

        // Ottenere giocatore per ID
        public async Task<GiocatoreDto> GetGiocatoreById(int id)
        {
            try
            {
                // Ricerca del giocatore con i ruoli Mantra associati
                var giocatore = await _dbContext.Giocatori
                    .Include(g => g.RuoliMantra)
                    .ThenInclude(rm => rm.Ruolo)
                    .FirstOrDefaultAsync(g => g.ID_Giocatore == id);

                if (giocatore == null)
                {
                    return null;
                }

                // Creazione del DTO e gestione dei campi che potrebbero essere null
                return new GiocatoreDto
                {
                    ID_Giocatore = giocatore.ID_Giocatore,
                    Nome = giocatore.Nome ?? "Nome non disponibile", // Gestisci null
                    Cognome = giocatore.Cognome ?? "Cognome non disponibile", // Gestisci null
                    Foto = giocatore.Foto ?? "Nessuna foto disponibile", // Gestisci null
                    SquadraAttuale = giocatore.SquadraAttuale ?? "Squadra non disponibile", // Gestisci null
                    GoalFatti = giocatore.GoalFatti,
                    GoalSubiti = giocatore.GoalSubiti,
                    Assist = giocatore.Assist,
                    PartiteGiocate = giocatore.PartiteGiocate,
                    RuoloClassic = giocatore.RuoloClassic ?? "Ruolo non disponibile", // Gestisci null

                    // Gestione dei ruoli Mantra
                    RuoliMantra = giocatore.RuoliMantra?.Select(rm => new RuoloMantraDTO
                    {
                        ID = rm.ID,
                        ID_Giocatore = rm.ID_Giocatore,
                        NomeGiocatore = giocatore.Nome + " " + giocatore.Cognome,
                        ID_Ruolo = rm.ID_Ruolo,
                        NomeRuolo = rm.Ruolo?.NomeRuolo ?? "Ruolo non disponibile" // Gestisci null
                    }).ToList() ?? new List<RuoloMantraDTO>() // Se non ci sono ruoli, restituisci una lista vuota
                };
            }
            catch (Exception ex)
            {
                // Log dell'errore per identificare il punto esatto dell'eccezione
                Console.WriteLine($"Errore durante il recupero del giocatore con ID {id}: {ex.Message}");
                throw;
            }
        }


        // Eliminare un giocatore
        public async Task DeleteGiocatore(int ID_Giocatore)
        {
            var giocatore = await _dbContext.Giocatori.FirstOrDefaultAsync(g => g.ID_Giocatore == ID_Giocatore);

            if (giocatore == null)
            {
                Console.WriteLine($"Giocatore con ID {ID_Giocatore} non trovato.");
                throw new Exception($"Giocatore con ID {ID_Giocatore} non trovato.");
            }

            Console.WriteLine($"Trovato giocatore con ID {ID_Giocatore}.");

            var ruoliMantraAssociati = await _dbContext.RuoloMantra
                .Where(rm => rm.ID_Giocatore == ID_Giocatore)
                .ToListAsync();

            if (ruoliMantraAssociati != null && ruoliMantraAssociati.Any())
            {
                Console.WriteLine($"Trovati {ruoliMantraAssociati.Count} ruoli associati. Procedo con l'eliminazione.");
                _dbContext.RuoloMantra.RemoveRange(ruoliMantraAssociati);
            }
            else
            {
                Console.WriteLine("Nessun ruolo Mantra associato trovato.");
            }

            _dbContext.Giocatori.Remove(giocatore);

            try
            {
                await _dbContext.SaveChangesAsync();
                Console.WriteLine("Giocatore eliminato con successo.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore durante l'eliminazione: {ex.Message}");
                throw;
            }
        }
        public async Task<IEnumerable<GiocatoreDto>> GetGiocatoriDisponibili(int idAsta)
        {
            var giocatoriDisponibili = await _dbContext.Giocatori
                .Where(g => !_dbContext.Operazioni
                                .Any(o => o.ID_Giocatore == g.ID_Giocatore
                                          && o.ID_Asta == idAsta
                                          && (o.StatoOperazione == "Assegnato" || o.StatoOperazione == "Svincolato"))) // Escludi giocatori con operazioni attive o svincolati
                .ToListAsync();

            return giocatoriDisponibili.Select(g => new GiocatoreDto
            {
                ID_Giocatore = g.ID_Giocatore,
                Nome = g.Nome,
                Cognome = g.Cognome,
                RuoloClassic = g.RuoloClassic,
                SquadraAttuale = g.SquadraAttuale,
                Foto = g.Foto
            }).AsEnumerable();
        }


        public async Task SvincolaGiocatoreAsync(int idGiocatore)
        {
            var giocatore = await _dbContext.Giocatori.FindAsync(idGiocatore);
            if (giocatore == null)
            {
                throw new KeyNotFoundException($"Giocatore con ID {idGiocatore} non trovato.");
            }

            // Imposta lo stato del giocatore come "Svincolato"
            giocatore.StatoGiocatore = "Svincolato";
            await _dbContext.SaveChangesAsync();
        }

        public async Task RipristinaGiocatoreAsync(int idGiocatore)
        {
            var giocatore = await _dbContext.Giocatori.FindAsync(idGiocatore);
            if (giocatore == null)
            {
                throw new KeyNotFoundException($"Giocatore con ID {idGiocatore} non trovato.");
            }

            // Ripristina lo stato del giocatore a "Disponibile"
            giocatore.StatoGiocatore = "Disponibile";
            await _dbContext.SaveChangesAsync();
        }
    

    public async Task UpdateGiocatoreStato(int idGiocatore, string nuovoStato)
        {
            // Trova il giocatore nel database
            var giocatore = await _dbContext.Giocatori.FirstOrDefaultAsync(g => g.ID_Giocatore == idGiocatore);

            // Verifica che il giocatore esista
            if (giocatore == null)
            {
                throw new KeyNotFoundException($"Giocatore con ID {idGiocatore} non trovato.");
            }

            // Aggiorna lo stato del giocatore
            giocatore.StatoGiocatore = nuovoStato;

            // Salva le modifiche nel database
            await _dbContext.SaveChangesAsync();
        }



    }
}
