using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CNHSworkloadAndClassSchedulingSystem.Migrations
{
    /// <inheritdoc />
    public partial class CreateTermsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubjectOffering_Term_TermID",
                table: "SubjectOffering");

            migrationBuilder.DropForeignKey(
                name: "FK_Term_SchoolYears_SchoolYearID",
                table: "Term");

            migrationBuilder.DropForeignKey(
                name: "FK_TimePeriod_Term_TermID",
                table: "TimePeriod");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Term",
                table: "Term");

            migrationBuilder.RenameTable(
                name: "Term",
                newName: "Terms");

            migrationBuilder.RenameIndex(
                name: "IX_Term_SchoolYearID_TermNumber",
                table: "Terms",
                newName: "IX_Terms_SchoolYearID_TermNumber");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Terms",
                table: "Terms",
                column: "TermID");

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectOffering_Terms_TermID",
                table: "SubjectOffering",
                column: "TermID",
                principalTable: "Terms",
                principalColumn: "TermID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Terms_SchoolYears_SchoolYearID",
                table: "Terms",
                column: "SchoolYearID",
                principalTable: "SchoolYears",
                principalColumn: "SchoolYearId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TimePeriod_Terms_TermID",
                table: "TimePeriod",
                column: "TermID",
                principalTable: "Terms",
                principalColumn: "TermID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubjectOffering_Terms_TermID",
                table: "SubjectOffering");

            migrationBuilder.DropForeignKey(
                name: "FK_Terms_SchoolYears_SchoolYearID",
                table: "Terms");

            migrationBuilder.DropForeignKey(
                name: "FK_TimePeriod_Terms_TermID",
                table: "TimePeriod");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Terms",
                table: "Terms");

            migrationBuilder.RenameTable(
                name: "Terms",
                newName: "Term");

            migrationBuilder.RenameIndex(
                name: "IX_Terms_SchoolYearID_TermNumber",
                table: "Term",
                newName: "IX_Term_SchoolYearID_TermNumber");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Term",
                table: "Term",
                column: "TermID");

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectOffering_Term_TermID",
                table: "SubjectOffering",
                column: "TermID",
                principalTable: "Term",
                principalColumn: "TermID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Term_SchoolYears_SchoolYearID",
                table: "Term",
                column: "SchoolYearID",
                principalTable: "SchoolYears",
                principalColumn: "SchoolYearId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TimePeriod_Term_TermID",
                table: "TimePeriod",
                column: "TermID",
                principalTable: "Term",
                principalColumn: "TermID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
