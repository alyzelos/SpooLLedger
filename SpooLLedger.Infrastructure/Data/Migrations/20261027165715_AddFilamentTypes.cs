using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpooLLedger.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFilamentTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FilamentTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilamentTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FilamentTypes_Name",
                table: "FilamentTypes",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FilamentTypes");
        }
    }
}
