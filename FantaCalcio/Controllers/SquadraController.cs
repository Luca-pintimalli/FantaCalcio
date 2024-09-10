using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using FantaCalcio.DTOs;
using FantaCalcio.Services.Interface;

namespace FantaCalcio.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SquadraController : ControllerBase
    {
        private readonly ISquadraService _squadraService;

        public SquadraController(ISquadraService squadraService)
        {
            _squadraService = squadraService;
        }

        // POST: api/squadra
        [HttpPost]
        public async Task<IActionResult> CreateSquadra([FromBody] SquadraCreateDto squadraDto)
        {
            if (squadraDto == null)
            {
                return BadRequest("La squadra non può essere null.");
            }

            try
            {
                await _squadraService.CreateSquadra(squadraDto.ID_Asta, squadraDto);
                return Ok("Squadra creata con successo.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Errore durante la creazione della squadra: {ex.Message}");
            }
        }


        // PUT: api/squadra/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSquadra(int id, [FromBody] SquadraDto squadraDto)
        {
            if (squadraDto == null)
            {
                return BadRequest("La squadra non può essere null.");
            }

            try
            {
                await _squadraService.UpdateSquadra(id, squadraDto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Errore durante l'aggiornamento della squadra: {ex.Message}");
            }
        }

        // DELETE: api/squadra/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSquadra(int id)
        {
            try
            {
                await _squadraService.DeleteSquadra(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Errore durante la cancellazione della squadra: {ex.Message}");
            }
        }

        // GET: api/squadra/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SquadraDto>> GetSquadraById(int id)
        {
            var squadra = await _squadraService.GetSquadraById(id);
            if (squadra == null)
            {
                return NotFound($"Squadra con ID {id} non trovata.");
            }
            return Ok(squadra);
        }

        // GET: api/squadra
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SquadraDto>>> GetAllSquadre()
        {
            var squadre = await _squadraService.GetAll();
            return Ok(squadre);
        }
    }
}
