using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FantaCalcio.Models
{
    public class GiocatoreRuoloModalita
    {
        [Required]
        public int ID_Giocatore { get; set; }

        [Required]
        public int ID_Ruolo { get; set; }

        [Required]
        public int ID_Asta { get; set; }


        public Giocatore Giocatore { get; set; }
        public Ruolo Ruolo { get; set; }
        public Asta Asta { get; set; }
    }
}
