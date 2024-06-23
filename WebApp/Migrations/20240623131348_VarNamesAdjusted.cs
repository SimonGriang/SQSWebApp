using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Migrations
{
    /// <inheritdoc />
    public partial class VarNamesAdjusted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "translated_at",
                table: "Translation",
                newName: "Translated_at");

            migrationBuilder.RenameColumn(
                name: "isTargetLanguage",
                table: "Language",
                newName: "IsTargetLanguage");

            migrationBuilder.RenameColumn(
                name: "isOriginLanguage",
                table: "Language",
                newName: "IsOriginLanguage");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Translated_at",
                table: "Translation",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Translated_at",
                table: "Translation",
                newName: "translated_at");

            migrationBuilder.RenameColumn(
                name: "IsTargetLanguage",
                table: "Language",
                newName: "isTargetLanguage");

            migrationBuilder.RenameColumn(
                name: "IsOriginLanguage",
                table: "Language",
                newName: "isOriginLanguage");

            migrationBuilder.AlterColumn<DateTime>(
                name: "translated_at",
                table: "Translation",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);
        }
    }
}
