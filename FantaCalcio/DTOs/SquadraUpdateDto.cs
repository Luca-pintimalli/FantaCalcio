using System.ComponentModel.DataAnnotations;

namespace FantaCalcio.DTOs
{
    public class SquadraUpdateDto
    {
        [Required]
        public string Nome { get; set; }

        [Required]
        public int CreditiTotali { get; set; }

        public string? Stemma { get; set; }  // Questo campo è opzionale se l'immagine non viene cambiata
    }
}
