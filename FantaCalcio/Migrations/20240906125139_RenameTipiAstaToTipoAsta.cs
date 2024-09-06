using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FantaCalcio.Migrations
{
    /// <inheritdoc />
    public partial class RenameTipiAstaToTipoAsta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Aste_TipiAsta_ID_TipoAsta",
                table: "Aste");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TipiAsta",
                table: "TipiAsta");

            migrationBuilder.RenameTable(
                name: "TipiAsta",
                newName: "TipoAsta");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TipoAsta",
                table: "TipoAsta",
                column: "ID_TipoAsta");

            migrationBuilder.AddForeignKey(
                name: "FK_Aste_TipoAsta_ID_TipoAsta",
                table: "Aste",
                column: "ID_TipoAsta",
                principalTable: "TipoAsta",
                principalColumn: "ID_TipoAsta");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Aste_TipoAsta_ID_TipoAsta",
                table: "Aste");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TipoAsta",
                table: "TipoAsta");

            migrationBuilder.RenameTable(
                name: "TipoAsta",
                newName: "TipiAsta");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TipiAsta",
                table: "TipiAsta",
                column: "ID_TipoAsta");

            migrationBuilder.AddForeignKey(
                name: "FK_Aste_TipiAsta_ID_TipoAsta",
                table: "Aste",
                column: "ID_TipoAsta",
                principalTable: "TipiAsta",
                principalColumn: "ID_TipoAsta");
        }
    }
}
