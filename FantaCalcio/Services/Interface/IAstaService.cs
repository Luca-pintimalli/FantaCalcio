using FantaCalcio.DTOs;
using FantaCalcio.Models;

namespace FantaCalcio.Services.Interface
{
    public interface IAstaService
    {
        Task<Asta> AddAsta(int userId, AstaCreateUpdateDto asta);  // Passa l'ID utente automaticamente

        Task UpdateAsta(int ID_Asta, int userId, AstaCreateUpdateDto asta);  // Passa anche l'ID utente per l'aggiornamento

        Task DeleteAsta(int ID_Asta);

        Task<AstaDto> GetAstaById(int ID_Asta);

        Task<IEnumerable<AstaDto>> GetAll();



        // Metodo per selezionare un giocatore casuale
        Task<Giocatore> SelezionaGiocatoreRandomAsync(int squadraId);

        // Metodo per gestire il prossimo giocatore in base al tipo d'asta (random o a chiamata)
        Task<Giocatore> ProssimoGiocatoreAsync(int squadraId);

        // Metodo per cercare un giocatore per cognome
        Task<Giocatore> CercaGiocatoreAsync(int idAsta, string nome, string cognome);
    }
}