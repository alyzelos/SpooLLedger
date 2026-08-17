using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpooLLedger.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPropFilamentStock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "FilamentStocks",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "FilamentStocks",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                table: "FilamentStocks");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "FilamentStocks");
        }
    }
}
