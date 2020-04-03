using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZDY.DMS.Web.Migrations
{
    public partial class Migration_0002 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OperatorId",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "OperatorName",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "PushTime",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "RedirectUrl",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Message");

            migrationBuilder.AddColumn<bool>(
                name: "IsSended",
                table: "Message",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "SenderId",
                table: "Message",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "SenderName",
                table: "Message",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSended",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "SenderId",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "SenderName",
                table: "Message");

            migrationBuilder.AddColumn<Guid>(
                name: "OperatorId",
                table: "Message",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "OperatorName",
                table: "Message",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PushTime",
                table: "Message",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "RedirectUrl",
                table: "Message",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "Message",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
