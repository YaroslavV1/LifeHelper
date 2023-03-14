using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LifeHelper.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddArchive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArchiveNotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArchiveNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArchiveNotes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArchiveSubNotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ArchiveNoteId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArchiveSubNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArchiveSubNotes_ArchiveNotes_ArchiveNoteId",
                        column: x => x.ArchiveNoteId,
                        principalTable: "ArchiveNotes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArchiveNotes_UserId",
                table: "ArchiveNotes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ArchiveSubNotes_ArchiveNoteId",
                table: "ArchiveSubNotes",
                column: "ArchiveNoteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArchiveSubNotes");

            migrationBuilder.DropTable(
                name: "ArchiveNotes");
        }
    }
}
