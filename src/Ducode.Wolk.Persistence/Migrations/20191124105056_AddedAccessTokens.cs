using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ducode.Wolk.Persistence.Migrations
{
    public partial class AddedAccessTokens : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "access_tokens",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Created = table.Column<DateTimeOffset>(nullable: false),
                    Changed = table.Column<DateTimeOffset>(nullable: true),
                    Token = table.Column<string>(maxLength: 300, nullable: true),
                    ExpirationDateTime = table.Column<DateTimeOffset>(nullable: true),
                    AccessTokenType = table.Column<int>(nullable: false),
                    Identifier = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_access_tokens", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_access_tokens_AccessTokenType",
                table: "access_tokens",
                column: "AccessTokenType");

            migrationBuilder.CreateIndex(
                name: "IX_access_tokens_ExpirationDateTime",
                table: "access_tokens",
                column: "ExpirationDateTime");

            migrationBuilder.CreateIndex(
                name: "IX_access_tokens_Identifier",
                table: "access_tokens",
                column: "Identifier");

            migrationBuilder.CreateIndex(
                name: "IX_access_tokens_Token",
                table: "access_tokens",
                column: "Token",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "access_tokens");
        }
    }
}
