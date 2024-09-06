using FantaCalcio.DTOs;
using FantaCalcio.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FantaCalcio.Services.Interface
{
    public interface IRuoloMantraService
    {
        // Aggiungo un ruolo Mantra a un giocatore
        Task AddRuoloMantra(int idGiocatore, int idRuolo);

        // Rimuovo un ruolo Mantra da un giocatore
        Task RemoveRuoloMantra(int idGiocatore, int idRuolo);

        // Ottengo tutti i ruoli Mantra di tutti i  giocatori
        Task<IEnumerable<RuoloMantraDTO>> GetAllRuoliMantra();

        //Ottengo tutti i ruoli mantra di un singolo giocatore 
        Task<IEnumerable<RuoloMantraDTO>> GetRuoliMantraByGiocatoreId(int idGiocatore);

    }
}
