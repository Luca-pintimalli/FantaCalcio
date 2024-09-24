using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using FantaCalcio.DTOs;
using FantaCalcio.Services.Interface;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using FantaCalcio.Models;
using Microsoft.EntityFrameworkCore;

namespace FantaCalcio.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AstaController : ControllerBase
    {
        private readonly IAstaService _astaService;

        public AstaController(IAstaService astaService)
        {
            _astaService = astaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AstaDto>>> GetAll()
        {
            var aste = await _astaService.GetAll();
            return Ok(aste);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AstaDto>> GetById(int id)
        {
            var asta = await _astaService.GetAstaById(id);
            if (asta == null)
            {
                return NotFound($"Asta con ID {id} non trovata.");
            }
            return Ok(asta);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<AstaDto>> Create([FromBody] AstaCreateUpdateDto astaDto)
        {
            if (astaDto == null)
            {
                return BadRequest("L'asta non può essere null.");
            }

            // Ottieni l'ID dell'utente autenticato dal token JWT
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized("Utente non autenticato.");
            }

            // Converti l'ID utente da stringa a intero
            if (!int.TryParse(userIdClaim, out int userId))
            {
                return BadRequest("ID utente non valido.");
            }

            try
            {
                // Passa l'ID utente dal token JWT e il DTO per creare l'asta
                var astaCreata = await _astaService.AddAsta(userId, astaDto);

                // Usa l'ID dell'asta appena creata per il ritorno
                return CreatedAtAction(nameof(GetById), new { id = astaCreata.ID_Asta }, astaCreata);
            }
            catch (Exception ex)
            {
                return BadRequest($"Errore durante la creazione dell'asta: {ex.Message}");
            }
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] AstaCreateUpdateDto astaDto)
        {
            if (astaDto == null)
            {
                return BadRequest("L'asta non può essere null.");
            }

            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized("Utente non autenticato.");
            }

            try
            {
                // Passa l'ID utente e il DTO al servizio
                await _astaService.UpdateAsta(id, userId, astaDto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest($"Errore durante l'aggiornamento dell'asta: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                // Chiama il servizio per eliminare l'asta
                await _astaService.DeleteAsta(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound($"Asta con ID {id} non trovata: {ex.Message}");
            }
            catch (Exception ex)
            {
                return BadRequest($"Errore durante la cancellazione dell'asta: {ex.Message}");
            }
        }



        // API per ottenere il prossimo giocatore in base all'asta corrente
        [HttpGet("prossimogiocatore/{idAsta}")]
        public async Task<ActionResult<GiocatoreDto>> ProssimoGiocatore(int idAsta)
        {
            try
            {
                // Passa l'ID dell'asta corrente per selezionare il giocatore random
                var giocatore = await _astaService.SelezionaGiocatoreRandomAsync(idAsta);

                // Popola tutti i campi nel GiocatoreDto, inclusi i ruoli Mantra, verificando che non sia null
                var giocatoreDto = new GiocatoreDto
                {
                    ID_Giocatore = giocatore.ID_Giocatore,
                    Nome = giocatore.Nome,
                    Cognome = giocatore.Cognome,
                    Foto = giocatore.Foto,
                    SquadraAttuale = giocatore.SquadraAttuale,
                    GoalFatti = giocatore.GoalFatti,
                    GoalSubiti = giocatore.GoalSubiti,
                    Assist = giocatore.Assist,
                    PartiteGiocate = giocatore.PartiteGiocate,
                    RuoloClassic = giocatore.RuoloClassic,
                    RuoliMantra = giocatore.RuoliMantra != null
                        ? giocatore.RuoliMantra.Select(rm => new RuoloMantraDTO
                        {
                            NomeRuolo = rm.Ruolo?.NomeRuolo  // Usa l'operatore null-safe in caso di ruoli non inizializzati
                        }).ToList()
                        : new List<RuoloMantraDTO>()  // Se la lista RuoliMantra è null, ritorna una lista vuota
                };

                return Ok(giocatoreDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }





        [HttpGet("{idAsta}/cercagiocatore")]
        public async Task<ActionResult<Giocatore>> CercaGiocatore(int idAsta, [FromQuery] string? nome, [FromQuery] string? cognome)
        {
            try
            {
                // Passa l'ID dell'asta per cercare il giocatore disponibile
                var giocatore = await _astaService.CercaGiocatoreAsync(idAsta, nome, cognome);
                return Ok(giocatore); // Restituiamo l'intero oggetto Giocatore
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }

}
