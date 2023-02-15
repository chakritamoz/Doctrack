using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Doctrack.Migrations
{
    /// <inheritdoc />
    public partial class InitialDocumentDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DocumentDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DocId = table.Column<string>(name: "Doc_Id", type: "TEXT", nullable: false),
                    EmpId = table.Column<int>(name: "Emp_Id", type: "INTEGER", nullable: false),
                    Remark = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentDetails_Documents_Doc_Id",
                        column: x => x.DocId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DocumentDetails_Employees_Emp_Id",
                        column: x => x.EmpId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentDetails_Doc_Id",
                table: "DocumentDetails",
                column: "Doc_Id");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentDetails_Emp_Id",
                table: "DocumentDetails",
                column: "Emp_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentDetails");
        }
    }
}
