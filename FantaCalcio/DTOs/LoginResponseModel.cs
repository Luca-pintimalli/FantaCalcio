using System;
namespace FantaCalcio.DTOs
{
    public class LoginResponseModel
    {
        public required string UserName { get; set; }

        public required string Token { get; set; }

        public DateTime Expires { get; set; }

    }
}
