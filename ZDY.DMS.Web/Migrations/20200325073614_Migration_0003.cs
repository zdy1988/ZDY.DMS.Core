using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZDY.DMS.Web.Migrations
{
    public partial class Migration_0003 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Work_Flow");

            migrationBuilder.DropTable(
                name: "Work_Flow_Comment");

            migrationBuilder.DropTable(
                name: "Work_Flow_Delegation");

            migrationBuilder.DropTable(
                name: "Work_Flow_Form");

            migrationBuilder.DropTable(
                name: "Work_Flow_Instance");

            migrationBuilder.DropTable(
                name: "Work_Flow_Task");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "User",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "User");

            migrationBuilder.CreateTable(
                name: "Work_Flow",
                columns: table => new
                {
                    Id = table.Column<string>(type: "char(36)", nullable: false),
                    CompanyId = table.Column<string>(type: "char(36)", nullable: false),
                    CreaterId = table.Column<string>(type: "char(36)", nullable: false),
                    DesignJson = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    FormId = table.Column<string>(type: "char(36)", nullable: false),
                    InstallTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    InstallerId = table.Column<string>(type: "char(36)", nullable: false),
                    InstanceManager = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    IsDisabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsRemoveCompleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Manager = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Name = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Note = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    RunJson = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    State = table.Column<int>(type: "int", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Work_Flow", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Work_Flow_Comment",
                columns: table => new
                {
                    Id = table.Column<string>(type: "char(36)", nullable: false),
                    Comment = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "char(36)", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Work_Flow_Comment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Work_Flow_Delegation",
                columns: table => new
                {
                    Id = table.Column<string>(type: "char(36)", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FlowId = table.Column<string>(type: "char(36)", nullable: true),
                    Note = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    SettingTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ToUserId = table.Column<string>(type: "char(36)", nullable: false),
                    UserId = table.Column<string>(type: "char(36)", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Work_Flow_Delegation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Work_Flow_Form",
                columns: table => new
                {
                    Id = table.Column<string>(type: "char(36)", nullable: false),
                    CompanyId = table.Column<string>(type: "char(36)", nullable: false),
                    CreaterId = table.Column<string>(type: "char(36)", nullable: false),
                    DesignFieldJson = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    DesignJson = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    IsDisabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Name = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Note = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    State = table.Column<int>(type: "int", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Work_Flow_Form", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Work_Flow_Instance",
                columns: table => new
                {
                    Id = table.Column<string>(type: "char(36)", nullable: false),
                    CompanyID = table.Column<string>(type: "char(36)", nullable: false),
                    CreaterID = table.Column<string>(type: "char(36)", nullable: false),
                    CreaterName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    DataJson = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    DesignJson = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    FlowID = table.Column<string>(type: "char(36)", nullable: false),
                    FlowJson = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    FlowName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    FormJson = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    InstanceID = table.Column<string>(type: "char(36)", nullable: false),
                    IsDisabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LastExecuteStepID = table.Column<string>(type: "char(36)", nullable: false),
                    LastExecuteStepName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    LastExecuteTaskID = table.Column<string>(type: "char(36)", nullable: false),
                    LastModifyTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Title = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Work_Flow_Instance", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Work_Flow_Task",
                columns: table => new
                {
                    Id = table.Column<string>(type: "char(36)", nullable: false),
                    Comment = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    CompanyId = table.Column<string>(type: "char(36)", nullable: false),
                    ExecutedTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    FlowId = table.Column<string>(type: "char(36)", nullable: false),
                    FlowName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    GroupId = table.Column<string>(type: "char(36)", nullable: false),
                    InstanceId = table.Column<string>(type: "char(36)", nullable: false),
                    IsDisabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsSign = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Note = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    OpenedTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    PlannedTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    PrevStepId = table.Column<string>(type: "char(36)", nullable: false),
                    PrevTaskId = table.Column<string>(type: "char(36)", nullable: false),
                    ReceiveTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ReceiverId = table.Column<string>(type: "char(36)", nullable: false),
                    ReceiverName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    SendTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    SenderId = table.Column<string>(type: "char(36)", nullable: false),
                    SenderName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Sort = table.Column<int>(type: "int", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    StepId = table.Column<string>(type: "char(36)", nullable: false),
                    StepName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    SubFlowInstanceId = table.Column<string>(type: "char(36)", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Title = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Work_Flow_Task", x => x.Id);
                });
        }
    }
}
