using System;
using FantaCalcio.DTOs;
using FantaCalcio.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FantaCalcio.Services.Interface
{
    public interface IOperazioneService
    {
        Task<OperazioneDettagliataDto> CreaOperazione(OperazioneDto operazioneDto); // Cambia il tipo di ritorno

        Task UpdateOperazione(int ID_Operazione, OperazioneDto operazioneDto);

        Task DeleteOperazione(int idOperazione, int idSquadra, int creditiSpesi);

        Task<OperazioneDto> GetOperazioneById(int ID_Operazione);

        Task<IEnumerable<OperazioneDto>> GetAll();

        Task CambiaStatoGiocatoreAsync(OperazioneSvincoloDto dto);

        // Nuovo metodo per ottenere operazioni per ID_Asta
        Task<IEnumerable<OperazioneDto>> GetOperazioniByAstaId(int ID_Asta);
    }
}
