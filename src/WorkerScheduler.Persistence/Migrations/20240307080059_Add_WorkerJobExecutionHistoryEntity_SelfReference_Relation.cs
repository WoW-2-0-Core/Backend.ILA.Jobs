using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkerScheduler.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_WorkerJobExecutionHistoryEntity_SelfReference_Relation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ExecutionTime",
                table: "JobExecutionHistories",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<byte>(
                name: "RetryCount",
                table: "JobExecutionHistories",
                type: "smallint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<Guid>(
                name: "RetryForHistoryId",
                table: "JobExecutionHistories",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_JobExecutionHistories_RetryForHistoryId",
                table: "JobExecutionHistories",
                column: "RetryForHistoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobExecutionHistories_JobExecutionHistories_RetryForHistory~",
                table: "JobExecutionHistories",
                column: "RetryForHistoryId",
                principalTable: "JobExecutionHistories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobExecutionHistories_JobExecutionHistories_RetryForHistory~",
                table: "JobExecutionHistories");

            migrationBuilder.DropIndex(
                name: "IX_JobExecutionHistories_RetryForHistoryId",
                table: "JobExecutionHistories");

            migrationBuilder.DropColumn(
                name: "ExecutionTime",
                table: "JobExecutionHistories");

            migrationBuilder.DropColumn(
                name: "RetryCount",
                table: "JobExecutionHistories");

            migrationBuilder.DropColumn(
                name: "RetryForHistoryId",
                table: "JobExecutionHistories");
        }
    }
}
