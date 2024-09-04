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
        public string Cognome { get; set; }  // Aggiunto per includere il cognome

        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(100)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [MaxLength(255)]
        [Url]
        public string Foto { get; set; }

        [Required]
        public DateTime DataRegistrazione { get; set; }  // Data di registrazione dell'utente

       
        public ICollection<Asta> Aste { get; set; }
    }
}