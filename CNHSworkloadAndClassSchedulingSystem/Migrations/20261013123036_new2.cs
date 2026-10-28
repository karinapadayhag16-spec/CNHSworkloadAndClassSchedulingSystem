using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CNHSworkloadAndClassSchedulingSystem.Migrations
{
    /// <inheritdoc />
    public partial class new2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassSection_SchoolYear_SchoolYearID",
                table: "ClassSection");

            migrationBuilder.DropForeignKey(
                name: "FK_Teacher_SchoolYear_SchoolYearID",
                table: "Teacher");

            migrationBuilder.DropForeignKey(
                name: "FK_Term_SchoolYear_SchoolYearID",
                table: "Term");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SchoolYear",
                table: "SchoolYear");

            migrationBuilder.RenameTable(
                name: "SchoolYear",
                newName: "SchoolYears");

            migrationBuilder.RenameIndex(
                name: "IX_SchoolYear_YearLabel",
                table: "SchoolYears",
                newName: "IX_SchoolYears_YearLabel");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SchoolYears",
                table: "SchoolYears",
                column: "SchoolYearId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClassSection_SchoolYears_SchoolYearID",
                table: "ClassSection",
                column: "SchoolYearID",
                principalTable: "SchoolYears",
                principalColumn: "SchoolYearId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Teacher_SchoolYears_SchoolYearID",
                table: "Teacher",
                column: "SchoolYearID",
                principalTable: "SchoolYears",
                principalColumn: "SchoolYearId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Term_SchoolYears_SchoolYearID",
                table: "Term",
                column: "SchoolYearID",
                principalTable: "SchoolYears",
                principalColumn: "SchoolYearId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassSection_SchoolYears_SchoolYearID",
                table: "ClassSection");

            migrationBuilder.DropForeignKey(
                name: "FK_Teacher_SchoolYears_SchoolYearID",
                table: "Teacher");

            migrationBuilder.DropForeignKey(
                name: "FK_Term_SchoolYears_SchoolYearID",
                table: "Term");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SchoolYears",
                table: "SchoolYears");

            migrationBuilder.RenameTable(
                name: "SchoolYears",
                newName: "SchoolYear");

            migrationBuilder.RenameIndex(
                name: "IX_SchoolYears_YearLabel",
                table: "SchoolYear",
                newName: "IX_SchoolYear_YearLabel");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SchoolYear",
                table: "SchoolYear",
                column: "SchoolYearId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClassSection_SchoolYear_SchoolYearID",
                table: "ClassSection",
                column: "SchoolYearID",
                principalTable: "SchoolYear",
                principalColumn: "SchoolYearId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Teacher_SchoolYear_SchoolYearID",
                table: "Teacher",
                column: "SchoolYearID",
                principalTable: "SchoolYear",
                principalColumn: "SchoolYearId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Term_SchoolYear_SchoolYearID",
                table: "Term",
                column: "SchoolYearID",
                principalTable: "SchoolYear",
                principalColumn: "SchoolYearId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
