using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FantaCalcio.Migrations
{
    public partial class AddIDAstaToOperazione : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Aggiungi la colonna ID_Asta
            migrationBuilder.AddColumn<int>(
                name: "ID_Asta",
                table: "Operazioni",
                type: "int",
                nullable: false,
                defaultValue: 0);

            // Aggiungi l'indice e la foreign key
            migrationBuilder.CreateIndex(
                name: "IX_Operazioni_ID_Asta",
                table: "Operazioni",
                column: "ID_Asta");

            migrationBuilder.AddForeignKey(
                name: "FK_Operazioni_Aste_ID_Asta",
                table: "Operazioni",
                column: "ID_Asta",
                principalTable: "Aste",
                principalColumn: "ID_Asta",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Operazioni_Aste_ID_Asta",
                table: "Operazioni");

            migrationBuilder.DropIndex(
                name: "IX_Operazioni_ID_Asta",
                table: "Operazioni");

            migrationBuilder.DropColumn(
                name: "ID_Asta",
                table: "Operazioni");
        }
    }
}
