using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkerScheduler.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_WorkerJob_And_WorkerJobExecutionHistory_Relation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JobExecutionHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    JobId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsSuccessful = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobExecutionHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobExecutionHistories_Jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobExecutionHistories_JobId",
                table: "JobExecutionHistories",
                column: "JobId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobExecutionHistories");
        }
    }
}
