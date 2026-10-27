using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpooLLedger.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class OrderPricingName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PricingName",
                table: "Orders",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql("UPDATE \"Orders\" SET \"PricingName\" = 'Standard' WHERE \"PricingName\" = '';");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PricingName",
                table: "Orders");
        }
    }
}
