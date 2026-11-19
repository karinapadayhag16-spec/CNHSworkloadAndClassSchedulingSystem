using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CNHSworkloadAndClassSchedulingSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddTeacherSchoolYearAssignmentAndAncillaryDuties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeacherExpertise_Subject_SubjectID",
                table: "TeacherExpertise");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherExpertise_Teachers_TeacherID",
                table: "TeacherExpertise");

            migrationBuilder.DropForeignKey(
                name: "FK_Teachers_SchoolYears_SchoolYearID",
                table: "Teachers");

            migrationBuilder.DropIndex(
                name: "IX_Teachers_SchoolYearID",
                table: "Teachers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TeacherExpertise",
                table: "TeacherExpertise");

            migrationBuilder.DropColumn(
                name: "Department",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "SchoolYearID",
                table: "Teachers");

            migrationBuilder.RenameTable(
                name: "TeacherExpertise",
                newName: "TeacherExpertises");

            migrationBuilder.RenameIndex(
                name: "IX_TeacherExpertise_TeacherID_SubjectID",
                table: "TeacherExpertises",
                newName: "IX_TeacherExpertises_TeacherID_SubjectID");

            migrationBuilder.RenameIndex(
                name: "IX_TeacherExpertise_SubjectID",
                table: "TeacherExpertises",
                newName: "IX_TeacherExpertises_SubjectID");

            migrationBuilder.AddColumn<bool>(
                name: "IsArchived",
                table: "Teachers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeacherExpertises",
                table: "TeacherExpertises",
                column: "TeacherExpertiseID");

            migrationBuilder.CreateTable(
                name: "AncillaryDuties",
                columns: table => new
                {
                    AncillaryDutyID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DutyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CountsTowardWorkload = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AncillaryDuties", x => x.AncillaryDutyID);
                });

            migrationBuilder.CreateTable(
                name: "TeacherSchoolYearAssignments",
                columns: table => new
                {
                    TeacherAssignmentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeacherID = table.Column<int>(type: "int", nullable: false),
                    SchoolYearID = table.Column<int>(type: "int", nullable: false),
                    Department = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherSchoolYearAssignments", x => x.TeacherAssignmentID);
                    table.ForeignKey(
                        name: "FK_TeacherSchoolYearAssignments_SchoolYears_SchoolYearID",
                        column: x => x.SchoolYearID,
                        principalTable: "SchoolYears",
                        principalColumn: "SchoolYearId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeacherSchoolYearAssignments_Teachers_TeacherID",
                        column: x => x.TeacherID,
                        principalTable: "Teachers",
                        principalColumn: "TeacherID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TeacherAncillaryDuties",
                columns: table => new
                {
                    TeacherAncillaryDutyID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeacherAssignmentID = table.Column<int>(type: "int", nullable: false),
                    AncillaryDutyID = table.Column<int>(type: "int", nullable: false),
                    WeeklyMinutes = table.Column<int>(type: "int", nullable: false),
                    CountsTowardWorkload = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherAncillaryDuties", x => x.TeacherAncillaryDutyID);
                    table.ForeignKey(
                        name: "FK_TeacherAncillaryDuties_AncillaryDuties_AncillaryDutyID",
                        column: x => x.AncillaryDutyID,
                        principalTable: "AncillaryDuties",
                        principalColumn: "AncillaryDutyID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeacherAncillaryDuties_TeacherSchoolYearAssignments_TeacherAssignmentID",
                        column: x => x.TeacherAssignmentID,
                        principalTable: "TeacherSchoolYearAssignments",
                        principalColumn: "TeacherAssignmentID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TeacherAncillaryDuties_AncillaryDutyID",
                table: "TeacherAncillaryDuties",
                column: "AncillaryDutyID");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherAncillaryDuties_TeacherAssignmentID_AncillaryDutyID",
                table: "TeacherAncillaryDuties",
                columns: new[] { "TeacherAssignmentID", "AncillaryDutyID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TeacherSchoolYearAssignments_SchoolYearID",
                table: "TeacherSchoolYearAssignments",
                column: "SchoolYearID");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherSchoolYearAssignments_TeacherID_SchoolYearID",
                table: "TeacherSchoolYearAssignments",
                columns: new[] { "TeacherID", "SchoolYearID" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherExpertises_Subject_SubjectID",
                table: "TeacherExpertises",
                column: "SubjectID",
                principalTable: "Subject",
                principalColumn: "SubjectID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherExpertises_Teachers_TeacherID",
                table: "TeacherExpertises",
                column: "TeacherID",
                principalTable: "Teachers",
                principalColumn: "TeacherID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeacherExpertises_Subject_SubjectID",
                table: "TeacherExpertises");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherExpertises_Teachers_TeacherID",
                table: "TeacherExpertises");

            migrationBuilder.DropTable(
                name: "TeacherAncillaryDuties");

            migrationBuilder.DropTable(
                name: "AncillaryDuties");

            migrationBuilder.DropTable(
                name: "TeacherSchoolYearAssignments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TeacherExpertises",
                table: "TeacherExpertises");

            migrationBuilder.DropColumn(
                name: "IsArchived",
                table: "Teachers");

            migrationBuilder.RenameTable(
                name: "TeacherExpertises",
                newName: "TeacherExpertise");

            migrationBuilder.RenameIndex(
                name: "IX_TeacherExpertises_TeacherID_SubjectID",
                table: "TeacherExpertise",
                newName: "IX_TeacherExpertise_TeacherID_SubjectID");

            migrationBuilder.RenameIndex(
                name: "IX_TeacherExpertises_SubjectID",
                table: "TeacherExpertise",
                newName: "IX_TeacherExpertise_SubjectID");

            migrationBuilder.AddColumn<int>(
                name: "Department",
                table: "Teachers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SchoolYearID",
                table: "Teachers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeacherExpertise",
                table: "TeacherExpertise",
                column: "TeacherExpertiseID");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_SchoolYearID",
                table: "Teachers",
                column: "SchoolYearID");

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherExpertise_Subject_SubjectID",
                table: "TeacherExpertise",
                column: "SubjectID",
                principalTable: "Subject",
                principalColumn: "SubjectID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherExpertise_Teachers_TeacherID",
                table: "TeacherExpertise",
                column: "TeacherID",
                principalTable: "Teachers",
                principalColumn: "TeacherID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Teachers_SchoolYears_SchoolYearID",
                table: "Teachers",
                column: "SchoolYearID",
                principalTable: "SchoolYears",
                principalColumn: "SchoolYearId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
