using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FantaCalcio.Models
{
	public class Giocatore_Ruolo_Modalita
	{
        [Required]
        public int ID_Giocatore { get; set; }

        [ForeignKey(nameof(ID_Giocatore))]
        public Giocatore Giocatore { get; set; }

        [Required]
        public int ID_Ruolo { get; set; }

        [ForeignKey(nameof(ID_Ruolo))]
        public Ruolo Ruolo { get; set; }

        [Required]
        public int ID_Asta { get; set; }

        [ForeignKey(nameof(ID_Asta))]
        public Asta Asta { get; set; }

        [Key]
        public (int ID_Giocatore, int ID_Ruolo, int ID_Asta) CompositeKey { get; set; }
    }
}

