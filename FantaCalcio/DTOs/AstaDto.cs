using System;
namespace FantaCalcio.DTOs
{
	
    public class AstaDto
    {
        public int ID_Asta { get; set; }
        public int ID_Utente { get; set; }
        public int ID_TipoAsta { get; set; } //  "random" o "chiamata"
        public int NumeroSquadre { get; set; }
        public int CreditiDisponibili { get; set; }
        public int ID_Modalita { get; set; }

        // Limiti per ruolo
        public int MaxPortieri { get; set; }  // Limite massimo di portieri per squadra
        public int MaxDifensori { get; set; }  // Limite massimo di difensori per squadra
        public int MaxCentrocampisti { get; set; }  // Limite massimo di centrocampisti per squadra
        public int MaxAttaccanti { get; set; }  // Limite massimo di attaccanti per squadra

        public string NomeUtente { get; set; } 
        public string NomeModalita { get; set; } // (Classic o Mantra)
        public string TipoAstaDescrizione { get; set; } // (random o chiamata)
    }
}


