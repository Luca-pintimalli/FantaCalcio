using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FantaCalcio.Models
{
    public class RuoloMantra
    {
        [Key]
        public int ID { get; set; }

        // Relazione con la tabella Giocatore
        [ForeignKey("Giocatore")]
        public int ID_Giocatore { get; set; }
        public Giocatore Giocatore { get; set; }

        // Relazione con la tabella Ruoli (contenente i ruoli della modalità Mantra)
        [ForeignKey("Ruolo")]
        public int ID_Ruolo { get; set; }
        public Ruolo Ruolo { get; set; }
    }
}