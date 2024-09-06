using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FantaCalcio.Models
{
    public class Ruolo
    {
        [Key]
        public int ID_Ruolo { get; set; }

        [Required]
        [MaxLength(50)]
        public string NomeRuolo { get; set; }

        [Required]
        public int ID_Modalita { get; set; } // Classic o Mantra

        [ForeignKey("ID_Modalita")]
        public Modalita Modalita { get; set; }


        public ICollection<RuoloMantra> RuoliMantra { get; set; } 

    }
}