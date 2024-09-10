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
        // Limiti per ruolo
        public int MaxPortieri { get; set; }  // Limite massimo di portieri per squadra
        public int MaxDifensori { get; set; }  // Limite massimo di difensori per squadra
        public int MaxCentrocampisti { get; set; }  // Limite massimo di centrocampisti per squadra
        public int MaxAttaccanti { get; set; }  // Limite massimo di attaccanti per squadra

    }
}

