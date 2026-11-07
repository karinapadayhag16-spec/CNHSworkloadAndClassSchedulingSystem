using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CNHSworkloadAndClassSchedulingSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddClassSections : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassSection_ClassSection_PreviousClassSectionID",
                table: "ClassSection");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassSection_SchoolYears_SchoolYearID",
                table: "ClassSection");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassSection_Teacher_AdviserID",
                table: "ClassSection");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassSectionSchedules_ClassSection_ClassSectionID",
                table: "ClassSectionSchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_Teacher_TeacherID",
                table: "Schedules");

            migrationBuilder.DropForeignKey(
                name: "FK_SubjectOffering_ClassSection_ClassSectionID",
                table: "SubjectOffering");

            migrationBuilder.DropForeignKey(
                name: "FK_Teacher_SchoolYears_SchoolYearID",
                table: "Teacher");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherExpertise_Teacher_TeacherID",
                table: "TeacherExpertise");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Teacher",
                table: "Teacher");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClassSection",
                table: "ClassSection");

            migrationBuilder.RenameTable(
                name: "Teacher",
                newName: "Teachers");

            migrationBuilder.RenameTable(
                name: "ClassSection",
                newName: "ClassSections");

            migrationBuilder.RenameIndex(
                name: "IX_Teacher_SchoolYearID",
                table: "Teachers",
                newName: "IX_Teachers_SchoolYearID");

            migrationBuilder.RenameIndex(
                name: "IX_ClassSection_SchoolYearID",
                table: "ClassSections",
                newName: "IX_ClassSections_SchoolYearID");

            migrationBuilder.RenameIndex(
                name: "IX_ClassSection_PreviousClassSectionID",
                table: "ClassSections",
                newName: "IX_ClassSections_PreviousClassSectionID");

            migrationBuilder.RenameIndex(
                name: "IX_ClassSection_AdviserID",
                table: "ClassSections",
                newName: "IX_ClassSections_AdviserID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Teachers",
                table: "Teachers",
                column: "TeacherID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClassSections",
                table: "ClassSections",
                column: "ClassSectionID");

            migrationBuilder.AddForeignKey(
                name: "FK_ClassSections_ClassSections_PreviousClassSectionID",
                table: "ClassSections",
                column: "PreviousClassSectionID",
                principalTable: "ClassSections",
                principalColumn: "ClassSectionID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassSections_SchoolYears_SchoolYearID",
                table: "ClassSections",
                column: "SchoolYearID",
                principalTable: "SchoolYears",
                principalColumn: "SchoolYearId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassSections_Teachers_AdviserID",
                table: "ClassSections",
                column: "AdviserID",
                principalTable: "Teachers",
                principalColumn: "TeacherID");

            migrationBuilder.AddForeignKey(
                name: "FK_ClassSectionSchedules_ClassSections_ClassSectionID",
                table: "ClassSectionSchedules",
                column: "ClassSectionID",
                principalTable: "ClassSections",
                principalColumn: "ClassSectionID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_Teachers_TeacherID",
                table: "Schedules",
                column: "TeacherID",
                principalTable: "Teachers",
                principalColumn: "TeacherID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectOffering_ClassSections_ClassSectionID",
                table: "SubjectOffering",
                column: "ClassSectionID",
                principalTable: "ClassSections",
                principalColumn: "ClassSectionID",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassSections_ClassSections_PreviousClassSectionID",
                table: "ClassSections");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassSections_SchoolYears_SchoolYearID",
                table: "ClassSections");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassSections_Teachers_AdviserID",
                table: "ClassSections");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassSectionSchedules_ClassSections_ClassSectionID",
                table: "ClassSectionSchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_Teachers_TeacherID",
                table: "Schedules");

            migrationBuilder.DropForeignKey(
                name: "FK_SubjectOffering_ClassSections_ClassSectionID",
                table: "SubjectOffering");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherExpertise_Teachers_TeacherID",
                table: "TeacherExpertise");

            migrationBuilder.DropForeignKey(
                name: "FK_Teachers_SchoolYears_SchoolYearID",
                table: "Teachers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Teachers",
                table: "Teachers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClassSections",
                table: "ClassSections");

            migrationBuilder.RenameTable(
                name: "Teachers",
                newName: "Teacher");

            migrationBuilder.RenameTable(
                name: "ClassSections",
                newName: "ClassSection");

            migrationBuilder.RenameIndex(
                name: "IX_Teachers_SchoolYearID",
                table: "Teacher",
                newName: "IX_Teacher_SchoolYearID");

            migrationBuilder.RenameIndex(
                name: "IX_ClassSections_SchoolYearID",
                table: "ClassSection",
                newName: "IX_ClassSection_SchoolYearID");

            migrationBuilder.RenameIndex(
                name: "IX_ClassSections_PreviousClassSectionID",
                table: "ClassSection",
                newName: "IX_ClassSection_PreviousClassSectionID");

            migrationBuilder.RenameIndex(
                name: "IX_ClassSections_AdviserID",
                table: "ClassSection",
                newName: "IX_ClassSection_AdviserID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Teacher",
                table: "Teacher",
                column: "TeacherID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClassSection",
                table: "ClassSection",
                column: "ClassSectionID");

            migrationBuilder.AddForeignKey(
                name: "FK_ClassSection_ClassSection_PreviousClassSectionID",
                table: "ClassSection",
                column: "PreviousClassSectionID",
                principalTable: "ClassSection",
                principalColumn: "ClassSectionID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassSection_SchoolYears_SchoolYearID",
                table: "ClassSection",
                column: "SchoolYearID",
                principalTable: "SchoolYears",
                principalColumn: "SchoolYearId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassSection_Teacher_AdviserID",
                table: "ClassSection",
                column: "AdviserID",
                principalTable: "Teacher",
                principalColumn: "TeacherID");

            migrationBuilder.AddForeignKey(
                name: "FK_ClassSectionSchedules_ClassSection_ClassSectionID",
                table: "ClassSectionSchedules",
                column: "ClassSectionID",
                principalTable: "ClassSection",
                principalColumn: "ClassSectionID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_Teacher_TeacherID",
                table: "Schedules",
                column: "TeacherID",
                principalTable: "Teacher",
                principalColumn: "TeacherID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectOffering_ClassSection_ClassSectionID",
                table: "SubjectOffering",
                column: "ClassSectionID",
                principalTable: "ClassSection",
                principalColumn: "ClassSectionID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Teacher_SchoolYears_SchoolYearID",
                table: "Teacher",
                column: "SchoolYearID",
                principalTable: "SchoolYears",
                principalColumn: "SchoolYearId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherExpertise_Teacher_TeacherID",
                table: "TeacherExpertise",
                column: "TeacherID",
                principalTable: "Teacher",
                principalColumn: "TeacherID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
