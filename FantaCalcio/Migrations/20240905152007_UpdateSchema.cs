using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FantaCalcio.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GiocatoreRuoloModalita");

            migrationBuilder.AddColumn<int>(
                name: "ID_Squadra",
                table: "Giocatori",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RuoloClassic",
                table: "Giocatori",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "RuoloMantra",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Giocatore = table.Column<int>(type: "int", nullable: false),
                    ID_Ruolo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RuoloMantra", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RuoloMantra_Giocatori_ID_Giocatore",
                        column: x => x.ID_Giocatore,
                        principalTable: "Giocatori",
                        principalColumn: "ID_Giocatore");
                    table.ForeignKey(
                        name: "FK_RuoloMantra_Ruoli_ID_Ruolo",
                        column: x => x.ID_Ruolo,
                        principalTable: "Ruoli",
                        principalColumn: "ID_Ruolo");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Giocatori_ID_Squadra",
                table: "Giocatori",
                column: "ID_Squadra");

            migrationBuilder.CreateIndex(
                name: "IX_RuoloMantra_ID_Giocatore",
                table: "RuoloMantra",
                column: "ID_Giocatore");

            migrationBuilder.CreateIndex(
                name: "IX_RuoloMantra_ID_Ruolo",
                table: "RuoloMantra",
                column: "ID_Ruolo");

            migrationBuilder.AddForeignKey(
                name: "FK_Giocatori_Squadre_ID_Squadra",
                table: "Giocatori",
                column: "ID_Squadra",
                principalTable: "Squadre",
                principalColumn: "ID_Squadra");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Giocatori_Squadre_ID_Squadra",
                table: "Giocatori");

            migrationBuilder.DropTable(
                name: "RuoloMantra");

            migrationBuilder.DropIndex(
                name: "IX_Giocatori_ID_Squadra",
                table: "Giocatori");

            migrationBuilder.DropColumn(
                name: "ID_Squadra",
                table: "Giocatori");

            migrationBuilder.DropColumn(
                name: "RuoloClassic",
                table: "Giocatori");

            migrationBuilder.CreateTable(
                name: "GiocatoreRuoloModalita",
                columns: table => new
                {
                    ID_Giocatore = table.Column<int>(type: "int", nullable: false),
                    ID_Ruolo = table.Column<int>(type: "int", nullable: false),
                    ID_Asta = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GiocatoreRuoloModalita", x => new { x.ID_Giocatore, x.ID_Ruolo, x.ID_Asta });
                    table.ForeignKey(
                        name: "FK_GiocatoreRuoloModalita_Aste_ID_Asta",
                        column: x => x.ID_Asta,
                        principalTable: "Aste",
                        principalColumn: "ID_Asta");
                    table.ForeignKey(
                        name: "FK_GiocatoreRuoloModalita_Giocatori_ID_Giocatore",
                        column: x => x.ID_Giocatore,
                        principalTable: "Giocatori",
                        principalColumn: "ID_Giocatore");
                    table.ForeignKey(
                        name: "FK_GiocatoreRuoloModalita_Ruoli_ID_Ruolo",
                        column: x => x.ID_Ruolo,
                        principalTable: "Ruoli",
                        principalColumn: "ID_Ruolo");
                });

            migrationBuilder.CreateIndex(
                name: "IX_GiocatoreRuoloModalita_ID_Asta",
                table: "GiocatoreRuoloModalita",
                column: "ID_Asta");

            migrationBuilder.CreateIndex(
                name: "IX_GiocatoreRuoloModalita_ID_Ruolo",
                table: "GiocatoreRuoloModalita",
                column: "ID_Ruolo");
        }
    }
}
