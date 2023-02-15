using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Doctrack.Migrations
{
    /// <inheritdoc />
    public partial class ICollectioncanbenull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Jobs_Job_Id",
                table: "Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Ranks_Rank_Id",
                table: "Employee");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Employee",
                table: "Employee");

            migrationBuilder.RenameTable(
                name: "Employee",
                newName: "Employees");

            migrationBuilder.RenameIndex(
                name: "IX_Employee_Rank_Id",
                table: "Employees",
                newName: "IX_Employees_Rank_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Employee_Job_Id",
                table: "Employees",
                newName: "IX_Employees_Job_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Employees",
                table: "Employees",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Jobs_Job_Id",
                table: "Employees",
                column: "Job_Id",
                principalTable: "Jobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Ranks_Rank_Id",
                table: "Employees",
                column: "Rank_Id",
                principalTable: "Ranks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Jobs_Job_Id",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Ranks_Rank_Id",
                table: "Employees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Employees",
                table: "Employees");

            migrationBuilder.RenameTable(
                name: "Employees",
                newName: "Employee");

            migrationBuilder.RenameIndex(
                name: "IX_Employees_Rank_Id",
                table: "Employee",
                newName: "IX_Employee_Rank_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Employees_Job_Id",
                table: "Employee",
                newName: "IX_Employee_Job_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Employee",
                table: "Employee",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Jobs_Job_Id",
                table: "Employee",
                column: "Job_Id",
                principalTable: "Jobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Ranks_Rank_Id",
                table: "Employee",
                column: "Rank_Id",
                principalTable: "Ranks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
