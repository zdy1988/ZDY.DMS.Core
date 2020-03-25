using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZDY.DMS.Web.Migrations
{
    public partial class Migration_0006 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InstanceManagerId",
                table: "Work_Flow");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "Work_Flow");

            migrationBuilder.AddColumn<string>(
                name: "InstanceManagers",
                table: "Work_Flow",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Managers",
                table: "Work_Flow",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InstanceManagers",
                table: "Work_Flow");

            migrationBuilder.DropColumn(
                name: "Managers",
                table: "Work_Flow");

            migrationBuilder.AddColumn<Guid>(
                name: "InstanceManagerId",
                table: "Work_Flow",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ManagerId",
                table: "Work_Flow",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
