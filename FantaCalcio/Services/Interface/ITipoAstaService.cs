using System;
using FantaCalcio.Models;

namespace FantaCalcio.Services.Interface
{
	public interface ITipoAstaService
	{
		Task AddTipoAstaAsync(TipoAsta tipoAsta);

		Task UpdateTipoAstaAsync(int ID_TipoAsta, TipoAsta tipoAsta);

		Task DeleteTipoAstaAsync(int ID_TipoAsta);

		Task<TipoAsta> GetTipoAstaById(int ID_TipoAsta);

		Task<IEnumerable<TipoAsta>> GetAll();

	}
}

