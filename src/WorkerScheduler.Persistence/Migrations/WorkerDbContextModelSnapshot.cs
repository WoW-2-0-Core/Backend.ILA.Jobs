﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using WorkerScheduler.Persistence.DataContexts;

#nullable disable

namespace WorkerScheduler.Persistence.Migrations
{
    [DbContext(typeof(WorkerDbContext))]
    partial class WorkerDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("WorkerScheduler.Domain.Entities.WorkerJobEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("CronSchedule")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasDefaultValue("0 0 * * *");

                    b.Property<string>("JobType")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("SkipMissedScheduledTime")
                        .HasColumnType("boolean");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.HasKey("Id");

                    b.HasIndex("JobType")
                        .IsUnique();

                    b.ToTable("Jobs");
                });

            modelBuilder.Entity("WorkerScheduler.Domain.Entities.WorkerJobExecutionHistoryEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("ExecutionTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsSuccessful")
                        .HasColumnType("boolean");

                    b.Property<Guid>("JobId")
                        .HasColumnType("uuid");

                    b.Property<byte>("RetryCount")
                        .HasColumnType("smallint");

                    b.Property<Guid?>("RetryForHistoryId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("JobId");

                    b.HasIndex("RetryForHistoryId");

                    b.ToTable("JobExecutionHistories");
                });

            modelBuilder.Entity("WorkerScheduler.Domain.Entities.WorkerJobExecutionHistoryEntity", b =>
                {
                    b.HasOne("WorkerScheduler.Domain.Entities.WorkerJobEntity", "Job")
                        .WithMany("ExecutionHistories")
                        .HasForeignKey("JobId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WorkerScheduler.Domain.Entities.WorkerJobExecutionHistoryEntity", null)
                        .WithMany("RetryHistories")
                        .HasForeignKey("RetryForHistoryId");

                    b.Navigation("Job");
                });

            modelBuilder.Entity("WorkerScheduler.Domain.Entities.WorkerJobEntity", b =>
                {
                    b.Navigation("ExecutionHistories");
                });

            modelBuilder.Entity("WorkerScheduler.Domain.Entities.WorkerJobExecutionHistoryEntity", b =>
                {
                    b.Navigation("RetryHistories");
                });
#pragma warning restore 612, 618
        }
    }
}
