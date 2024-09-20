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
    public class SquadraService : ISquadraService
    {
        private readonly AppDbContext _dbContext;

        public SquadraService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Crea una nuova squadra associata a un'asta esistente
        public async Task CreateSquadra(int ID_Asta, SquadraCreateDto squadraDto)
        {
            // Verifica che l'asta esista
            var asta = await _dbContext.Aste.FirstOrDefaultAsync(a => a.ID_Asta == ID_Asta);
            if (asta == null)
            {
                throw new KeyNotFoundException($"Asta con ID {ID_Asta} non trovata.");
            }

            // Crea una nuova squadra associata all'asta, imposta i crediti totali dall'asta
            var squadra = new Squadra
            {
                ID_Asta = ID_Asta,
                Nome = squadraDto.Nome,
                Stemma = squadraDto.Stemma,
                CreditiTotali = asta.CreditiDisponibili,  // Imposta i crediti totali dall'asta
                CreditiSpesi = squadraDto.CreditiSpesi
            };

            _dbContext.Squadre.Add(squadra);
            await _dbContext.SaveChangesAsync();
        }


        // Implementazione del metodo UpdateSquadra con supporto per il file immagine

        // Metodo per aggiornare una squadra
        public async Task UpdateSquadra(int ID_Squadra, SquadraUpdateDto squadraDto, IFormFile? foto)
        {
            var squadraEsistente = await _dbContext.Squadre.FirstOrDefaultAsync(s => s.ID_Squadra == ID_Squadra);
            if (squadraEsistente == null)
            {
                throw new KeyNotFoundException($"Squadra con ID {ID_Squadra} non trovata.");
            }

            // Aggiorna i campi
            squadraEsistente.Nome = squadraDto.Nome;
            squadraEsistente.CreditiTotali = squadraDto.CreditiTotali;
            squadraEsistente.CreditiSpesi = squadraDto.CreditiSpesi;  // Aggiungi i crediti spesi

            // Gestisci l'immagine se presente
            if (foto != null)
            {
                var uploadsDir = Path.Combine("wwwroot/uploads");
                if (!Directory.Exists(uploadsDir))
                {
                    Directory.CreateDirectory(uploadsDir);
                }

                if (!string.IsNullOrEmpty(squadraEsistente.Stemma))
                {
                    var oldImagePath = Path.Combine(uploadsDir, Path.GetFileName(squadraEsistente.Stemma));
                    if (File.Exists(oldImagePath))
                    {
                        File.Delete(oldImagePath);
                    }
                }

                var filePath = Path.Combine(uploadsDir, foto.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await foto.CopyToAsync(stream);
                }

                squadraEsistente.Stemma = $"/uploads/{foto.FileName}";
            }

            await _dbContext.SaveChangesAsync();
        }




        // Cancella una squadra esistente
        public async Task DeleteSquadra(int ID_Squadra)
        {
            var squadra = await _dbContext.Squadre.FirstOrDefaultAsync(s => s.ID_Squadra == ID_Squadra);
            if (squadra == null)
            {
                throw new KeyNotFoundException($"Squadra con ID {ID_Squadra} non trovata.");
            }

            _dbContext.Squadre.Remove(squadra);
            await _dbContext.SaveChangesAsync();
        }

        // Ottieni una squadra per ID
        public async Task<SquadraDto> GetSquadraById(int ID_Squadra)
        {
            var squadra = await _dbContext.Squadre
                .Include(s => s.Giocatori)
                .Include(s => s.Operazioni)
                .Include(s => s.Asta)  // Include l'asta associata per ottenere le impostazioni
                .FirstOrDefaultAsync(s => s.ID_Squadra == ID_Squadra);

            if (squadra == null)
            {
                return null;
            }

            // Mappa i dati in un DTO
            return new SquadraDto
            {
                ID_Squadra = squadra.ID_Squadra,
                ID_Asta = squadra.ID_Asta,
                Nome = squadra.Nome,
                Stemma = squadra.Stemma,
                CreditiTotali = squadra.CreditiTotali,
                CreditiSpesi = squadra.CreditiSpesi,
                GiocatoriIds = squadra.Giocatori.Select(g => g.ID_Giocatore).ToList(),
                OperazioniIds = squadra.Operazioni.Select(o => o.ID_Operazione).ToList()
            };
        }

        // Ottieni tutte le squadre
        public async Task<IEnumerable<SquadraDto>> GetAll()
        {
            var squadre = await _dbContext.Squadre
                .Include(s => s.Giocatori)
                .Include(s => s.Operazioni)
                .Include(s => s.Asta)  // Include l'asta associata per ottenere le impostazioni
                .ToListAsync();

            return squadre.Select(s => new SquadraDto
            {
                ID_Squadra = s.ID_Squadra,
                ID_Asta = s.ID_Asta,
                Nome = s.Nome,
                Stemma = s.Stemma,
                CreditiTotali = s.CreditiTotali,
                CreditiSpesi = s.CreditiSpesi,
                GiocatoriIds = s.Giocatori.Select(g => g.ID_Giocatore).ToList(),
                OperazioniIds = s.Operazioni.Select(o => o.ID_Operazione).ToList()
            }).ToList();
        }
    }
}
