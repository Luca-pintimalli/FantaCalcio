using System;
namespace FantaCalcio.DTOs
{
    public class AstaCreateUpdateDto
    {
        public int ID_Utente { get; set; }  // Questo verrà automaticamente popolato dal controller
        public int ID_TipoAsta { get; set; }  // (random o chiamata)
        public int NumeroSquadre { get; set; }  // Numero di squadre partecipanti
        public int CreditiDisponibili { get; set; }  // Crediti iniziali per le squadre
        public int ID_Modalita { get; set; }  //  (Classic o Mantra)
    }
}

