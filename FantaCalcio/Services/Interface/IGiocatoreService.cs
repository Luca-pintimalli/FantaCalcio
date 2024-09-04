using System;
using FantaCalcio.Models;

namespace FantaCalcio.Services.Interface
{
	public interface IGiocatoreService
	{
		Task AddGiocatore(Giocatore giocatore);

		Task UpdateGiocatore(int ID_Giocatore, Giocatore giocatore);

		Task DeleteGiocatore(int ID_Giocatore);

        Task<Giocatore> GetGiocatoreById(int id);

        Task<IEnumerable<Giocatore>> GetAll();
    }
}

