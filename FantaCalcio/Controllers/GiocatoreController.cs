using FantaCalcio.DTOs;
using FantaCalcio.Services;
using FantaCalcio.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    public async Task<IActionResult> AddGiocatore([FromForm] GiocatoreCreateUpdateDto giocatoreDto, IFormFile file = null)
    {
        if (giocatoreDto == null)
        {
            return BadRequest("Dati del giocatore non validi.");
        }

        string filePath = null;  // Predefinito a null se nessun file è caricato

        if (file != null && file.Length > 0)
        {
            // Se c'è un file, salviamo il percorso
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
            var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
            filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            filePath = $"/uploads/{uniqueFileName}"; // Imposta il percorso dell'immagine
        }

        try
        {
            // Passa filePath al servizio, sarà null se non c'è immagine
            await _giocatoreService.AddGiocatore(giocatoreDto, filePath);
            return Ok(new { message = "Giocatore creato con successo." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateGiocatore(int id, [FromForm] GiocatoreUpdateModel model)
    {
        if (model == null)
        {
            return BadRequest(new { message = "Dati del giocatore non validi." });
        }

        // Crea il DTO a partire dal modello fornito
        var giocatoreDto = new GiocatoreCreateUpdateDto
        {
            Nome = model.Nome,
            Cognome = model.Cognome,
            SquadraAttuale = model.SquadraAttuale,
            GoalFatti = model.GoalFatti,
            GoalSubiti = model.GoalSubiti,
            Assist = model.Assist,
            PartiteGiocate = model.PartiteGiocate,
            RuoloClassic = model.RuoloClassic
        };

        try
        {
            // Chiama il servizio per aggiornare il giocatore, passando il file solo se esiste
            await _giocatoreService.UpdateGiocatore(id, giocatoreDto, model.File);

            return Ok(new { message = "Giocatore aggiornato con successo." });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Errore durante l'aggiornamento del giocatore: {ex.Message}" });
        }
    }









    [HttpGet]
    public async Task<ActionResult<IEnumerable<GiocatoreDto>>> GetAllGiocatori([FromQuery] string ruolo = null, [FromQuery] string search = null)
    {
        try
        {
            var giocatori = await _giocatoreService.GetAll(ruolo, search);

            if (giocatori == null || !giocatori.Any())
            {
                return NotFound(new { message = "Nessun giocatore trovato." });
            }

            return Ok(giocatori);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Errore durante il recupero dei giocatori: {ex.Message}");
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

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGiocatore(int id)
    {
        try
        {
            await _giocatoreService.DeleteGiocatore(id);
            return Ok(new { message = "Giocatore eliminato con successo." });
        }
        catch (DbUpdateException dbEx)
        {
            var innerException = dbEx.InnerException != null ? dbEx.InnerException.Message : dbEx.Message;
            return StatusCode(500, new { message = $"Errore durante l'eliminazione del giocatore: {innerException}" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }
}
