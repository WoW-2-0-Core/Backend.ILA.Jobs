using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkerScheduler.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_SkipMissedScheduledTime_To_WorkerJob : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "SkipMissedScheduledTime",
                table: "Jobs",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SkipMissedScheduledTime",
                table: "Jobs");
        }
    }
}
