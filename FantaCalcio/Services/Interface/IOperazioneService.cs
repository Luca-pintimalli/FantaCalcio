using System;
using FantaCalcio.DTOs;
using FantaCalcio.Models;

namespace FantaCalcio.Services.Interface
{
    public interface IOperazioneService
    {
        Task<Operazione> CreaOperazione(OperazioneDto operazioneDto);

        Task UpdateOperazione(int ID_Operazione, OperazioneDto operazioneDto);

        Task DeleteOperazione(int ID_Operazione);

        Task<OperazioneDto> GetOperazioneById(int ID_Operazione);

        Task<IEnumerable<OperazioneDto>> GetAll();
    }
}