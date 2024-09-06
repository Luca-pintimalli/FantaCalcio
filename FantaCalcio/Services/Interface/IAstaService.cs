using System;
using FantaCalcio.Models;

namespace FantaCalcio.Services.Interface
{
	public interface IAstaService
	{
		Task AddAsta(Asta asta);

		Task UpdateAsta(int ID_Asta, Asta asta);

		Task DeleteAsta(int ID_Asta);

		Task<Asta> GetAstaById(int ID_Asta);

		Task<IEnumerable<Asta>> GetAll();

	}
}

