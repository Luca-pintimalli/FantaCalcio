using System;
namespace FantaCalcio.DTOs
{

    public class OperazioneDto
    {
        public int ID_Operazione { get; set; }
        public int ID_Giocatore { get; set; }
        public int? ID_Squadra { get; set; }
        public int? CreditiSpesi { get; set; }
        public string StatoOperazione { get; set; }
        public DateTime? DataOperazione { get; set; }

        public int ID_Asta { get; set; }



    }
}
