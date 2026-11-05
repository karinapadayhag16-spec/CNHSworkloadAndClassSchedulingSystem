using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CNHSworkloadAndClassSchedulingSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddSchedulesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedule_SubjectOffering_SubjectOfferingID",
                table: "Schedule");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedule_Teacher_TeacherID",
                table: "Schedule");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedule_TimePeriods_TimePeriodID",
                table: "Schedule");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Schedule",
                table: "Schedule");

            migrationBuilder.RenameTable(
                name: "Schedule",
                newName: "Schedules");

            migrationBuilder.RenameIndex(
                name: "IX_Schedule_TimePeriodID",
                table: "Schedules",
                newName: "IX_Schedules_TimePeriodID");

            migrationBuilder.RenameIndex(
                name: "IX_Schedule_TeacherID_TimePeriodID_DayOfWeek",
                table: "Schedules",
                newName: "IX_Schedules_TeacherID_TimePeriodID_DayOfWeek");

            migrationBuilder.RenameIndex(
                name: "IX_Schedule_SubjectOfferingID",
                table: "Schedules",
                newName: "IX_Schedules_SubjectOfferingID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Schedules",
                table: "Schedules",
                column: "ScheduleID");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_SubjectOffering_SubjectOfferingID",
                table: "Schedules",
                column: "SubjectOfferingID",
                principalTable: "SubjectOffering",
                principalColumn: "SubjectOfferingID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_Teacher_TeacherID",
                table: "Schedules",
                column: "TeacherID",
                principalTable: "Teacher",
                principalColumn: "TeacherID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_TimePeriods_TimePeriodID",
                table: "Schedules",
                column: "TimePeriodID",
                principalTable: "TimePeriods",
                principalColumn: "TimePeriodId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_SubjectOffering_SubjectOfferingID",
                table: "Schedules");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_Teacher_TeacherID",
                table: "Schedules");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_TimePeriods_TimePeriodID",
                table: "Schedules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Schedules",
                table: "Schedules");

            migrationBuilder.RenameTable(
                name: "Schedules",
                newName: "Schedule");

            migrationBuilder.RenameIndex(
                name: "IX_Schedules_TimePeriodID",
                table: "Schedule",
                newName: "IX_Schedule_TimePeriodID");

            migrationBuilder.RenameIndex(
                name: "IX_Schedules_TeacherID_TimePeriodID_DayOfWeek",
                table: "Schedule",
                newName: "IX_Schedule_TeacherID_TimePeriodID_DayOfWeek");

            migrationBuilder.RenameIndex(
                name: "IX_Schedules_SubjectOfferingID",
                table: "Schedule",
                newName: "IX_Schedule_SubjectOfferingID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Schedule",
                table: "Schedule",
                column: "ScheduleID");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedule_SubjectOffering_SubjectOfferingID",
                table: "Schedule",
                column: "SubjectOfferingID",
                principalTable: "SubjectOffering",
                principalColumn: "SubjectOfferingID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedule_Teacher_TeacherID",
                table: "Schedule",
                column: "TeacherID",
                principalTable: "Teacher",
                principalColumn: "TeacherID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedule_TimePeriods_TimePeriodID",
                table: "Schedule",
                column: "TimePeriodID",
                principalTable: "TimePeriods",
                principalColumn: "TimePeriodId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
