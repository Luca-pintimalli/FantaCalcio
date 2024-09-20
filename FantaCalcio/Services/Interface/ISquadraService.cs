using System;
using FantaCalcio.DTOs;
using Microsoft.AspNetCore.Http;

namespace FantaCalcio.Services.Interface
{
    public interface ISquadraService
    {
        Task CreateSquadra(int ID_Asta, SquadraCreateDto squadraDto);

        // Aggiungi il supporto per IFormFile per la gestione delle immagini
        Task UpdateSquadra(int ID_Squadra, SquadraUpdateDto squadraDto, IFormFile? foto);

        Task DeleteSquadra(int ID_Squadra);

        Task<SquadraDto> GetSquadraById(int ID_Squadra);

        Task<IEnumerable<SquadraDto>> GetAll();
    }
}
