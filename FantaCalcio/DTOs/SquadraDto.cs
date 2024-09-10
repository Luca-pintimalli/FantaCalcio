using System;
using FantaCalcio.Models;

namespace FantaCalcio.DTOs
{
    public class SquadraDto
    {
        public int ID_Squadra { get; set; }
        public int ID_Asta { get; set; }
        public string Nome { get; set; }
        public string Stemma { get; set; }
        public int CreditiTotali { get; set; }
        public int CreditiSpesi { get; set; } = 0;

        // Non includere direttamente Asta, Giocatori, Operazioni nel DTO
        public List<int> GiocatoriIds { get; set; } // Lista degli ID dei giocatori
        public List<int> OperazioniIds { get; set; } // Lista degli ID delle operazioni
    }
}
