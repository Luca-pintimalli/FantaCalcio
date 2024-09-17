public class LoginResponseModel
{
    public required int id { get; set; }  // Aggiungi l'ID dell'utente
    public required string UserName { get; set; }
    public required string Cognome { get; set; }  // Aggiungi il cognome
    public required string Email { get; set; }    // Aggiungi l'email
    public string Foto { get; set; }  // Aggiungi l'URL della foto se disponibile
    public required string Token { get; set; }
    public DateTime Expires { get; set; }
    public DateTime DataRegistrazione { get; set; }  // Aggiungi la data di registrazione
}
