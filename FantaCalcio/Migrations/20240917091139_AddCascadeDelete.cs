using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FantaCalcio.Migrations
{
    /// <inheritdoc />
    public partial class AddCascadeDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Operazioni_Giocatori_ID_Giocatore",
                table: "Operazioni");

            migrationBuilder.DropForeignKey(
                name: "FK_RuoloMantra_Giocatori_ID_Giocatore",
                table: "RuoloMantra");

            migrationBuilder.AddForeignKey(
                name: "FK_Operazioni_Giocatori_ID_Giocatore",
                table: "Operazioni",
                column: "ID_Giocatore",
                principalTable: "Giocatori",
                principalColumn: "ID_Giocatore",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RuoloMantra_Giocatori_ID_Giocatore",
                table: "RuoloMantra",
                column: "ID_Giocatore",
                principalTable: "Giocatori",
                principalColumn: "ID_Giocatore",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Operazioni_Giocatori_ID_Giocatore",
                table: "Operazioni");

            migrationBuilder.DropForeignKey(
                name: "FK_RuoloMantra_Giocatori_ID_Giocatore",
                table: "RuoloMantra");

            migrationBuilder.AddForeignKey(
                name: "FK_Operazioni_Giocatori_ID_Giocatore",
                table: "Operazioni",
                column: "ID_Giocatore",
                principalTable: "Giocatori",
                principalColumn: "ID_Giocatore");

            migrationBuilder.AddForeignKey(
                name: "FK_RuoloMantra_Giocatori_ID_Giocatore",
                table: "RuoloMantra",
                column: "ID_Giocatore",
                principalTable: "Giocatori",
                principalColumn: "ID_Giocatore");
        }
    }
}
