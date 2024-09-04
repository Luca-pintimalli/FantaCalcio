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
        public string TipoAsta { get; set; } //  "random" o "chiamata", indica il tipo di asta

        [Required]
        [MaxLength(50)]
        public string SistemaGioco { get; set; } // Classic o Mantra, specifica il sistema di regole applicato

        [Required]
        [Range(1, int.MaxValue)]
        public int NumeroSquadre { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int CreditiDisponibili { get; set; }

        [Required]
        public int ID_Modalita { get; set; }

        [ForeignKey("ID_Utente")]
        public Utente Utente { get; set; }

        [ForeignKey("ID_Modalita")]
        public Modalita Modalita { get; set; }
        public ICollection<Squadra> Squadre { get; set; }
        public ICollection<GiocatoreRuoloModalita> GiocatoreRuoloModalitas { get; set; }
    }
}
