using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpooLLedger.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ItemFilamentTypeAndColor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Items",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FilamentType",
                table: "Items",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql("""
                UPDATE Items SET FilamentType = 'PLA', Color = 'Oh My Gold' WHERE ProductName = 'trophy stand';
                UPDATE Items SET FilamentType = 'PLA', Color = 'Azure Blue' WHERE ProductName = 'statue';
                UPDATE Items SET FilamentType = 'PLA', Color = 'Lipstick Red' WHERE ProductName = 'toy';
                UPDATE Items SET FilamentType = 'PLA', Color = 'Blend My Silverness' WHERE ProductName = 'car';
                UPDATE Items SET FilamentType = 'PET-G', Color = 'Oh My Gold' WHERE ProductName = 'keychain';
                UPDATE Items SET FilamentType = 'PLA', Color = ProductName WHERE FilamentType = '' OR Color = '';
                """);

            migrationBuilder.CreateIndex(
                name: "IX_Items_ProductName_FilamentType_Color",
                table: "Items",
                columns: new[] { "ProductName", "FilamentType", "Color" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Items_ProductName_FilamentType_Color",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "FilamentType",
                table: "Items");
        }
    }
}
