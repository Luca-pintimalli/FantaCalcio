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

        [MaxLength(255)]
        public string Foto { get; set; }

        [MaxLength(100)]
        public string SquadraAttuale { get; set; }

        public int GoalFatti { get; set; } = 0;

        public int GoalSubiti { get; set; } = 0;

        public int Assist { get; set; } = 0;

        public int PartiteGiocate { get; set; } = 0;
    }
}

