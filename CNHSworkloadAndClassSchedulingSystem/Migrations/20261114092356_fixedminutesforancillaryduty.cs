using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CNHSworkloadAndClassSchedulingSystem.Migrations
{
    /// <inheritdoc />
    public partial class fixedminutesforancillaryduty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FixedWeeklyMinutes",
                table: "AncillaryDuties",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FixedWeeklyMinutes",
                table: "AncillaryDuties");
        }
    }
}
