using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FantaCalcio.Migrations
{
    /// <inheritdoc />
    public partial class MakeSquadraNullableInOperazioni : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int?>(
                name: "ID_Squadra",
                table: "Operazioni",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ID_Squadra",
                table: "Operazioni",
                type: "int",
                nullable: false,
                oldClrType: typeof(int?),
                oldType: "int",
                oldNullable: true);
        }
    }
}
