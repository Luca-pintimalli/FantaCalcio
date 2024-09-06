using System;
using System.ComponentModel.DataAnnotations;

namespace FantaCalcio.Models
{
    public class TipoAsta
    {
        [Key]
        public int ID_TipoAsta { get; set; }

        [Required]
        [MaxLength(50)]
        public string NomeTipoAsta { get; set; }

        public ICollection<Asta> Aste { get; set; }
    }
}