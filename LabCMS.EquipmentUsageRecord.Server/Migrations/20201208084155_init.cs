using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

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
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    No = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    FullName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
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
                    ProjectId = table.Column<int>(type: "integer", nullable: false),
                    StartTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    EndTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
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
                        name: "FK_UsageRecords_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Projects_FullName",
                table: "Projects",
                column: "FullName");

            migrationBuilder.CreateIndex(
                name: "IX_UsageRecords_EquipmentNo",
                table: "UsageRecords",
                column: "EquipmentNo")
                .Annotation("Npgsql:IndexInclude", new[] { "TestType" });

            migrationBuilder.CreateIndex(
                name: "IX_UsageRecords_ProjectId",
                table: "UsageRecords",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_UsageRecords_StartTime",
                table: "UsageRecords",
                column: "StartTime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsageRecords");

            migrationBuilder.DropTable(
                name: "EquipmentHourlyRates");

            migrationBuilder.DropTable(
                name: "Projects");
        }
    }
}
