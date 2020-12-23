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
                name: "equipment_hourly_rates",
                columns: table => new
                {
                    equipment_no = table.Column<string>(type: "text", nullable: false),
                    equipment_name = table.Column<string>(type: "text", nullable: false),
                    machine_category = table.Column<string>(type: "text", nullable: false),
                    hourly_rate = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_equipment_hourly_rates", x => x.equipment_no);
                });

            migrationBuilder.CreateTable(
                name: "projects",
                columns: table => new
                {
                    no = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    full_name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_projects", x => x.no);
                });

            migrationBuilder.CreateTable(
                name: "usage_records",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    unique_token = table.Column<Guid>(type: "uuid", nullable: false),
                    user = table.Column<string>(type: "text", nullable: false),
                    test_no = table.Column<string>(type: "text", nullable: false),
                    equipment_no = table.Column<string>(type: "text", nullable: false),
                    test_type = table.Column<string>(type: "text", nullable: true),
                    project_no = table.Column<string>(type: "text", nullable: false),
                    start_time = table.Column<long>(type: "bigint", nullable: false),
                    end_time = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_usage_records", x => x.id);
                    table.ForeignKey(
                        name: "fk_usage_records_equipment_hourly_rates_equipment_no",
                        column: x => x.equipment_no,
                        principalTable: "equipment_hourly_rates",
                        principalColumn: "equipment_no",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_usage_records_projects_project_no",
                        column: x => x.project_no,
                        principalTable: "projects",
                        principalColumn: "no",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_projects_full_name",
                table: "projects",
                column: "full_name");

            migrationBuilder.CreateIndex(
                name: "ix_usage_records_equipment_no",
                table: "usage_records",
                column: "equipment_no")
                .Annotation("Npgsql:IndexInclude", new[] { "test_type" });

            migrationBuilder.CreateIndex(
                name: "ix_usage_records_project_no",
                table: "usage_records",
                column: "project_no");

            migrationBuilder.CreateIndex(
                name: "ix_usage_records_start_time",
                table: "usage_records",
                column: "start_time");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "usage_records");

            migrationBuilder.DropTable(
                name: "equipment_hourly_rates");

            migrationBuilder.DropTable(
                name: "projects");
        }
    }
}
