using System;
using System.ComponentModel.DataAnnotations;

namespace FantaCalcio.Models
{
	public class Utente
	{
        [Key]
        public int ID_Utente { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nome { get; set; }

        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(100)]
        public string Password { get; set; }

        [MaxLength(255)]
        public string Foto { get; set; }
    }
}

