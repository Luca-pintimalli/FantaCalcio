using System.ComponentModel.DataAnnotations;

public class RuoloDto
{
    public int ID_Ruolo { get; set; }

    [Required]
    [MaxLength(50)]
    public string NomeRuolo { get; set; }

    [Required]
    public int ID_Modalita { get; set; }

}
