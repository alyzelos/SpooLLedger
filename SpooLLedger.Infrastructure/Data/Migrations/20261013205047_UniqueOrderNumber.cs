using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpooLLedger.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UniqueOrderNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                UPDATE "Orders"
                SET "OrderNumber" = (
                    SELECT COUNT(*)
                    FROM "Orders" AS "Older"
                    WHERE "Older"."OrderDate" < "Orders"."OrderDate"
                       OR ("Older"."OrderDate" = "Orders"."OrderDate" AND "Older"."Id" <= "Orders"."Id")
                );
                """);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderNumber",
                table: "Orders",
                column: "OrderNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Orders_OrderNumber",
                table: "Orders");
        }
    }
}
