using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpooLLedger.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class FilamentColors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FilamentColors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilamentColors", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FilamentColors_Name",
                table: "FilamentColors",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FilamentColors");
        }
    }
}
