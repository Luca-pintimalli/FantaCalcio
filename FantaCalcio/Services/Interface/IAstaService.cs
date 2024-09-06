using FantaCalcio.DTOs;

namespace FantaCalcio.Services.Interface
{
    public interface IAstaService
    {
        Task AddAsta(int userId, AstaCreateUpdateDto asta);  // Passa l'ID utente automaticamente

        Task UpdateAsta(int ID_Asta, int userId, AstaCreateUpdateDto asta);  // Passa anche l'ID utente per l'aggiornamento

        Task DeleteAsta(int ID_Asta);

        Task<AstaDto> GetAstaById(int ID_Asta);

        Task<IEnumerable<AstaDto>> GetAll();
    }
}