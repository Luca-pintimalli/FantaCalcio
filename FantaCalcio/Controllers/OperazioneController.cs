using System;
namespace FantaCalcio.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using global::FantaCalcio.DTOs;
    using global::FantaCalcio.Services.Interface;
    using global::FantaCalcio.Services;
    using Microsoft.EntityFrameworkCore;

    namespace FantaCalcio.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        public class OperazioneController : ControllerBase
        {
            private readonly IOperazioneService _operazioneService;
            private readonly ISquadraService _squadraService;
            private readonly ILogger<OperazioneController> _logger;


            public OperazioneController(ISquadraService squadraService, IOperazioneService operazioneService, ILogger<OperazioneController> logger)
            {
                _squadraService = squadraService;
                _operazioneService = operazioneService;
                _logger = logger;
            }
            // POST: api/operazione
            [HttpPost]
            public async Task<IActionResult> CreaOperazione([FromBody] OperazioneDto operazioneDto)
            {
                try
                {
                    // Assicurati che operazioneDto includa ID_Asta
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

            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteOperazione(int id, [FromQuery] int idSquadra, [FromQuery] int creditiSpesi)
            {
                try
                {
                    await _operazioneService.DeleteOperazione(id, idSquadra, creditiSpesi);
                    return NoContent();
                }
                catch (KeyNotFoundException ex)
                {
                    return NotFound(ex.Message);
                }
                catch (Exception ex)
                {
                    return BadRequest($"Errore durante l'eliminazione dell'operazione: {ex.Message}");
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

            [HttpGet("asta/{id}")]
            public async Task<IActionResult> GetOperazioniByAsta(int id)
            {
                var operazioni = await _operazioneService.GetOperazioniByAstaId(id);
                if (!operazioni.Any())
                {
                    return NotFound();
                }

                return Ok(operazioni);
            }




            // GET: api/operazione
            [HttpGet]
            public async Task<ActionResult<IEnumerable<OperazioneDto>>> GetAllOperazioni()
            {
                var operazioni = await _operazioneService.GetAll();
                return Ok(operazioni);
            }

            [HttpPost("svincola/{idGiocatore}")]
            public async Task<IActionResult> SvincolaGiocatore(int idGiocatore, [FromQuery] int idAsta)
            {
                try
                {
                    // Creiamo un DTO da passare al metodo
                    var dto = new OperazioneSvincoloDto
                    {
                        ID_Giocatore = idGiocatore,
                        ID_Asta = idAsta,
                        StatoOperazione = "Svincolato" // Stato per svincolare
                    };

                    // Chiama il servizio con il DTO
                    await _operazioneService.CambiaStatoGiocatoreAsync(dto);
                    return Ok(new { message = "Giocatore svincolato con successo." });
                }
                catch (KeyNotFoundException ex)
                {
                    return NotFound(new { message = ex.Message });
                }
                catch (Exception ex)
                {
                    return BadRequest($"Errore durante lo svincolo del giocatore: {ex.Message}");
                }
            }


            [HttpPut("ripristina/{idGiocatore}")]
            public async Task<IActionResult> RipristinaGiocatore(int idGiocatore, [FromQuery] int idAsta)
            {
                try
                {
                    // Crea un oggetto OperazioneSvincoloDto e passa i valori necessari
                    var dto = new OperazioneSvincoloDto
                    {
                        ID_Giocatore = idGiocatore,
                        ID_Asta = idAsta,
                        StatoOperazione = "Disponibile" // Stato per ripristinare il giocatore
                    };

                    // Passa il DTO al metodo CambiaStatoGiocatoreAsync
                    await _operazioneService.CambiaStatoGiocatoreAsync(dto);

                    // Restituisce lo stato HTTP 200 (OK)
                    return Ok(new { message = "Giocatore ripristinato con successo." });
                }
                catch (KeyNotFoundException ex)
                {
                    // Usa il logger per tracciare l'errore
                    _logger.LogWarning("Giocatore o operazione non trovata: {Message}", ex.Message);

                    // Restituisci un errore HTTP 404 (NotFound)
                    return NotFound(new { message = ex.Message });
                }
                catch (Exception ex)
                {
                    // Usa il logger per tracciare l'errore generico
                    _logger.LogError(ex, "Errore durante il ripristino del giocatore");

                    // Restituisci un errore HTTP 400 (BadRequest)
                    return BadRequest(new { message = $"Errore durante il ripristino del giocatore: {ex.Message}" });
                }
            }

        }
    }
}
