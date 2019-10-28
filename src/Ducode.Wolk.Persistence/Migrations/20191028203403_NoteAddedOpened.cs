using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ducode.Wolk.Persistence.Migrations
{
    public partial class NoteAddedOpened : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "Opened",
                table: "notes",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_notes_Opened",
                table: "notes",
                column: "Opened");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_notes_Opened",
                table: "notes");

            migrationBuilder.DropColumn(
                name: "Opened",
                table: "notes");
        }
    }
}
