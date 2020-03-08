using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ducode.Wolk.Persistence.Migrations
{
    public partial class NoteHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "note_history",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Created = table.Column<DateTimeOffset>(nullable: false),
                    Changed = table.Column<DateTimeOffset>(nullable: true),
                    Title = table.Column<string>(maxLength: 200, nullable: true),
                    Content = table.Column<string>(nullable: true),
                    NoteType = table.Column<int>(nullable: false),
                    OriginalCreated = table.Column<DateTimeOffset>(nullable: false),
                    OriginalChanged = table.Column<DateTimeOffset>(nullable: true),
                    NoteId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_note_history", x => x.Id);
                    table.ForeignKey(
                        name: "FK_note_history_notes_NoteId",
                        column: x => x.NoteId,
                        principalTable: "notes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_note_history_NoteId",
                table: "note_history",
                column: "NoteId");

            migrationBuilder.CreateIndex(
                name: "IX_note_history_Title",
                table: "note_history",
                column: "Title");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "note_history");
        }
    }
}
