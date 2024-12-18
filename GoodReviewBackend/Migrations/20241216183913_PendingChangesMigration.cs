using Microsoft.EntityFrameworkCore.Migrations;

public partial class PendingChangesMigration : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_GATUNKOW_GATUNKOWO_GATUNEK",
            table: "GATUNKOWOSC");

        migrationBuilder.DropForeignKey(
            name: "FK_GATUNKOW_GATUNKOWO_KSIAZKA",
            table: "GATUNKOWOSC");

        migrationBuilder.DropTable(
            name: "Gatunkowosci");

        migrationBuilder.DropPrimaryKey(
            name: "PK_GATUNKOWOSC",
            table: "GATUNKOWOSC");

        migrationBuilder.DropIndex(
            name: "GATUNKOWOSC2_FK",
            table: "GATUNKOWOSC");

        migrationBuilder.RenameColumn(
            name: "ID_KSIAZKA",
            table: "GATUNKOWOSC",
            newName: "IdKsiazka");

        migrationBuilder.RenameColumn(
            name: "ID_GATUNKU",
            table: "GATUNKOWOSC",
            newName: "IdGatunek");

        migrationBuilder.RenameIndex(
            name: "GATUNKOWOSC_FK",
            table: "GATUNKOWOSC",
            newName: "IX_GATUNKOWOSC_IdGatunek");

        migrationBuilder.AddPrimaryKey(
            name: "PK_GATUNKOWOSC",
            table: "GATUNKOWOSC",
            columns: new[] { "IdKsiazka", "IdGatunek" });

        migrationBuilder.CreateTable(
            name: "GatunekKsiazka",
            columns: table => new
            {
                IdGatunkusIdGatunku = table.Column<int>(type: "int", nullable: false),
                IdKsiazkasIdKsiazka = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_GatunekKsiazka", x => new { x.IdGatunkusIdGatunku, x.IdKsiazkasIdKsiazka });
                table.ForeignKey(
                    name: "FK_GatunekKsiazka_GATUNEK_IdGatunkusIdGatunku",
                    column: x => x.IdGatunkusIdGatunku,
                    principalTable: "GATUNEK",
                    principalColumn: "ID_GATUNKU",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_GatunekKsiazka_KSIAZKA_IdKsiazkasIdKsiazka",
                    column: x => x.IdKsiazkasIdKsiazka,
                    principalTable: "KSIAZKA",
                    principalColumn: "ID_KSIAZKA",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_GatunekKsiazka_IdKsiazkasIdKsiazka",
            table: "GatunekKsiazka",
            column: "IdKsiazkasIdKsiazka");

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

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_GATUNKOWOSC_GATUNEK_IdGatunek",
            table: "GATUNKOWOSC");

        migrationBuilder.DropForeignKey(
            name: "FK_GATUNKOWOSC_KSIAZKA_IdKsiazka",
            table: "GATUNKOWOSC");

        migrationBuilder.DropTable(
            name: "GatunekKsiazka");

        migrationBuilder.DropPrimaryKey(
            name: "PK_GATUNKOWOSC",
            table: "GATUNKOWOSC");

        migrationBuilder.RenameColumn(
            name: "IdKsiazka",
            table: "GATUNKOWOSC",
            newName: "ID_KSIAZKA");

        migrationBuilder.RenameColumn(
            name: "IdGatunek",
            table: "GATUNKOWOSC",
            newName: "ID_GATUNKU");

        migrationBuilder.RenameIndex(
            name: "IX_GATUNKOWOSC_IdGatunek",
            table: "GATUNKOWOSC",
            newName: "GATUNKOWOSC_FK");

        migrationBuilder.AddPrimaryKey(
            name: "PK_GATUNKOWOSC",
            table: "GATUNKOWOSC",
            columns: new[] { "ID_GATUNKU", "ID_KSIAZKA" })
            .Annotation("SqlServer:Clustered", false);

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
            name: "GATUNKOWOSC2_FK",
            table: "GATUNKOWOSC",
            column: "ID_KSIAZKA");

        migrationBuilder.CreateIndex(
            name: "IX_Gatunkowosci_IdGatunek",
            table: "Gatunkowosci",
            column: "IdGatunek");

        migrationBuilder.AddForeignKey(
            name: "FK_GATUNKOW_GATUNKOWO_GATUNEK",
            table: "GATUNKOWOSC",
            column: "ID_GATUNKU",
            principalTable: "GATUNEK",
            principalColumn: "ID_GATUNKU");

        migrationBuilder.AddForeignKey(
            name: "FK_GATUNKOW_GATUNKOWO_KSIAZKA",
            table: "GATUNKOWOSC",
            column: "ID_KSIAZKA",
            principalTable: "KSIAZKA",
            principalColumn: "ID_KSIAZKA");
    }
}
