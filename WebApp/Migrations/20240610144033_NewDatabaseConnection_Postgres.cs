using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WebApp.Migrations
{
    /// <inheritdoc />
    public partial class NewDatabaseConnection_Postgres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Language",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Abbreviation = table.Column<string>(type: "text", nullable: true),
                    isTargetLanguage = table.Column<bool>(type: "boolean", nullable: false),
                    isOriginLanguage = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Language", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Translation",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OriginalText = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    TranslatedText = table.Column<string>(type: "text", nullable: true),
                    translated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OriginalLanguageID = table.Column<int>(type: "integer", nullable: true),
                    TranslatedLanguageID = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Translation", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Translation_Language_OriginalLanguageID",
                        column: x => x.OriginalLanguageID,
                        principalTable: "Language",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Translation_Language_TranslatedLanguageID",
                        column: x => x.TranslatedLanguageID,
                        principalTable: "Language",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Translation_OriginalLanguageID",
                table: "Translation",
                column: "OriginalLanguageID");

            migrationBuilder.CreateIndex(
                name: "IX_Translation_TranslatedLanguageID",
                table: "Translation",
                column: "TranslatedLanguageID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Translation");

            migrationBuilder.DropTable(
                name: "Language");
        }
    }
}
