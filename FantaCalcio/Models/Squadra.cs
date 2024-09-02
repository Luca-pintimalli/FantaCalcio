using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FantaCalcio.Models
{
	public class Squadra
	{
        [Key]
        public int ID_Squadra { get; set; }

        [Required]
        public int ID_Asta { get; set; }

        [ForeignKey(nameof(ID_Asta))]
        public Asta Asta { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nome { get; set; }

        [MaxLength(255)]
        public string Stemma { get; set; }

        public int CreditiTotali { get; set; }

        public int CreditiSpesi { get; set; } = 0;
    }
}

