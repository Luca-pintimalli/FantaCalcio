using System;
namespace FantaCalcio.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using global::FantaCalcio.DTOs;
    using global::FantaCalcio.Services.Interface;
    using global::FantaCalcio.Services;

    namespace FantaCalcio.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        public class OperazioneController : ControllerBase
        {
            private readonly IOperazioneService _operazioneService;
            private readonly ISquadraService _squadraService;

            public OperazioneController(ISquadraService squadraService, IOperazioneService operazioneService)
            {
                _squadraService = squadraService;
                _operazioneService = operazioneService;
            }
            // POST: api/operazione
            [HttpPost]
            public async Task<IActionResult> CreaOperazione([FromBody] OperazioneDto operazioneDto)
            {
                try
                {
                    await _operazioneService.CreaOperazione(operazioneDto);
                    return Ok(new { message = "Operazione creata con successo." });
                }
                catch (KeyNotFoundException ex)
                {
                    return NotFound(new { message = ex.Message });
                }
                catch (InvalidOperationException ex)
                {
                    return BadRequest(new { message = ex.Message });
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { message = "Errore durante la creazione dell'operazione: " + ex.Message });
                }
            }

            // PUT: api/operazione/5
            [HttpPut("{id}")]
            public async Task<IActionResult> UpdateOperazione(int id, [FromBody] OperazioneDto operazioneDto)
            {
                if (operazioneDto == null)
                {
                    return BadRequest("L'operazione non può essere null.");
                }

                try
                {
                    // Prova ad aggiornare l'operazione
                    await _operazioneService.UpdateOperazione(id, operazioneDto);
                    return NoContent();
                }
                catch (KeyNotFoundException ex)
                {
                    return NotFound(ex.Message);
                }
                catch (InvalidOperationException ex) // Eccezione per crediti insufficienti o altre logiche
                {
                    return BadRequest(ex.Message);
                }
                catch (Exception ex)
                {
                    return BadRequest($"Errore durante l'aggiornamento dell'operazione: {ex.Message}");
                }
            }

            // DELETE: api/operazione/5
            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteOperazione(int id)
            {
                try
                {
                    await _operazioneService.DeleteOperazione(id);
                    return NoContent();
                }
                catch (KeyNotFoundException ex)
                {
                    return NotFound(ex.Message);
                }
                catch (Exception ex)
                {
                    return BadRequest($"Errore durante la cancellazione dell'operazione: {ex.Message}");
                }
            }

            // GET: api/operazione/5
            [HttpGet("{id}")]
            public async Task<ActionResult<OperazioneDto>> GetOperazioneById(int id)
            {
                var operazione = await _operazioneService.GetOperazioneById(id);
                if (operazione == null)
                {
                    return NotFound($"Operazione con ID {id} non trovata.");
                }
                return Ok(operazione);
            }

            // GET: api/operazione
            [HttpGet]
            public async Task<ActionResult<IEnumerable<OperazioneDto>>> GetAllOperazioni()
            {
                var operazioni = await _operazioneService.GetAll();
                return Ok(operazioni);
            }
        }
    }
}
