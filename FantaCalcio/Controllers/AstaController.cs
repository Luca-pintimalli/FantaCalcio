using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using FantaCalcio.Models;
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

        // GET: api/asta
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Asta>>> GetAll()
        {
            var aste = await _astaService.GetAll();
            return Ok(aste);
        }

        // GET: api/asta/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Asta>> GetById(int id)
        {
            var asta = await _astaService.GetAstaById(id);
            if (asta == null)
            {
                return NotFound();
            }
            return Ok(asta);
        }

        // POST: api/asta
        [HttpPost]
        public async Task<ActionResult<Asta>> Create([FromBody] Asta asta)
        {
            if (asta == null)
            {
                return BadRequest("L'asta non può essere null.");
            }

            try
            {
                await _astaService.AddAsta(asta);
                return CreatedAtAction(nameof(GetById), new { id = asta.ID_Asta }, asta);
            }
            catch (Exception ex)
            {
                return BadRequest($"Errore durante la creazione dell'asta: {ex.Message}");
            }
        }

        // PUT: api/asta/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Asta asta)
        {
            if (asta == null || id != asta.ID_Asta)
            {
                return BadRequest("ID non corrispondente o asta null.");
            }

            try
            {
                await _astaService.UpdateAsta(id, asta);
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

        // DELETE: api/asta/5
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
