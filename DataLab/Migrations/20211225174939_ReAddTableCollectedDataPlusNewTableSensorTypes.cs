using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLab.Migrations
{
    public partial class ReAddTableCollectedDataPlusNewTableSensorTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SensorTypes",
                columns: table => new
                {
                    SensorTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SensorType = table.Column<string>(type: "nvarchar(20)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SensorTypes", x => x.SensorTypeId);
                });

            migrationBuilder.CreateTable(
                name: "CollectedData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Datetime = table.Column<DateTime>(type: "Datetime", nullable: false),
                    SensorTypeId = table.Column<int>(type: "int", nullable: true),
                    Value = table.Column<decimal>(type: "decimal(5,1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollectedData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CollectedData_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CollectedData_SensorTypes_SensorTypeId",
                        column: x => x.SensorTypeId,
                        principalTable: "SensorTypes",
                        principalColumn: "SensorTypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CollectedData_CustomerId",
                table: "CollectedData",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CollectedData_SensorTypeId",
                table: "CollectedData",
                column: "SensorTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CollectedData");

            migrationBuilder.DropTable(
                name: "SensorTypes");
        }
    }
}
