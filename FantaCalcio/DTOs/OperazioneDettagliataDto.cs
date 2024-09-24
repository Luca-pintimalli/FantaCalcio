public class OperazioneDettagliataDto
{
    public int ID_Operazione { get; set; }
    public int ID_Giocatore { get; set; }
    public string NomeGiocatore { get; set; }
    public string CognomeGiocatore { get; set; }
    public string RuoloClassic { get; set; }
    public int? ID_Squadra { get; set; }
    public string NomeSquadra { get; set; }
    public int? CreditiSpesi { get; set; }
    public string StatoOperazione { get; set; }
    public DateTime DataOperazione { get; set; }
    public int ID_Asta { get; set; }
}
