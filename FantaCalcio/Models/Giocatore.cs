using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FantaCalcio.Models
{
    public class Giocatore
    {
        [Key]
        public int ID_Giocatore { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nome { get; set; }

        [Required]
        [MaxLength(100)]
        public string Cognome { get; set; } 

        [MaxLength(255)]
        [Url]
        public string Foto { get; set; }

        [MaxLength(100)]
        public string SquadraAttuale { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int GoalFatti { get; set; } = 0;

        [Required]
        [Range(0, int.MaxValue)]
        public int GoalSubiti { get; set; } = 0;

        [Required]
        [Range(0, int.MaxValue)]
        public int Assist { get; set; } = 0;

        [Required]
        [Range(0, int.MaxValue)]
        public int PartiteGiocate { get; set; } = 0;

        [Required]
        [MaxLength(50)]
        public string RuoloClassic { get; set; }

        public int? ID_Squadra { get; set; }

        [ForeignKey("ID_Squadra")]
        public Squadra Squadra { get; set; }


        public ICollection<Operazione> Operazioni { get; set; }
        public ICollection<RuoloMantra> RuoliMantra { get; set; }
    }
}
