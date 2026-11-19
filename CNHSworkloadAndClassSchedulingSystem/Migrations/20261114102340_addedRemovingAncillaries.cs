using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CNHSworkloadAndClassSchedulingSystem.Migrations
{
    /// <inheritdoc />
    public partial class addedRemovingAncillaries : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TeacherAncillaryDuties_TeacherAssignmentID_AncillaryDutyID",
                table: "TeacherAncillaryDuties");

            migrationBuilder.AddColumn<int>(
                name: "TermID",
                table: "TeacherAncillaryDuties",
                type: "int",
                nullable: false,
                defaultValue: 0);
            // Assign existing ancillary duties to Term 1
            // based on the teacher's school-year assignment.
            migrationBuilder.Sql(@"
                UPDATE tad
                SET tad.TermID = t.TermID
                FROM TeacherAncillaryDuties tad
                INNER JOIN TeacherSchoolYearAssignments tsa
                    ON tad.TeacherAssignmentID = tsa.TeacherAssignmentID
                INNER JOIN Terms t
                    ON tsa.SchoolYearID = t.SchoolYearID
                    AND t.TermNumber = 1;
            ");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherAncillaryDuties_TeacherAssignmentID_TermID_AncillaryDutyID",
                table: "TeacherAncillaryDuties",
                columns: new[] { "TeacherAssignmentID", "TermID", "AncillaryDutyID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TeacherAncillaryDuties_TermID",
                table: "TeacherAncillaryDuties",
                column: "TermID");

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherAncillaryDuties_Terms_TermID",
                table: "TeacherAncillaryDuties",
                column: "TermID",
                principalTable: "Terms",
                principalColumn: "TermID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeacherAncillaryDuties_Terms_TermID",
                table: "TeacherAncillaryDuties");

            migrationBuilder.DropIndex(
                name: "IX_TeacherAncillaryDuties_TeacherAssignmentID_TermID_AncillaryDutyID",
                table: "TeacherAncillaryDuties");

            migrationBuilder.DropIndex(
                name: "IX_TeacherAncillaryDuties_TermID",
                table: "TeacherAncillaryDuties");

            migrationBuilder.DropColumn(
                name: "TermID",
                table: "TeacherAncillaryDuties");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherAncillaryDuties_TeacherAssignmentID_AncillaryDutyID",
                table: "TeacherAncillaryDuties",
                columns: new[] { "TeacherAssignmentID", "AncillaryDutyID" },
                unique: true);
        }
    }
}
