using FantaCalcio.DTOs;
using FantaCalcio.Models;
using FantaCalcio.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FantaCalcio.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RuoloMantraController : ControllerBase
    {
        private readonly IRuoloMantraService _ruoloMantraService;

        public RuoloMantraController(IRuoloMantraService ruoloMantraService)
        {
            _ruoloMantraService = ruoloMantraService;
        }

        [HttpPost("aggiungi")]
        public async Task<IActionResult> AddRuoloMantra([FromBody] RuoloMantraDTO ruoloMantraDTO)
        {
            try
            {
                if (ruoloMantraDTO == null)
                {
                    return BadRequest("Dati del ruolo non validi.");
                }

                await _ruoloMantraService.AddRuoloMantra(ruoloMantraDTO.ID_Giocatore, ruoloMantraDTO.ID_Ruolo);
                return Ok(new { message = "Ruolo Mantra assegnato con successo." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpDelete("rimuovi")]
        public async Task<IActionResult> RemoveRuoloMantra([FromBody] RuoloMantraDTO ruoloMantraDTO)
        {
            try
            {
                if (ruoloMantraDTO == null)
                {
                    return BadRequest("Dati del ruolo non validi.");
                }

                await _ruoloMantraService.RemoveRuoloMantra(ruoloMantraDTO.ID_Giocatore, ruoloMantraDTO.ID_Ruolo);
                return Ok(new { message = "Ruolo Mantra rimosso con successo." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RuoloMantraDTO>>> GetAllRuoliMantra()
        {
            try
            {
                var ruoliMantra = await _ruoloMantraService.GetAllRuoliMantra();

                if (ruoliMantra == null || !ruoliMantra.Any())
                {
                    return NotFound(new { message = "Nessun ruolo Mantra trovato." });
                }

                return Ok(ruoliMantra);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("giocatore/{idGiocatore}")]
        public async Task<ActionResult<IEnumerable<RuoloMantraDTO>>> GetRuoliMantraByGiocatoreId(int idGiocatore)
        {
            try
            {
                var ruoliMantra = await _ruoloMantraService.GetRuoliMantraByGiocatoreId(idGiocatore);

                if (ruoliMantra == null || !ruoliMantra.Any())
                {
                    return NotFound(new { message = $"Nessun ruolo Mantra trovato per il giocatore con ID {idGiocatore}." });
                }

                return Ok(ruoliMantra);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
