using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoodReviewBackend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateColumnNamesToIdGatunku : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GATUNKOWOSC_GATUNEK_IdGatunek",
                table: "GATUNKOWOSC");

            migrationBuilder.DropForeignKey(
                name: "FK_GATUNKOWOSC_KSIAZKA_IdKsiazka",
                table: "GATUNKOWOSC");

            migrationBuilder.RenameColumn(
                name: "IdGatunek",
                table: "GATUNKOWOSC",
                newName: "IdGatunku");

            migrationBuilder.RenameIndex(
                name: "IX_GATUNKOWOSC_IdGatunek",
                table: "GATUNKOWOSC",
                newName: "IX_GATUNKOWOSC_IdGatunku");

            migrationBuilder.AddColumn<int>(
                name: "GatunekIdGatunku",
                table: "GATUNKOWOSC",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "KsiazkaIdKsiazka",
                table: "GATUNKOWOSC",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GATUNKOWOSC_GatunekIdGatunku",
                table: "GATUNKOWOSC",
                column: "GatunekIdGatunku");

            migrationBuilder.CreateIndex(
                name: "IX_GATUNKOWOSC_KsiazkaIdKsiazka",
                table: "GATUNKOWOSC",
                column: "KsiazkaIdKsiazka");

            migrationBuilder.AddForeignKey(
                name: "FK_GATUNKOWOSC_GATUNEK_GatunekIdGatunku",
                table: "GATUNKOWOSC",
                column: "GatunekIdGatunku",
                principalTable: "GATUNEK",
                principalColumn: "ID_GATUNKU");

            migrationBuilder.AddForeignKey(
                name: "FK_GATUNKOWOSC_KSIAZKA_KsiazkaIdKsiazka",
                table: "GATUNKOWOSC",
                column: "KsiazkaIdKsiazka",
                principalTable: "KSIAZKA",
                principalColumn: "ID_KSIAZKA");

            migrationBuilder.AddForeignKey(
                name: "FK_GATUNKOW_GATUNKOWO_GATUNEK",
                table: "GATUNKOWOSC",
                column: "IdGatunku",
                principalTable: "GATUNEK",
                principalColumn: "ID_GATUNKU");

            migrationBuilder.AddForeignKey(
                name: "FK_GATUNKOW_GATUNKOWO_KSIAZKA",
                table: "GATUNKOWOSC",
                column: "IdKsiazka",
                principalTable: "KSIAZKA",
                principalColumn: "ID_KSIAZKA");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GATUNKOWOSC_GATUNEK_GatunekIdGatunku",
                table: "GATUNKOWOSC");

            migrationBuilder.DropForeignKey(
                name: "FK_GATUNKOWOSC_KSIAZKA_KsiazkaIdKsiazka",
                table: "GATUNKOWOSC");

            migrationBuilder.DropForeignKey(
                name: "FK_GATUNKOW_GATUNKOWO_GATUNEK",
                table: "GATUNKOWOSC");

            migrationBuilder.DropForeignKey(
                name: "FK_GATUNKOW_GATUNKOWO_KSIAZKA",
                table: "GATUNKOWOSC");

            migrationBuilder.DropIndex(
                name: "IX_GATUNKOWOSC_GatunekIdGatunku",
                table: "GATUNKOWOSC");

            migrationBuilder.DropIndex(
                name: "IX_GATUNKOWOSC_KsiazkaIdKsiazka",
                table: "GATUNKOWOSC");

            migrationBuilder.DropColumn(
                name: "GatunekIdGatunku",
                table: "GATUNKOWOSC");

            migrationBuilder.DropColumn(
                name: "KsiazkaIdKsiazka",
                table: "GATUNKOWOSC");

            migrationBuilder.RenameColumn(
                name: "IdGatunku",
                table: "GATUNKOWOSC",
                newName: "IdGatunek");

            migrationBuilder.RenameIndex(
                name: "IX_GATUNKOWOSC_IdGatunku",
                table: "GATUNKOWOSC",
                newName: "IX_GATUNKOWOSC_IdGatunek");

            migrationBuilder.AddForeignKey(
                name: "FK_GATUNKOWOSC_GATUNEK_IdGatunek",
                table: "GATUNKOWOSC",
                column: "IdGatunek",
                principalTable: "GATUNEK",
                principalColumn: "ID_GATUNKU");

            migrationBuilder.AddForeignKey(
                name: "FK_GATUNKOWOSC_KSIAZKA_IdKsiazka",
                table: "GATUNKOWOSC",
                column: "IdKsiazka",
                principalTable: "KSIAZKA",
                principalColumn: "ID_KSIAZKA");
        }
    }
}
