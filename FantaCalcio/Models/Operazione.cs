﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FantaCalcio.Models
{
    public class Operazione
    {
        [Key]
        public int ID_Operazione { get; set; }

        [Required]
        public int ID_Giocatore { get; set; }

        [Required]
        public int? ID_Squadra { get; set; }

        [Range(0, int.MaxValue)]
        public int? CreditiSpesi { get; set; }

        public DateTime? DataOperazione { get; set; } = DateTime.Now;

        public string StatoOperazione { get; set; } = "Libero"; // Default 'Libero' 


        [ForeignKey("ID_Giocatore")]
        public Giocatore Giocatore { get; set; }

        [ForeignKey("ID_Squadra")]
        public Squadra Squadra { get; set; }

        public int ID_Asta { get; set; }

        [ForeignKey("ID_Asta")]
        public Asta Asta { get; set; }
    }
}