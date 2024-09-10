using System;
using FantaCalcio.DTOs;

namespace FantaCalcio.Services.Interface
{
	public interface ISquadraService
	{
		Task CreateSquadra(int ID_Asta, SquadraCreateDto squadraDto);

		Task UpdateSquadra(int ID_Squadra, SquadraDto squadraDto);

		Task DeleteSquadra(int ID_Squadra);

		Task<SquadraDto> GetSquadraById(int ID_Squadra);

		Task<IEnumerable<SquadraDto>> GetAll();
	}
}

