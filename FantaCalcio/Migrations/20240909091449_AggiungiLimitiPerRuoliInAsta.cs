using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FantaCalcio.Migrations
{
    /// <inheritdoc />
    public partial class AggiungiLimitiPerRuoliInAsta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaxAttaccanti",
                table: "Aste",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaxCentrocampisti",
                table: "Aste",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaxDifensori",
                table: "Aste",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaxPortieri",
                table: "Aste",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxAttaccanti",
                table: "Aste");

            migrationBuilder.DropColumn(
                name: "MaxCentrocampisti",
                table: "Aste");

            migrationBuilder.DropColumn(
                name: "MaxDifensori",
                table: "Aste");

            migrationBuilder.DropColumn(
                name: "MaxPortieri",
                table: "Aste");
        }
    }
}
