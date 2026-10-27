using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpooLLedger.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class OrderLineCompletedAndPaymentStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompletedQuantity",
                table: "OrderItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql("""
                UPDATE Orders SET Status = 'Confirmed payment' WHERE Status = 'Delivered';
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompletedQuantity",
                table: "OrderItems");
        }
    }
}
