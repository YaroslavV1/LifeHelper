using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LifeHelper.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangedSubNote : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subnotes_Notes_NoteId",
                table: "Subnotes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Subnotes",
                table: "Subnotes");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Subnotes");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Subnotes");

            migrationBuilder.RenameTable(
                name: "Subnotes",
                newName: "SubNotes");

            migrationBuilder.RenameIndex(
                name: "IX_Subnotes_NoteId",
                table: "SubNotes",
                newName: "IX_SubNotes_NoteId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubNotes",
                table: "SubNotes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SubNotes_Notes_NoteId",
                table: "SubNotes",
                column: "NoteId",
                principalTable: "Notes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubNotes_Notes_NoteId",
                table: "SubNotes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubNotes",
                table: "SubNotes");

            migrationBuilder.RenameTable(
                name: "SubNotes",
                newName: "Subnotes");

            migrationBuilder.RenameIndex(
                name: "IX_SubNotes_NoteId",
                table: "Subnotes",
                newName: "IX_Subnotes_NoteId");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Subnotes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Subnotes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subnotes",
                table: "Subnotes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Subnotes_Notes_NoteId",
                table: "Subnotes",
                column: "NoteId",
                principalTable: "Notes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
