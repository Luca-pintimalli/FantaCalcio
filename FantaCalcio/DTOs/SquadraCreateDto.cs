using System;
namespace FantaCalcio.DTOs
{
    public class SquadraCreateDto
    {
        public int ID_Asta { get; set; }
        public string Nome { get; set; }
        public string Stemma { get; set; }
        public int CreditiSpesi { get; set; } = 0;

        public List<int> GiocatoriIds { get; set; }
        public List<int> OperazioniIds { get; set; }
    }

}

