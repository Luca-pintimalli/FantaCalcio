using System;
using System.ComponentModel.DataAnnotations;

namespace FantaCalcio.Models
{
	public class Modalita
	{
        [Key]
        public int ID_Modalita { get; set; }

        [Required]
        [MaxLength(50)]
        public string TipoModalita { get; set; } // Random o a chiamata
    }
}

