using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FantaCalcio.Models
{
	public class Asta
	{
        [Key]
        public int ID_Asta { get; set; }

        [Required]
        public int ID_Utente { get; set; }

        [ForeignKey(nameof(ID_Utente))]
        public Utente Utente { get; set; }

        [Required]
        [MaxLength(50)]
        public string TipoAsta { get; set; } // random  o chiamata 

        [Required]
        [MaxLength(50)]
        public string SistemaGioco { get; set; } // Classic o Mantra

        [Required]
        public int NumeroSquadre { get; set; }

        [Required]
        public int CreditiDisponibili { get; set; }

        [Required]
        public int ID_Modalita { get; set; }

        [ForeignKey(nameof(ID_Modalita))]
        public Modalita Modalita { get; set; }
    }
}

