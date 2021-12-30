using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLab.Migrations
{
    public partial class CollectedDataUpdatesCustIntIDAndSensorIntIDAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CollectedData_Customers_CustomerId",
                table: "CollectedData");

            migrationBuilder.DropForeignKey(
                name: "FK_CollectedData_SensorTypes_SensorTypeId",
                table: "CollectedData");

            migrationBuilder.AlterColumn<int>(
                name: "SensorTypeId",
                table: "CollectedData",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "CollectedData",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CollectedData_Customers_CustomerId",
                table: "CollectedData",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CollectedData_SensorTypes_SensorTypeId",
                table: "CollectedData",
                column: "SensorTypeId",
                principalTable: "SensorTypes",
                principalColumn: "SensorTypeId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CollectedData_Customers_CustomerId",
                table: "CollectedData");

            migrationBuilder.DropForeignKey(
                name: "FK_CollectedData_SensorTypes_SensorTypeId",
                table: "CollectedData");

            migrationBuilder.AlterColumn<int>(
                name: "SensorTypeId",
                table: "CollectedData",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "CollectedData",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_CollectedData_Customers_CustomerId",
                table: "CollectedData",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CollectedData_SensorTypes_SensorTypeId",
                table: "CollectedData",
                column: "SensorTypeId",
                principalTable: "SensorTypes",
                principalColumn: "SensorTypeId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
