using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CNHSworkloadAndClassSchedulingSystem.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTeacherAncillaryDutyTermRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeacherAncillaryDuties_Terms_TermID",
                table: "TeacherAncillaryDuties");

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherAncillaryDuties_Terms_TermID",
                table: "TeacherAncillaryDuties",
                column: "TermID",
                principalTable: "Terms",
                principalColumn: "TermID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeacherAncillaryDuties_Terms_TermID",
                table: "TeacherAncillaryDuties");

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherAncillaryDuties_Terms_TermID",
                table: "TeacherAncillaryDuties",
                column: "TermID",
                principalTable: "Terms",
                principalColumn: "TermID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
