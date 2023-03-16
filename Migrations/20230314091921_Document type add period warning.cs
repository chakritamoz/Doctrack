using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Doctrack.Migrations
{
    /// <inheritdoc />
    public partial class Documenttypeaddperiodwarning : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Period",
                table: "DocumentTypes");

            migrationBuilder.AddColumn<int>(
                name: "PeriodEnd",
                table: "DocumentTypes",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PeriodWarning",
                table: "DocumentTypes",
                type: "INTEGER",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PeriodEnd",
                table: "DocumentTypes");

            migrationBuilder.DropColumn(
                name: "PeriodWarning",
                table: "DocumentTypes");

            migrationBuilder.AddColumn<int>(
                name: "Period",
                table: "DocumentTypes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
