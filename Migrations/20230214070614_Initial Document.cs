using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Doctrack.Migrations
{
    /// <inheritdoc />
    public partial class InitialDocument : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    DocTypeId = table.Column<int>(name: "DocType_Id", type: "INTEGER", nullable: false),
                    ReceiptDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Operation = table.Column<string>(type: "TEXT", nullable: true),
                    OperationDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CommandOrder = table.Column<string>(type: "TEXT", nullable: true),
                    RemarkAll = table.Column<string>(type: "TEXT", nullable: true),
                    User = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Documents_DocumentTypes_DocType_Id",
                        column: x => x.DocTypeId,
                        principalTable: "DocumentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Documents_DocType_Id",
                table: "Documents",
                column: "DocType_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Documents");
        }
    }
}
