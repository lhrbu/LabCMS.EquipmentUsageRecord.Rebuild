﻿using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LabCMS.EquipmentUsageRecord.Server.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EquipmentHourlyRates",
                columns: table => new
                {
                    EquipmentNo = table.Column<string>(type: "text", nullable: false),
                    EquipmentName = table.Column<string>(type: "text", nullable: false),
                    MachineCategory = table.Column<string>(type: "text", nullable: false),
                    HourlyRate = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentHourlyRates", x => x.EquipmentNo);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    No = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    NameInFIN = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.No);
                });

            migrationBuilder.CreateTable(
                name: "ActiveProjectIndices",
                columns: table => new
                {
                    No = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActiveProjectIndices", x => x.No);
                    table.ForeignKey(
                        name: "FK_ActiveProjectIndices_Projects_No",
                        column: x => x.No,
                        principalTable: "Projects",
                        principalColumn: "No",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsageRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    User = table.Column<string>(type: "text", nullable: false),
                    TestNo = table.Column<string>(type: "text", nullable: false),
                    EquipmentNo = table.Column<string>(type: "text", nullable: false),
                    TestType = table.Column<string>(type: "text", nullable: true),
                    ProjectNo = table.Column<string>(type: "text", nullable: false),
                    StartTime = table.Column<long>(type: "bigint", nullable: false),
                    EndTime = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsageRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsageRecords_EquipmentHourlyRates_EquipmentNo",
                        column: x => x.EquipmentNo,
                        principalTable: "EquipmentHourlyRates",
                        principalColumn: "EquipmentNo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsageRecords_Projects_ProjectNo",
                        column: x => x.ProjectNo,
                        principalTable: "Projects",
                        principalColumn: "No",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Projects_Name",
                table: "Projects",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UsageRecords_EquipmentNo",
                table: "UsageRecords",
                column: "EquipmentNo")
                .Annotation("Npgsql:IndexInclude", new[] { "TestType" });

            migrationBuilder.CreateIndex(
                name: "IX_UsageRecords_ProjectNo",
                table: "UsageRecords",
                column: "ProjectNo");

            migrationBuilder.CreateIndex(
                name: "IX_UsageRecords_StartTime",
                table: "UsageRecords",
                column: "StartTime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActiveProjectIndices");

            migrationBuilder.DropTable(
                name: "UsageRecords");

            migrationBuilder.DropTable(
                name: "EquipmentHourlyRates");

            migrationBuilder.DropTable(
                name: "Projects");
        }
    }
}
