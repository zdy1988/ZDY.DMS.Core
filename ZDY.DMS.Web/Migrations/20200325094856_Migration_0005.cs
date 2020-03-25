using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZDY.DMS.Web.Migrations
{
    public partial class Migration_0005 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifyTime",
                table: "Work_Flow",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastModifyTime",
                table: "Work_Flow");
        }
    }
}
