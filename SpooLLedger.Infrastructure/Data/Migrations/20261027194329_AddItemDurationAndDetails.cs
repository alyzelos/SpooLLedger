using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpooLLedger.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddItemDurationAndDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "PricePerUnit",
                table: "Items",
                type: "TEXT",
                precision: 18,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "TEXT",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AddColumn<string>(
                name: "Details",
                table: "Items",
                type: "TEXT",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PrintDurationMinutes",
                table: "Items",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql("""
                UPDATE Items SET PrintDurationMinutes = 150, Details = 'Base for a small trophy, printed upright.' WHERE Id = 1 AND PrintDurationMinutes = 0;
                UPDATE Items SET PrintDurationMinutes = 180, Details = 'Decorative statue, needs supports.' WHERE Id = 2 AND PrintDurationMinutes = 0;
                UPDATE Items SET PrintDurationMinutes = 480, Details = 'Large toy figure.' WHERE Id = 3 AND PrintDurationMinutes = 0;
                UPDATE Items SET PrintDurationMinutes = 45, Details = 'Small toy car.' WHERE Id = 4 AND PrintDurationMinutes = 0;
                UPDATE Items SET PrintDurationMinutes = 20, Details = 'Keychain with a custom name.' WHERE Id = 5 AND PrintDurationMinutes = 0;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Details",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "PrintDurationMinutes",
                table: "Items");

            migrationBuilder.AlterColumn<decimal>(
                name: "PricePerUnit",
                table: "Items",
                type: "TEXT",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "TEXT",
                oldPrecision: 18,
                oldScale: 2,
                oldNullable: true);
        }
    }
}
