using FantaCalcio.DTOs;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

public interface IGiocatoreService
{
    Task<IEnumerable<GiocatoreDto>> GetAll(string ruolo = null, string search = null);
    Task<GiocatoreDto> GetGiocatoreById(int id);
    Task AddGiocatore(GiocatoreCreateUpdateDto giocatoreDto, string filePath);
    Task UpdateGiocatore(int id, GiocatoreCreateUpdateDto giocatoreDto, IFormFile file);  // Modifica qui
    Task DeleteGiocatore(int id);
}
