using System;
using FantaCalcio.Models;

namespace FantaCalcio.Services.Interface
{
	public interface IModalitaService
	{
		Task AddModalita(Modalita modalita);

		Task UpdateModalita(int ID_Modalita, Modalita modalita);

		Task DeleteModalita(int ID_Modalita);

		Task<Modalita> GetModalitaById(int id);

		Task<IEnumerable<Modalita>> GetAll();


	}
}

