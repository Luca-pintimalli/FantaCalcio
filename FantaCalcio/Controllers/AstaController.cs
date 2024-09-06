using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using FantaCalcio.DTOs;
using FantaCalcio.Services.Interface;

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
                return NotFound();
            }
            return Ok(asta);
        }

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
                // Passa l'ID utente al servizio
                await _astaService.AddAsta(userId, astaDto);
                return CreatedAtAction(nameof(GetById), new { id = astaDto.ID_TipoAsta }, astaDto);
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
            catch (Exception ex)
            {
                return NotFound($"Errore durante la cancellazione dell'asta: {ex.Message}");
            }
        }
    }
}

