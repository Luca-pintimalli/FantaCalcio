using System;
using System.ComponentModel.DataAnnotations;

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
        [MaxLength(50)]
        public string TipoAsta { get; set; } // Classic o Mantra


        public ICollection<GiocatoreRuoloModalita> GiocatoreRuoloModalitas { get; set; }
    }
}