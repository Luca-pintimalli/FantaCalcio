using FantaCalcio.DTOs;
using FantaCalcio.Models;
using FantaCalcio.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace FantaCalcio.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TipoAstaController : ControllerBase
    {
        private readonly ITipoAstaService _tipoAstaService;

        public TipoAstaController(ITipoAstaService tipoAstaService)
        {
            _tipoAstaService = tipoAstaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoAstaDto>>> GetAllTipoAsteAsync()
        {
            var tipiAsta = await _tipoAstaService.GetAll();
            var tipiAstaDto = tipiAsta.Select(t => new TipoAstaDto
            {
                ID_TipoAsta = t.ID_TipoAsta,
                NomeTipoAsta = t.NomeTipoAsta
            });

            return Ok(tipiAstaDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TipoAstaDto>> GetTipoAstaById(int id)
        {
            var tipoAsta = await _tipoAstaService.GetTipoAstaById(id);

            if (tipoAsta == null)
            {
                return NotFound();
            }

            var tipoAstaDto = new TipoAstaDto
            {
                ID_TipoAsta = tipoAsta.ID_TipoAsta,
                NomeTipoAsta = tipoAsta.NomeTipoAsta
            };

            return Ok(tipoAstaDto);
        }

        [HttpPost]
        public async Task<ActionResult<TipoAstaDto>> AddTipoAstaAsync([FromBody] TipoAstaDto tipoAstaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tipoAsta = new TipoAsta
            {
                NomeTipoAsta = tipoAstaDto.NomeTipoAsta
            };

            await _tipoAstaService.AddTipoAstaAsync(tipoAsta);

            tipoAstaDto.ID_TipoAsta = tipoAsta.ID_TipoAsta;

            return CreatedAtAction(nameof(GetTipoAstaById), new { id = tipoAsta.ID_TipoAsta }, tipoAstaDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTipoAstaAsync(int id, [FromBody] TipoAstaDto tipoAstaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tipoAstaAggiornato = new TipoAsta
            {
                NomeTipoAsta = tipoAstaDto.NomeTipoAsta
            };

            try
            {
                await _tipoAstaService.UpdateTipoAstaAsync(id, tipoAstaAggiornato);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"TipoAsta con ID {id} non trovato");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTipoAstaAsync(int id)
        {
            try
            {
                await _tipoAstaService.DeleteTipoAstaAsync(id);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

            return NoContent();
        }
    }
}
