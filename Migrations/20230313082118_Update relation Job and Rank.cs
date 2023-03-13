using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Doctrack.Migrations
{
    /// <inheritdoc />
    public partial class UpdaterelationJobandRank : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Jobs_Job_Id",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Ranks_Rank_Id",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_Job_Id",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_Rank_Id",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Job_Id",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Rank_Id",
                table: "Employees");

            migrationBuilder.AddColumn<int>(
                name: "Job_Id",
                table: "DocumentDetails",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Rank_Id",
                table: "DocumentDetails",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "JobRankDetial",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    JobId = table.Column<int>(name: "Job_Id", type: "INTEGER", nullable: false),
                    RankId = table.Column<int>(name: "Rank_Id", type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobRankDetial", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobRankDetial_Jobs_Job_Id",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobRankDetial_Ranks_Rank_Id",
                        column: x => x.RankId,
                        principalTable: "Ranks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentDetails_Job_Id",
                table: "DocumentDetails",
                column: "Job_Id");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentDetails_Rank_Id",
                table: "DocumentDetails",
                column: "Rank_Id");

            migrationBuilder.CreateIndex(
                name: "IX_JobRankDetial_Job_Id",
                table: "JobRankDetial",
                column: "Job_Id");

            migrationBuilder.CreateIndex(
                name: "IX_JobRankDetial_Rank_Id",
                table: "JobRankDetial",
                column: "Rank_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentDetails_Jobs_Job_Id",
                table: "DocumentDetails",
                column: "Job_Id",
                principalTable: "Jobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentDetails_Ranks_Rank_Id",
                table: "DocumentDetails",
                column: "Rank_Id",
                principalTable: "Ranks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentDetails_Jobs_Job_Id",
                table: "DocumentDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_DocumentDetails_Ranks_Rank_Id",
                table: "DocumentDetails");

            migrationBuilder.DropTable(
                name: "JobRankDetial");

            migrationBuilder.DropIndex(
                name: "IX_DocumentDetails_Job_Id",
                table: "DocumentDetails");

            migrationBuilder.DropIndex(
                name: "IX_DocumentDetails_Rank_Id",
                table: "DocumentDetails");

            migrationBuilder.DropColumn(
                name: "Job_Id",
                table: "DocumentDetails");

            migrationBuilder.DropColumn(
                name: "Rank_Id",
                table: "DocumentDetails");

            migrationBuilder.AddColumn<int>(
                name: "Job_Id",
                table: "Employees",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Rank_Id",
                table: "Employees",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Job_Id",
                table: "Employees",
                column: "Job_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Rank_Id",
                table: "Employees",
                column: "Rank_Id");

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
    }
}
