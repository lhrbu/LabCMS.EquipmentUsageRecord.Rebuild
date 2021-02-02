using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace LabCMS.EquipmentUsageRecord.MachineDown.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "MachineDownRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    EquipmentNo = table.Column<string>(type: "text", nullable: false),
                    MachineDownDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    MachineRepairedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Comment = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MachineDownRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MachineDownRecords_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotifiedTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NotifiedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    MachineDownRecordId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotifiedTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotifiedTokens_MachineDownRecords_MachineDownRecordId",
                        column: x => x.MachineDownRecordId,
                        principalTable: "MachineDownRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MachineDownRecords_UserId",
                table: "MachineDownRecords",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_NotifiedTokens_MachineDownRecordId",
                table: "NotifiedTokens",
                column: "MachineDownRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_NotifiedTokens_NotifiedDate",
                table: "NotifiedTokens",
                column: "NotifiedDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotifiedTokens");

            migrationBuilder.DropTable(
                name: "MachineDownRecords");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
