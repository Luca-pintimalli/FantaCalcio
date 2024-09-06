using FantaCalcio.Models;
using FantaCalcio.Services.Interface;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class RuoliController : ControllerBase
{
    private readonly IRuoloService _ruoloService;

    public RuoliController(IRuoloService ruoloService)
    {
        _ruoloService = ruoloService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RuoloDto>>> GetAllRuoli()
    {
        var ruoli = await _ruoloService.GetAllAsync();
        var ruoliDto = ruoli.Select(r => MapToRuoloDto(r)).ToList();
        return Ok(ruoliDto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RuoloDto>> GetRuoloById(int id)
    {
        var ruolo = await _ruoloService.GetRuoloByIdAsync(id);

        if (ruolo == null)
        {
            return NotFound();
        }

        var ruoloDto = MapToRuoloDto(ruolo);
        return Ok(ruoloDto);
    }

    [HttpPost]
    public async Task<ActionResult> AddRuolo(RuoloDto ruoloDto)
    {
        var ruolo = MapToRuolo(ruoloDto);
        await _ruoloService.AddRuoloAsync(ruolo);

        return CreatedAtAction(nameof(GetRuoloById), new { id = ruolo.ID_Ruolo }, ruoloDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRuolo(int id, RuoloDto ruoloDto)
    {
        if (id != ruoloDto.ID_Ruolo)
        {
            return BadRequest();
        }

        var ruolo = MapToRuolo(ruoloDto);

        try
        {
            await _ruoloService.UpdateRuoloAsync(id, ruolo);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRuolo(int id)
    {
        try
        {
            await _ruoloService.DeleteRuoloAsync(id);
        }
        catch (Exception)
        {
            return NotFound();
        }

        return NoContent();
    }

    // Mappatura da Ruolo a RuoloDto
    private RuoloDto MapToRuoloDto(Ruolo ruolo)
    {
        return new RuoloDto
        {
            ID_Ruolo = ruolo.ID_Ruolo,
            NomeRuolo = ruolo.NomeRuolo,
            ID_Modalita = ruolo.ID_Modalita,
        };
    }

    // Mappatura da RuoloDto a Ruolo 
    private Ruolo MapToRuolo(RuoloDto ruoloDto)
    {
        return new Ruolo
        {
            ID_Ruolo = ruoloDto.ID_Ruolo,
            NomeRuolo = ruoloDto.NomeRuolo,
            ID_Modalita = ruoloDto.ID_Modalita 
        };
    }
}
