using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using FantaCalcio.DTOs;
using FantaCalcio.Services.Interface;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

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


        // API per ottenere il prossimo giocatore in base alla squadra
        [HttpGet("prossimogiocatore")]
        public async Task<ActionResult<GiocatoreDto>> ProssimoGiocatore()
        {
            try
            {
                var giocatore = await _astaService.SelezionaGiocatoreRandomAsync();
                var giocatoreDto = new GiocatoreDto
                {
                    ID_Giocatore = giocatore.ID_Giocatore,
                    Nome = giocatore.Nome,
                    Cognome = giocatore.Cognome
                };
                return Ok(giocatoreDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // API per cercare un giocatore per cognome (asta a chiamata)
        [HttpGet("{squadraId}/cercagiocatore/{cognome}")]
        public async Task<ActionResult<GiocatoreDto>> CercaGiocatore(int squadraId, string cognome)
        {
            try
            {
                var giocatore = await _astaService.CercaGiocatorePerCognomeAsync(squadraId, cognome);
                var giocatoreDto = new GiocatoreDto
                {
                    ID_Giocatore = giocatore.ID_Giocatore,
                    Nome = giocatore.Nome,
                    Cognome = giocatore.Cognome
                };
                return Ok(giocatoreDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
