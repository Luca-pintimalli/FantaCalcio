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

        [Required]
        [MaxLength(50)]
        public int ID_TipoAsta { get; set; } //  "random" o "chiamata", indica il tipo di asta

        [Required]
        [Range(1, int.MaxValue)]
        public int NumeroSquadre { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int CreditiDisponibili { get; set; }

        [Required]
        public int ID_Modalita { get; set; } //Classic o Mantra

        [ForeignKey("ID_Utente")]
        public Utente Utente { get; set; }

        [ForeignKey("ID_Modalita")]
        public Modalita Modalita { get; set; }

        [ForeignKey("ID_TipoAsta")]
        public TipoAsta TipoAsta { get; set; }


        public ICollection<Squadra> Squadre { get; set; }

    }
}
