using System.Collections.Generic;
using System.Threading.Tasks;
using FantaCalcio.DTOs;

namespace FantaCalcio.Services.Interface
{
    public interface IGiocatoreService
    {
        Task<IEnumerable<GiocatoreDto>> GetAll(string ruolo = null, string search = null);  
        Task<GiocatoreDto> GetGiocatoreById(int id);
        Task AddGiocatore(GiocatoreCreateUpdateDto giocatoreDto);
        Task UpdateGiocatore(int id, GiocatoreCreateUpdateDto giocatoreDto);
        Task DeleteGiocatore(int id);
    }
}
