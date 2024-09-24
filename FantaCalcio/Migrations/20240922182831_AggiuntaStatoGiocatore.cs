using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FantaCalcio.Migrations
{
    public partial class AggiuntaStatoGiocatore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StatoGiocatore",
                table: "Giocatori",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "Disponibile"); // Imposta un valore predefinito

          
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatoGiocatore",
                table: "Giocatori");
        }
    }
}
