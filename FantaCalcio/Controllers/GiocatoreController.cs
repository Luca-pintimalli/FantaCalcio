using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FantaCalcio.Models;
using FantaCalcio.Services.Interface;

namespace FantaCalcio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GiocatoreController : ControllerBase
    {
        private readonly IGiocatoreService _giocatoreService;

        public GiocatoreController(IGiocatoreService giocatoreService)
        {
            _giocatoreService = giocatoreService;
        }

        // GET: api/giocatore
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Giocatore>>> GetAll()
        {
            var giocatori = await _giocatoreService.GetAll();
            return Ok(giocatori);
        }

        // GET: api/giocatore/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Giocatore>> GetGiocatoreById(int id)
        {
            var giocatore = await _giocatoreService.GetGiocatoreById(id);
            if (giocatore == null)
            {
                return NotFound();
            }
            return Ok(giocatore);
        }

        // POST: api/giocatore
        [HttpPost]
        public async Task<ActionResult<Giocatore>> PostGiocatore(Giocatore giocatore)
        {
            if (giocatore == null)
            {
                return BadRequest("Giocatore is null.");
            }

            // Create the giocatore
            await _giocatoreService.AddGiocatore(giocatore);

            // Return the newly created giocatore
            return CreatedAtAction(nameof(GetGiocatoreById), new { id = giocatore.ID_Giocatore }, giocatore);
        }

        // PUT: api/giocatore/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGiocatore(int id, Giocatore giocatore)
        {
            if (id != giocatore.ID_Giocatore)
            {
                return BadRequest();
            }

            try
            {
                await _giocatoreService.UpdateGiocatore(id, giocatore);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await GiocatoreExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/giocatore/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGiocatore(int id)
        {
            var giocatore = await _giocatoreService.GetGiocatoreById(id);
            if (giocatore == null)
            {
                return NotFound();
            }

            await _giocatoreService.DeleteGiocatore(id);

            return NoContent();
        }

        private async Task<bool> GiocatoreExists(int id)
        {
            return await _giocatoreService.GetGiocatoreById(id) != null;
        }
    }
}
