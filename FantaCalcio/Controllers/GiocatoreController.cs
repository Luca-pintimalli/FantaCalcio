using FantaCalcio.DTOs;
using FantaCalcio.Services.Interface;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class GiocatoriController : ControllerBase
{
    private readonly IGiocatoreService _giocatoreService;

    public GiocatoriController(IGiocatoreService giocatoreService)
    {
        _giocatoreService = giocatoreService;
    }

    [HttpPost]
    public async Task<IActionResult> AddGiocatore([FromBody] GiocatoreCreateUpdateDto giocatoreDto)
    {
        if (giocatoreDto == null)
        {
            return BadRequest("Dati del giocatore non validi.");
        }

        try
        {
            await _giocatoreService.AddGiocatore(giocatoreDto);
            return Ok(new { message = "Giocatore creato con successo." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateGiocatore(int id, [FromBody] GiocatoreCreateUpdateDto giocatoreDto)
    {
        if (giocatoreDto == null)
        {
            return BadRequest("Dati del giocatore non validi.");
        }

        try
        {
            await _giocatoreService.UpdateGiocatore(id, giocatoreDto);
            return Ok(new { message = "Giocatore aggiornato con successo." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GiocatoreDto>>> GetAllGiocatori()
    {
        try
        {
            var giocatori = await _giocatoreService.GetAll();

            if (giocatori == null || !giocatori.Any())
            {
                return NotFound(new { message = "Nessun giocatore trovato." });
            }

            return Ok(giocatori);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GiocatoreDto>> GetGiocatoreById(int id)
    {
        try
        {
            var giocatore = await _giocatoreService.GetGiocatoreById(id);

            if (giocatore == null)
            {
                return NotFound(new { message = $"Giocatore con ID {id} non trovato." });
            }

            return Ok(giocatore);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }
}