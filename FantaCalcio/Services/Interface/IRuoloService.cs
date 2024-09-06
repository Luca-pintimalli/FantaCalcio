using System;
using FantaCalcio.Models;

namespace FantaCalcio.Services.Interface
{
	public interface IRuoloService
	{
		Task AddRuoloAsync(Ruolo ruolo);

		Task<Ruolo> GetRuoloByIdAsync(int ID_Ruolo);

		Task<IEnumerable<Ruolo>> GetAllAsync();

		Task UpdateRuoloAsync(int ID_Ruolo, Ruolo ruolo);

		Task DeleteRuoloAsync(int ID_Ruolo);
	}
}

