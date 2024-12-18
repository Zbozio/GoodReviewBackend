using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoodReviewBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddGatunkowoscTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Gatunkowosci",
                columns: table => new
                {
                    IdKsiazka = table.Column<int>(type: "int", nullable: false),
                    IdGatunek = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gatunkowosci", x => new { x.IdKsiazka, x.IdGatunek });
                    table.ForeignKey(
                        name: "FK_Gatunkowosci_GATUNEK_IdGatunek",
                        column: x => x.IdGatunek,
                        principalTable: "GATUNEK",
                        principalColumn: "ID_GATUNKU");
                    table.ForeignKey(
                        name: "FK_Gatunkowosci_KSIAZKA_IdKsiazka",
                        column: x => x.IdKsiazka,
                        principalTable: "KSIAZKA",
                        principalColumn: "ID_KSIAZKA");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Gatunkowosci_IdGatunek",
                table: "Gatunkowosci",
                column: "IdGatunek");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Gatunkowosci");
        }
    }
}
