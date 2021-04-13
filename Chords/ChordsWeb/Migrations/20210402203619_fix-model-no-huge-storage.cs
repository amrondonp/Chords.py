using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ChordsWeb.Migrations
{
    public partial class fixmodelnohugestorage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Pcp",
                table: "ChordWithKey");

            migrationBuilder.DropColumn(
                name: "Samples",
                table: "ChordWithKey");

            migrationBuilder.AddColumn<bool>(
                name: "AutoBorder",
                table: "Predictions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "Predictions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModelName",
                table: "Predictions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WindowInMs",
                table: "Predictions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SampleLength",
                table: "ChordWithKey",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AutoBorder",
                table: "Predictions");

            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "Predictions");

            migrationBuilder.DropColumn(
                name: "ModelName",
                table: "Predictions");

            migrationBuilder.DropColumn(
                name: "WindowInMs",
                table: "Predictions");

            migrationBuilder.DropColumn(
                name: "SampleLength",
                table: "ChordWithKey");

            migrationBuilder.AddColumn<double[]>(
                name: "Pcp",
                table: "ChordWithKey",
                type: "double precision[]",
                nullable: true);

            migrationBuilder.AddColumn<float[]>(
                name: "Samples",
                table: "ChordWithKey",
                type: "real[]",
                nullable: true);
        }
    }
}
