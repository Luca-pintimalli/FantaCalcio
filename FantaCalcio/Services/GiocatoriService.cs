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
    public class GiocatoriService : IGiocatoreService
    {
        private readonly AppDbContext _dbContext;

        public GiocatoriService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Aggiungi un giocatore senza gestire i ruoli Mantra
        public async Task AddGiocatore(GiocatoreCreateUpdateDto giocatoreDTO)
        {
            var giocatore = new Giocatore
            {
                Nome = giocatoreDTO.Nome,
                Cognome = giocatoreDTO.Cognome,
                Foto = giocatoreDTO.Foto,
                SquadraAttuale = giocatoreDTO.SquadraAttuale,
                GoalFatti = giocatoreDTO.GoalFatti,
                GoalSubiti = giocatoreDTO.GoalSubiti,
                Assist = giocatoreDTO.Assist,
                PartiteGiocate = giocatoreDTO.PartiteGiocate,
                RuoloClassic = giocatoreDTO.RuoloClassic
            };

            _dbContext.Giocatori.Add(giocatore);
            await _dbContext.SaveChangesAsync();
        }

        // Aggiorna un giocatore (solo i dettagli del giocatore, non i ruoli Mantra)
        public async Task UpdateGiocatore(int id, GiocatoreCreateUpdateDto giocatoreDTO)
        {
            var giocatoreEsistente = await _dbContext.Giocatori.FindAsync(id);

            if (giocatoreEsistente == null)
            {
                throw new Exception($"Giocatore con ID {id} non trovato.");
            }

            giocatoreEsistente.Nome = giocatoreDTO.Nome;
            giocatoreEsistente.Cognome = giocatoreDTO.Cognome;
            giocatoreEsistente.Foto = giocatoreDTO.Foto;
            giocatoreEsistente.SquadraAttuale = giocatoreDTO.SquadraAttuale;
            giocatoreEsistente.GoalFatti = giocatoreDTO.GoalFatti;
            giocatoreEsistente.GoalSubiti = giocatoreDTO.GoalSubiti;
            giocatoreEsistente.Assist = giocatoreDTO.Assist;
            giocatoreEsistente.PartiteGiocate = giocatoreDTO.PartiteGiocate;
            giocatoreEsistente.RuoloClassic = giocatoreDTO.RuoloClassic;

            await _dbContext.SaveChangesAsync();
        }

        // Ottenere tutti i giocatori
        public async Task<IEnumerable<GiocatoreDto>> GetAll()
        {
            return await _dbContext.Giocatori
                .Include(g => g.RuoliMantra)
                .ThenInclude(rm => rm.Ruolo)
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
                    // Mappa i ruoli Mantra con il nome del giocatore
                    RuoliMantra = g.RuoliMantra.Select(rm => new RuoloMantraDTO
                    {
                        ID = rm.ID,
                        ID_Giocatore = rm.ID_Giocatore,
                        NomeGiocatore = g.Nome + " " + g.Cognome, 
                        ID_Ruolo = rm.ID_Ruolo,
                        NomeRuolo = rm.Ruolo.NomeRuolo
                    }).ToList()
                })
                .ToListAsync();
        }


        // Ottenere giocatore per ID
        public async Task<GiocatoreDto> GetGiocatoreById(int id)
        {
            var giocatore = await _dbContext.Giocatori
                .Include(g => g.RuoliMantra)
                .ThenInclude(rm => rm.Ruolo)
                .FirstOrDefaultAsync(g => g.ID_Giocatore == id);

            if (giocatore == null)
            {
                return null;
            }

            return new GiocatoreDto
            {
                ID_Giocatore = giocatore.ID_Giocatore,
                Nome = giocatore.Nome,
                Cognome = giocatore.Cognome,
                SquadraAttuale = giocatore.SquadraAttuale,
                GoalFatti = giocatore.GoalFatti,
                GoalSubiti = giocatore.GoalSubiti,
                Assist = giocatore.Assist,
                PartiteGiocate = giocatore.PartiteGiocate,
                RuoloClassic = giocatore.RuoloClassic,
                RuoliMantra = giocatore.RuoliMantra.Select(rm => new RuoloMantraDTO
                {
                    ID = rm.ID,
                    ID_Giocatore = rm.ID_Giocatore,
                    NomeGiocatore = giocatore.Nome + " " + giocatore.Cognome, 
                    ID_Ruolo = rm.ID_Ruolo,
                    NomeRuolo = rm.Ruolo.NomeRuolo
                }).ToList()
            };
        }


        // Eliminare un giocatore
        public async Task DeleteGiocatore(int ID_Giocatore)
        {
            var giocatore = await _dbContext.Giocatori.FirstOrDefaultAsync(g => g.ID_Giocatore == ID_Giocatore);
            if (giocatore == null)
            {
                throw new Exception($"Giocatore con ID {ID_Giocatore} non trovato.");
            }

            // Elimina le associazioni con RuoloMantra prima di eliminare il giocatore
            var ruoliMantraAssociati = _dbContext.RuoloMantra
                .Where(rm => rm.ID_Giocatore == ID_Giocatore);

            _dbContext.RuoloMantra.RemoveRange(ruoliMantraAssociati);

            // Ora elimina il giocatore
            _dbContext.Giocatori.Remove(giocatore);
            await _dbContext.SaveChangesAsync();
        }

    }
}
