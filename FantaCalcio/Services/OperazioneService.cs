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
        public async Task<OperazioneDettagliataDto> CreaOperazione(OperazioneDto operazioneDto)
        {
            // Recupera il giocatore associato all'operazione
            var giocatore = await _dbContext.Giocatori.FirstOrDefaultAsync(g => g.ID_Giocatore == operazioneDto.ID_Giocatore);
            if (giocatore == null)
            {
                throw new KeyNotFoundException($"Giocatore con ID {operazioneDto.ID_Giocatore} non trovato.");
            }

            // Verifica se il giocatore ha già un'operazione attiva nella stessa asta o è stato svincolato
            var operazioneEsistente = await _dbContext.Operazioni
                .FirstOrDefaultAsync(o => o.ID_Giocatore == operazioneDto.ID_Giocatore
                                          && o.ID_Asta == operazioneDto.ID_Asta
                                          && (o.StatoOperazione == "Assegnato" || o.StatoOperazione == "Svincolato"));

            if (operazioneEsistente != null && operazioneDto.StatoOperazione != "Svincolato")
            {
                throw new InvalidOperationException("Il giocatore ha già un'operazione attiva o è stato svincolato in questa asta.");
            }

            Squadra? squadra = null;

            if (operazioneDto.StatoOperazione != "Svincolato")
            {
                // Recupera la squadra associata all'operazione
                squadra = await _dbContext.Squadre.FirstOrDefaultAsync(s => s.ID_Squadra == operazioneDto.ID_Squadra);
                if (squadra == null)
                {
                    throw new KeyNotFoundException($"Squadra con ID {operazioneDto.ID_Squadra} non trovata.");
                }

                // Recupera l'asta per i limiti di ruolo
                var asta = await _dbContext.Aste.FirstOrDefaultAsync(a => a.ID_Asta == squadra.ID_Asta);
                if (asta == null)
                {
                    throw new KeyNotFoundException($"Asta con ID {squadra.ID_Asta} non trovata.");
                }

                // Verifica il numero di giocatori per ruolo nella squadra
                var numeroGiocatoriPerRuolo = await _dbContext.Operazioni
                    .Where(o => o.ID_Squadra == operazioneDto.ID_Squadra
                                && o.Giocatore.RuoloClassic == giocatore.RuoloClassic
                                && o.StatoOperazione == "Assegnato")
                    .CountAsync();

                // Verifica i limiti per ruolo
                var limiteRaggiunto = giocatore.RuoloClassic switch
                {
                    "Portiere" => numeroGiocatoriPerRuolo >= asta.MaxPortieri,
                    "Difensore" => numeroGiocatoriPerRuolo >= asta.MaxDifensori,
                    "Centrocampista" => numeroGiocatoriPerRuolo >= asta.MaxCentrocampisti,
                    "Attaccante" => numeroGiocatoriPerRuolo >= asta.MaxAttaccanti,
                    _ => false
                };

                if (limiteRaggiunto)
                {
                    throw new InvalidOperationException($"Hai già raggiunto il numero massimo di {giocatore.RuoloClassic}.");
                }

                // Gestione dei crediti totali
                var creditiTotaliPrima = squadra.CreditiTotali;
                var creditiRichiesti = operazioneDto.CreditiSpesi ?? 0;

                var creditiTotaliAggiornati = creditiTotaliPrima - creditiRichiesti;
                if (creditiTotaliAggiornati < 0)
                {
                    throw new InvalidOperationException("Crediti insufficienti per completare l'operazione.");
                }

                // Aggiorna i crediti totali e spesi
                squadra.CreditiTotali = creditiTotaliAggiornati;
                squadra.CreditiSpesi += creditiRichiesti;

                _dbContext.Entry(squadra).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
            }

            // Crea una nuova operazione
            var operazione = new Operazione
            {
                ID_Giocatore = operazioneDto.ID_Giocatore,
                ID_Squadra = operazioneDto.StatoOperazione == "Svincolato" ? null : operazioneDto.ID_Squadra,
                CreditiSpesi = operazioneDto.CreditiSpesi,
                StatoOperazione = operazioneDto.StatoOperazione == "Svincolato" ? "Svincolato" : "Assegnato",
                DataOperazione = operazioneDto.DataOperazione ?? DateTime.Now,
                ID_Asta = operazioneDto.ID_Asta
            };

            _dbContext.Operazioni.Add(operazione);
            await _dbContext.SaveChangesAsync();

            // Restituisci il DTO con le informazioni dettagliate
            return new OperazioneDettagliataDto
            {
                ID_Operazione = operazione.ID_Operazione,
                ID_Giocatore = giocatore.ID_Giocatore,
                NomeGiocatore = giocatore.Nome,
                CognomeGiocatore = giocatore.Cognome,
                RuoloClassic = giocatore.RuoloClassic,
                ID_Squadra = operazioneDto.StatoOperazione == "Svincolato" ? null : squadra?.ID_Squadra,
                NomeSquadra = operazioneDto.StatoOperazione == "Svincolato" ? "Svincolato" : squadra?.Nome,
                CreditiSpesi = operazione.CreditiSpesi,
                StatoOperazione = operazione.StatoOperazione,
                DataOperazione = operazioneDto.DataOperazione ?? DateTime.Now,
                ID_Asta = operazione.ID_Asta
            };
        }




        //UPDATE
        public async Task UpdateOperazione(int ID_Operazione, OperazioneDto operazioneDto)
        {
            // Trova l'operazione esistente
            var operazioneEsistente = await _dbContext.Operazioni.FirstOrDefaultAsync(o => o.ID_Operazione == ID_Operazione);

            if (operazioneEsistente == null)
            {
                throw new KeyNotFoundException($"Operazione con ID {ID_Operazione} non trovata.");
            }

            // Trova la squadra associata all'operazione
            var squadra = await _dbContext.Squadre.FirstOrDefaultAsync(s => s.ID_Squadra == operazioneEsistente.ID_Squadra);

            if (squadra == null)
            {
                throw new KeyNotFoundException($"Squadra con ID {operazioneEsistente.ID_Squadra} non trovata.");
            }

            // Calcola i crediti disponibili
            var creditiDisponibili = squadra.CreditiTotali - squadra.CreditiSpesi + (operazioneEsistente.CreditiSpesi ?? 0);

            // Verifica se i nuovi crediti spesi sono maggiori dei crediti disponibili
            if (!operazioneDto.CreditiSpesi.HasValue || operazioneDto.CreditiSpesi > creditiDisponibili)
            {
                throw new InvalidOperationException("Crediti insufficienti per completare l'operazione.");
            }

            // Aggiorna i crediti spesi della squadra (rimuovi quelli precedenti e aggiungi quelli nuovi)
            squadra.CreditiSpesi = squadra.CreditiSpesi - (operazioneEsistente.CreditiSpesi ?? 0) + (operazioneDto.CreditiSpesi ?? 0);

            // Aggiorna i campi dell'operazione
            operazioneEsistente.ID_Giocatore = operazioneDto.ID_Giocatore;
            operazioneEsistente.ID_Squadra = operazioneDto.ID_Squadra;
            operazioneEsistente.CreditiSpesi = operazioneDto.CreditiSpesi ?? 0; // Gestione nullable
            operazioneEsistente.StatoOperazione = operazioneDto.StatoOperazione;
            operazioneEsistente.DataOperazione = operazioneDto.DataOperazione ?? DateTime.Now; // Gestione nullable

            // Salva i cambiamenti
            await _dbContext.SaveChangesAsync();
        }




        //ELIMINAZIONE OPERAZIONE 
        public async Task DeleteOperazione(int idOperazione, int idSquadra, int creditiSpesi)
        {
            var operazione = await _dbContext.Operazioni.FirstOrDefaultAsync(o => o.ID_Operazione == idOperazione);

            if (operazione == null)
            {
                throw new KeyNotFoundException("Operazione non trovata.");
            }

            var squadra = await _dbContext.Squadre.FirstOrDefaultAsync(s => s.ID_Squadra == idSquadra);

            if (squadra == null)
            {
                throw new KeyNotFoundException("Squadra non trovata.");
            }

            // Aumenta i crediti totali della squadra
            squadra.CreditiTotali += creditiSpesi;

            // Riduce i crediti spesi della squadra
            squadra.CreditiSpesi -= creditiSpesi;

            // Rimuovi l'operazione dal database
            _dbContext.Operazioni.Remove(operazione);

            // Aggiorna lo stato dell'entità per far sì che EF tracci la modifica
            _dbContext.Entry(squadra).State = EntityState.Modified;

            // Salva i cambiamenti
            await _dbContext.SaveChangesAsync();
        }





        //TROVA UNA SINGOLA OPERAZIONE 
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

        public async Task<IEnumerable<OperazioneDto>> GetOperazioniByAstaId(int ID_Asta)
        {
            var query = _dbContext.Operazioni
                .Include(o => o.Giocatore)
                .Include(o => o.Squadra)
                .Where(o => o.Squadra.ID_Asta == ID_Asta) // Filtro per ID_Asta
                .AsQueryable();

            // Ordina in base al ruolo Classic (con stringhe uniformate a maiuscolo)
            query = query.OrderBy(o => o.Giocatore.RuoloClassic.ToUpper() == "PORTIERE" ? 1 :
                                       o.Giocatore.RuoloClassic.ToUpper() == "DIFENSORE" ? 2 :
                                       o.Giocatore.RuoloClassic.ToUpper() == "CENTROCAMPISTA" ? 3 :
                                       o.Giocatore.RuoloClassic.ToUpper() == "ATTACCANTE" ? 4 :
                                       5); // Ordina altri ruoli come ultimo gruppo

            var operazioni = await query.ToListAsync();

            // Trasforma le operazioni in DTO
            return operazioni.Select(o => new OperazioneDto
            {
                ID_Operazione = o.ID_Operazione,
                ID_Giocatore = o.ID_Giocatore,
                ID_Squadra = o.ID_Squadra,
                CreditiSpesi = o.CreditiSpesi,
                StatoOperazione = o.StatoOperazione,
                DataOperazione = o.DataOperazione,
                ID_Asta = o.Squadra.ID_Asta // Associa correttamente l'ID_Asta
            }).ToList();
        }




        //TROVA TUTTE LE OPERAZIONI 
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
                DataOperazione = o.DataOperazione,
                ID_Asta = o.ID_Asta  // Aggiungi l'ID_Asta qui
            }).ToList();
        }




        public async Task CambiaStatoGiocatoreAsync(OperazioneSvincoloDto dto)
        {
            // Trova l'operazione esistente per il giocatore e l'asta
            var operazione = await _dbContext.Operazioni
                .FirstOrDefaultAsync(o => o.ID_Giocatore == dto.ID_Giocatore && o.ID_Asta == dto.ID_Asta);

            if (operazione == null)
            {
                // Se l'operazione non esiste, crea una nuova operazione
                operazione = new Operazione
                {
                    ID_Giocatore = dto.ID_Giocatore,
                    ID_Asta = dto.ID_Asta,
                    StatoOperazione = dto.StatoOperazione,
                    DataOperazione = DateTime.Now,
                    ID_Squadra = null, // Imposta a null se non è assegnato a una squadra
                    CreditiSpesi = 0 // Nessun costo per lo svincolo o ripristino
                };
                _dbContext.Operazioni.Add(operazione);
            }
            else
            {
                // Aggiorna l'operazione esistente
                operazione.StatoOperazione = dto.StatoOperazione;
                operazione.DataOperazione = DateTime.Now;

                // Se ID_Squadra è null e il giocatore viene ripristinato, lascia ID_Squadra a null
                if (!operazione.ID_Squadra.HasValue && dto.StatoOperazione == "Disponibile")
                {
                    operazione.ID_Squadra = null;
                }

                // Traccia le modifiche all'operazione
                _dbContext.Entry(operazione).State = EntityState.Modified;
            }

            // Salva i cambiamenti
            await _dbContext.SaveChangesAsync();
        }
    }
}
