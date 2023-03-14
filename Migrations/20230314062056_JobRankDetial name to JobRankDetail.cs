using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Doctrack.Migrations
{
    /// <inheritdoc />
    public partial class JobRankDetialnametoJobRankDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobRankDetial");

            migrationBuilder.CreateTable(
                name: "JobRankDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    JobId = table.Column<int>(name: "Job_Id", type: "INTEGER", nullable: false),
                    RankId = table.Column<int>(name: "Rank_Id", type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobRankDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobRankDetails_Jobs_Job_Id",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobRankDetails_Ranks_Rank_Id",
                        column: x => x.RankId,
                        principalTable: "Ranks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobRankDetails_Job_Id",
                table: "JobRankDetails",
                column: "Job_Id");

            migrationBuilder.CreateIndex(
                name: "IX_JobRankDetails_Rank_Id",
                table: "JobRankDetails",
                column: "Rank_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobRankDetails");

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
                name: "IX_JobRankDetial_Job_Id",
                table: "JobRankDetial",
                column: "Job_Id");

            migrationBuilder.CreateIndex(
                name: "IX_JobRankDetial_Rank_Id",
                table: "JobRankDetial",
                column: "Rank_Id");
        }
    }
}
