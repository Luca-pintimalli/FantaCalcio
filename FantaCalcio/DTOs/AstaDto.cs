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



        public string NomeUtente { get; set; } 
        public string NomeModalita { get; set; } // (Classic o Mantra)
        public string TipoAstaDescrizione { get; set; } // (random o chiamata)
    }
}


