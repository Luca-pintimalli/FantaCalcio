using FantaCalcio.DTOs;
using FantaCalcio.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FantaCalcio.Services.Interface
{
    public interface IGiocatoreService
    {
        // Aggiungi un nuovo giocatore (senza ruoli Mantra)
        Task AddGiocatore(GiocatoreCreateUpdateDto giocatoreDTO);

        // Aggiorna un giocatore esistente (senza ruoli Mantra)
        Task UpdateGiocatore(int id, GiocatoreCreateUpdateDto giocatoreDTO);

        // Ottenere tutti i giocatori con ruoli Mantra inclusi
        Task<IEnumerable<GiocatoreDto>> GetAll();

        // Ottenere giocatore per ID (con ruoli Mantra inclusi)
        Task<GiocatoreDto> GetGiocatoreById(int id);

        // Eliminare un giocatore
        Task DeleteGiocatore(int ID_Giocatore);
    }
}
