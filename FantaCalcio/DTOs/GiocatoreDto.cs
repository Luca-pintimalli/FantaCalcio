namespace FantaCalcio.DTOs
{
    public class GiocatoreDto
    {
        public int ID_Giocatore { get; set; }

        public string Nome { get; set; }
        public string Cognome { get; set; }
        public string Foto { get; set; }
        public string SquadraAttuale { get; set; }
        public int GoalFatti { get; set; }
        public int GoalSubiti { get; set; }
        public int Assist { get; set; }
        public int PartiteGiocate { get; set; }

        public string RuoloClassic { get; set; }

        public List<RuoloMantraDTO> RuoliMantra { get; set; }


    }
}