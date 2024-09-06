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

        [Required]
        [MaxLength(100)]
        public string Nome { get; set; }

        [MaxLength(255)]
        [Url]
        public string Stemma { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int CreditiTotali { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int CreditiSpesi { get; set; } = 0;

        [ForeignKey("ID_Asta")]
        public Asta Asta { get; set; }

        public ICollection<Giocatore> Giocatori { get; set; } 

        public ICollection<Operazione> Operazioni { get; set; }
    }
}
