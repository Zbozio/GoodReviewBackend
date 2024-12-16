using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoodReviewBackend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataUrodzenia",
                table: "UZYTKOWNIK",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataUrodzenia",
                table: "UZYTKOWNIK");
        }
    }
}
