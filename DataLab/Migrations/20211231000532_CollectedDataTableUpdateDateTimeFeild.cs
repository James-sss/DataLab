﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLab.Migrations
{
    public partial class CollectedDataTableUpdateDateTimeFeild : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Datetime",
                table: "CollectedData",
                type: "Datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "Datetime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Datetime",
                table: "CollectedData",
                type: "Datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "Datetime",
                oldNullable: true);
        }
    }
}
