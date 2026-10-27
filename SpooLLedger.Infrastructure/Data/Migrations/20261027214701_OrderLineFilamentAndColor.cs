using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpooLLedger.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class OrderLineFilamentAndColor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "OrderItems",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FilamentType",
                table: "OrderItems",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql("""
                UPDATE OrderItems
                SET FilamentType = (SELECT FilamentType FROM Items WHERE Items.Id = OrderItems.ItemId),
                    Color = (SELECT Color FROM Items WHERE Items.Id = OrderItems.ItemId)
                WHERE EXISTS (SELECT 1 FROM Items WHERE Items.Id = OrderItems.ItemId);
                UPDATE OrderItems SET FilamentType = 'PLA' WHERE FilamentType = '';
                UPDATE OrderItems SET Color = 'Oh My Gold' WHERE Color = '';
                """);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "FilamentType",
                table: "OrderItems");

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

            migrationBuilder.CreateIndex(
                name: "IX_Items_ProductName_FilamentType_Color",
                table: "Items",
                columns: new[] { "ProductName", "FilamentType", "Color" },
                unique: true);
        }
    }
}
