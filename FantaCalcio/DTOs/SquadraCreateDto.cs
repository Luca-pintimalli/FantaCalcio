﻿public class SquadraCreateDto
{
    public int ID_Asta { get; set; }
    public string Nome { get; set; }
    public string Stemma { get; set; }
    public int CreditiSpesi { get; set; } = 0;  // Sarà sempre 0 alla creazione
}
