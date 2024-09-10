using System;
using FantaCalcio.Data;
using FantaCalcio.DTOs;
using FantaCalcio.Models;
using FantaCalcio.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace FantaCalcio.Services
{
    public class OperazioneService : IOperazioneService
    {
        private readonly AppDbContext _dbContext;

        public OperazioneService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateOperazione(OperazioneDto operazioneDto)
        {
            // Recupera la squadra associata all'operazione
            var squadra = await _dbContext.Squadre
                .Include(s => s.Giocatori)  // Include i giocatori per contare i ruoli
                .FirstOrDefaultAsync(s => s.ID_Squadra == operazioneDto.ID_Squadra);

            if (squadra == null)
            {
                throw new KeyNotFoundException($"Squadra con ID {operazioneDto.ID_Squadra} non trovata.");
            }

            // Recupera l'asta associata alla squadra
            var asta = await _dbContext.Aste.FirstOrDefaultAsync(a => a.ID_Asta == squadra.ID_Asta);

            if (asta == null)
            {
                throw new KeyNotFoundException($"Asta con ID {squadra.ID_Asta} non trovata.");
            }

            // Recupera il giocatore associato all'operazione per ottenere il ruolo
            var giocatore = await _dbContext.Giocatori.FirstOrDefaultAsync(g => g.ID_Giocatore == operazioneDto.ID_Giocatore);

            if (giocatore == null)
            {
                throw new KeyNotFoundException($"Giocatore con ID {operazioneDto.ID_Giocatore} non trovato.");
            }

            // Verifica il numero di giocatori per ruolo utilizzando RuoloClassic
            var numeroGiocatoriPerRuolo = squadra.Giocatori
                .Count(g => g.RuoloClassic == giocatore.RuoloClassic);

            // Verifica i limiti per ruolo in base all'asta
            switch (giocatore.RuoloClassic)
            {
                case "Portiere":
                    if (numeroGiocatoriPerRuolo >= asta.MaxPortieri)
                    {
                        throw new InvalidOperationException("Hai già raggiunto il numero massimo di portieri.");
                    }
                    break;
                case "Difensore":
                    if (numeroGiocatoriPerRuolo >= asta.MaxDifensori)
                    {
                        throw new InvalidOperationException("Hai già raggiunto il numero massimo di difensori.");
                    }
                    break;
                case "Centrocampista":
                    if (numeroGiocatoriPerRuolo >= asta.MaxCentrocampisti)
                    {
                        throw new InvalidOperationException("Hai già raggiunto il numero massimo di centrocampisti.");
                    }
                    break;
                case "Attaccante":
                    if (numeroGiocatoriPerRuolo >= asta.MaxAttaccanti)
                    {
                        throw new InvalidOperationException("Hai già raggiunto il numero massimo di attaccanti.");
                    }
                    break;
            }

            // Verifica se la squadra ha crediti sufficienti per completare l'operazione
            if (squadra.CreditiTotali - squadra.CreditiSpesi < operazioneDto.CreditiSpesi)
            {
                throw new InvalidOperationException("Crediti insufficienti per completare l'operazione.");
            }

            // Sottrai i crediti spesi
            squadra.CreditiSpesi += operazioneDto.CreditiSpesi;

            // Crea una nuova operazione (acquisto del giocatore)
            var operazione = new Operazione
            {
                ID_Giocatore = operazioneDto.ID_Giocatore,
                ID_Squadra = operazioneDto.ID_Squadra,
                CreditiSpesi = operazioneDto.CreditiSpesi,
                StatoOperazione = operazioneDto.StatoOperazione,
                DataOperazione = operazioneDto.DataOperazione
            };

            _dbContext.Operazioni.Add(operazione);

            // Salva i cambiamenti nel database (inclusi i crediti aggiornati e la nuova operazione)
            await _dbContext.SaveChangesAsync();
        }




        // Aggiorna un'operazione esistente
        public async Task UpdateOperazione(int ID_Operazione, OperazioneDto operazioneDto)
        {
            var operazioneEsistente = await _dbContext.Operazioni.FirstOrDefaultAsync(o => o.ID_Operazione == ID_Operazione);

            if (operazioneEsistente == null)
            {
                throw new KeyNotFoundException($"Operazione con ID {ID_Operazione} non trovata.");
            }

            // Aggiorna i campi dell'operazione
            operazioneEsistente.ID_Giocatore = operazioneDto.ID_Giocatore;
            operazioneEsistente.ID_Squadra = operazioneDto.ID_Squadra;
            operazioneEsistente.CreditiSpesi = operazioneDto.CreditiSpesi;
            operazioneEsistente.StatoOperazione = operazioneDto.StatoOperazione;
            operazioneEsistente.DataOperazione = operazioneDto.DataOperazione;

            await _dbContext.SaveChangesAsync();
        }

        // Cancella un'operazione esistente
        public async Task DeleteOperazione(int ID_Operazione)
        {
            var operazione = await _dbContext.Operazioni.FirstOrDefaultAsync(o => o.ID_Operazione == ID_Operazione);

            if (operazione == null)
            {
                throw new KeyNotFoundException($"Operazione con ID {ID_Operazione} non trovata.");
            }

            _dbContext.Operazioni.Remove(operazione);
            await _dbContext.SaveChangesAsync();
        }

        // Ottieni un'operazione per ID
        public async Task<OperazioneDto> GetOperazioneById(int ID_Operazione)
        {
            var operazione = await _dbContext.Operazioni
                .Include(o => o.Giocatore)
                .Include(o => o.Squadra)
                .FirstOrDefaultAsync(o => o.ID_Operazione == ID_Operazione);

            if (operazione == null)
            {
                return null;
            }

            return new OperazioneDto
            {
                ID_Operazione = operazione.ID_Operazione,
                ID_Giocatore = operazione.ID_Giocatore,
                ID_Squadra = operazione.ID_Squadra,
                CreditiSpesi = operazione.CreditiSpesi,
                StatoOperazione = operazione.StatoOperazione,
                DataOperazione = operazione.DataOperazione
            };
        }

        // Ottieni tutte le operazioni
        public async Task<IEnumerable<OperazioneDto>> GetAll()
        {
            var operazioni = await _dbContext.Operazioni
                .Include(o => o.Giocatore)
                .Include(o => o.Squadra)
                .ToListAsync();

            return operazioni.Select(o => new OperazioneDto
            {
                ID_Operazione = o.ID_Operazione,
                ID_Giocatore = o.ID_Giocatore,
                ID_Squadra = o.ID_Squadra,
                CreditiSpesi = o.CreditiSpesi,
                StatoOperazione = o.StatoOperazione,
                DataOperazione = o.DataOperazione
            }).ToList();
        }
    }
}