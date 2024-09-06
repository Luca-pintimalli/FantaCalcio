using Microsoft.AspNetCore.Mvc;
using FantaCalcio.Models;
using FantaCalcio.Services.Interface;
using System.Threading.Tasks;
using System.Collections.Generic;
using FantaCalcio.DTOs;

namespace FantaCalcio.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ModalitaController : ControllerBase
    {
        private readonly IModalitaService _modalitaService;

        public ModalitaController(IModalitaService modalitaService)
        {
            _modalitaService = modalitaService;
        }

        [HttpPost]
        public async Task<ActionResult<ModalitaDTO>> AddModalita([FromBody] ModalitaDTO modalitaDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var modalita = new Modalita
            {
                TipoModalita = modalitaDTO.TipoModalita
            };

            await _modalitaService.AddModalita(modalita);

            modalitaDTO.ID_Modalita = modalita.ID_Modalita;

            return CreatedAtAction(nameof(GetModalitaById), new { id = modalita.ID_Modalita }, modalitaDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateModalita(int id, [FromBody] ModalitaDTO modalitaDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var modalitaAggiornata = new Modalita
            {
                TipoModalita = modalitaDTO.TipoModalita
            };

            try
            {
                await _modalitaService.UpdateModalita(id, modalitaAggiornata);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Modalità con ID {id} non trovata.");
            }

            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ModalitaDTO>> GetModalitaById(int id)
        {
            var modalita = await _modalitaService.GetModalitaById(id);
            if (modalita == null)
            {
                return NotFound();
            }

            var modalitaDTO = new ModalitaDTO
            {
                ID_Modalita = modalita.ID_Modalita,
                TipoModalita = modalita.TipoModalita
            };

            return Ok(modalitaDTO);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ModalitaDTO>>> GetAll()
        {
            var modalitaList = await _modalitaService.GetAll();

            var modalitaDTOList = new List<ModalitaDTO>();
            foreach (var modalita in modalitaList)
            {
                modalitaDTOList.Add(new ModalitaDTO
                {
                    ID_Modalita = modalita.ID_Modalita,
                    TipoModalita = modalita.TipoModalita
                });
            }

            return Ok(modalitaDTOList);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteModalita(int id)
        {
            try
            {
                await _modalitaService.DeleteModalita(id);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

            return NoContent();
        }
    }
}

