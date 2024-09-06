using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FantaCalcio.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Giocatori",
                columns: table => new
                {
                    ID_Giocatore = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Cognome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Foto = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    SquadraAttuale = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GoalFatti = table.Column<int>(type: "int", nullable: false),
                    GoalSubiti = table.Column<int>(type: "int", nullable: false),
                    Assist = table.Column<int>(type: "int", nullable: false),
                    PartiteGiocate = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Giocatori", x => x.ID_Giocatore);
                });

            migrationBuilder.CreateTable(
                name: "Modalita",
                columns: table => new
                {
                    ID_Modalita = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipoModalita = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modalita", x => x.ID_Modalita);
                });

            migrationBuilder.CreateTable(
                name: "TipiAsta",
                columns: table => new
                {
                    ID_TipoAsta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeTipoAsta = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipiAsta", x => x.ID_TipoAsta);
                });

            migrationBuilder.CreateTable(
                name: "Utenti",
                columns: table => new
                {
                    ID_Utente = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Cognome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Foto = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DataRegistrazione = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utenti", x => x.ID_Utente);
                });

            migrationBuilder.CreateTable(
                name: "Ruoli",
                columns: table => new
                {
                    ID_Ruolo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeRuolo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ID_Modalita = table.Column<int>(type: "int", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ruoli", x => x.ID_Ruolo);
                    table.ForeignKey(
                        name: "FK_Ruoli_Modalita_ID_Modalita",
                        column: x => x.ID_Modalita,
                        principalTable: "Modalita",
                        principalColumn: "ID_Modalita");
                });

            migrationBuilder.CreateTable(
                name: "Aste",
                columns: table => new
                {
                    ID_Asta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Utente = table.Column<int>(type: "int", nullable: false),
                    ID_TipoAsta = table.Column<int>(type: "int", maxLength: 50, nullable: false),
                    NumeroSquadre = table.Column<int>(type: "int", nullable: false),
                    CreditiDisponibili = table.Column<int>(type: "int", nullable: false),
                    ID_Modalita = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aste", x => x.ID_Asta);
                    table.ForeignKey(
                        name: "FK_Aste_Modalita_ID_Modalita",
                        column: x => x.ID_Modalita,
                        principalTable: "Modalita",
                        principalColumn: "ID_Modalita");
                    table.ForeignKey(
                        name: "FK_Aste_TipiAsta_ID_TipoAsta",
                        column: x => x.ID_TipoAsta,
                        principalTable: "TipiAsta",
                        principalColumn: "ID_TipoAsta");
                    table.ForeignKey(
                        name: "FK_Aste_Utenti_ID_Utente",
                        column: x => x.ID_Utente,
                        principalTable: "Utenti",
                        principalColumn: "ID_Utente");
                });

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

            migrationBuilder.CreateTable(
                name: "Squadre",
                columns: table => new
                {
                    ID_Squadra = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Asta = table.Column<int>(type: "int", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Stemma = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreditiTotali = table.Column<int>(type: "int", nullable: false),
                    CreditiSpesi = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Squadre", x => x.ID_Squadra);
                    table.ForeignKey(
                        name: "FK_Squadre_Aste_ID_Asta",
                        column: x => x.ID_Asta,
                        principalTable: "Aste",
                        principalColumn: "ID_Asta");
                });

            migrationBuilder.CreateTable(
                name: "Operazioni",
                columns: table => new
                {
                    ID_Operazione = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Giocatore = table.Column<int>(type: "int", nullable: false),
                    ID_Squadra = table.Column<int>(type: "int", nullable: false),
                    CreditiSpesi = table.Column<int>(type: "int", nullable: false),
                    DataOperazione = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StatoOperazione = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operazioni", x => x.ID_Operazione);
                    table.ForeignKey(
                        name: "FK_Operazioni_Giocatori_ID_Giocatore",
                        column: x => x.ID_Giocatore,
                        principalTable: "Giocatori",
                        principalColumn: "ID_Giocatore");
                    table.ForeignKey(
                        name: "FK_Operazioni_Squadre_ID_Squadra",
                        column: x => x.ID_Squadra,
                        principalTable: "Squadre",
                        principalColumn: "ID_Squadra");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Aste_ID_Modalita",
                table: "Aste",
                column: "ID_Modalita");

            migrationBuilder.CreateIndex(
                name: "IX_Aste_ID_TipoAsta",
                table: "Aste",
                column: "ID_TipoAsta");

            migrationBuilder.CreateIndex(
                name: "IX_Aste_ID_Utente",
                table: "Aste",
                column: "ID_Utente");

            migrationBuilder.CreateIndex(
                name: "IX_GiocatoreRuoloModalita_ID_Asta",
                table: "GiocatoreRuoloModalita",
                column: "ID_Asta");

            migrationBuilder.CreateIndex(
                name: "IX_GiocatoreRuoloModalita_ID_Ruolo",
                table: "GiocatoreRuoloModalita",
                column: "ID_Ruolo");

            migrationBuilder.CreateIndex(
                name: "IX_Operazioni_ID_Giocatore",
                table: "Operazioni",
                column: "ID_Giocatore");

            migrationBuilder.CreateIndex(
                name: "IX_Operazioni_ID_Squadra",
                table: "Operazioni",
                column: "ID_Squadra");

            migrationBuilder.CreateIndex(
                name: "IX_Ruoli_ID_Modalita",
                table: "Ruoli",
                column: "ID_Modalita");

            migrationBuilder.CreateIndex(
                name: "IX_Squadre_ID_Asta",
                table: "Squadre",
                column: "ID_Asta");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GiocatoreRuoloModalita");

            migrationBuilder.DropTable(
                name: "Operazioni");

            migrationBuilder.DropTable(
                name: "Ruoli");

            migrationBuilder.DropTable(
                name: "Giocatori");

            migrationBuilder.DropTable(
                name: "Squadre");

            migrationBuilder.DropTable(
                name: "Aste");

            migrationBuilder.DropTable(
                name: "Modalita");

            migrationBuilder.DropTable(
                name: "TipiAsta");

            migrationBuilder.DropTable(
                name: "Utenti");
        }
    }
}
