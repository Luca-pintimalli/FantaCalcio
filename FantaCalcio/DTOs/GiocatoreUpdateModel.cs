public class GiocatoreUpdateModel
{
    public string Nome { get; set; }
    public string Cognome { get; set; }
    public string SquadraAttuale { get; set; }
    public int GoalFatti { get; set; }
    public int GoalSubiti { get; set; }
    public int Assist { get; set; }
    public int PartiteGiocate { get; set; }
    public string RuoloClassic { get; set; }
    public IFormFile? File { get; set; } 
}
