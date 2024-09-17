using System;
namespace FantaCalcio.DTOs
{
    public class RegisterModel
    {
        public required string Nome { get; set; }
        public required string Cognome { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }

}
