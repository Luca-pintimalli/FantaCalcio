using System;
using System.ComponentModel.DataAnnotations;

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

        
        public ICollection<Operazione> Operazioni { get; set; }
        public ICollection<GiocatoreRuoloModalita> GiocatoreRuoloModalitas { get; set; }
    }
}
