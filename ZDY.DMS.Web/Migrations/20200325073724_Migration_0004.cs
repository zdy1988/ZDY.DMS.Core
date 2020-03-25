using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZDY.DMS.Web.Migrations
{
    public partial class Migration_0004 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Work_Flow",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    Version = table.Column<int>(nullable: false),
                    FormId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    ManagerId = table.Column<Guid>(nullable: false),
                    InstanceManagerId = table.Column<Guid>(nullable: false),
                    CreaterId = table.Column<Guid>(nullable: false),
                    DesignJson = table.Column<string>(nullable: true),
                    RuntimeJson = table.Column<string>(nullable: true),
                    InstallTime = table.Column<DateTime>(nullable: true),
                    InstallerId = table.Column<Guid>(nullable: false),
                    State = table.Column<int>(nullable: false),
                    IsRemoveCompletedInstance = table.Column<bool>(nullable: false),
                    Note = table.Column<string>(nullable: true),
                    IsDisabled = table.Column<bool>(nullable: false),
                    CompanyId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Work_Flow", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Work_Flow_Comment",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    Version = table.Column<int>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    Order = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Work_Flow_Comment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Work_Flow_Delegation",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    Version = table.Column<int>(nullable: false),
                    PublisherId = table.Column<Guid>(nullable: false),
                    StartTime = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: false),
                    FlowId = table.Column<Guid>(nullable: false),
                    DelegaterId = table.Column<Guid>(nullable: false),
                    SetTime = table.Column<DateTime>(nullable: false),
                    Note = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Work_Flow_Delegation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Work_Flow_Form",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    Version = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    CreaterId = table.Column<Guid>(nullable: false),
                    LastModifyTime = table.Column<DateTime>(nullable: false),
                    DesignJson = table.Column<string>(nullable: true),
                    DesignFieldJson = table.Column<string>(nullable: true),
                    State = table.Column<int>(nullable: false),
                    Note = table.Column<string>(nullable: true),
                    IsDisabled = table.Column<bool>(nullable: false),
                    CompanyId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Work_Flow_Form", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Work_Flow_Instance",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    Version = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    FlowId = table.Column<Guid>(nullable: false),
                    FlowName = table.Column<string>(nullable: true),
                    FlowDesignJson = table.Column<string>(nullable: true),
                    FlowJson = table.Column<string>(nullable: true),
                    FormJson = table.Column<string>(nullable: true),
                    FormDataJson = table.Column<string>(nullable: true),
                    LastExecuteTaskId = table.Column<Guid>(nullable: false),
                    LastExecuteStepId = table.Column<Guid>(nullable: false),
                    LastExecuteStepName = table.Column<string>(nullable: true),
                    LastModifyTime = table.Column<DateTime>(nullable: false),
                    CreaterId = table.Column<Guid>(nullable: false),
                    CreaterName = table.Column<string>(nullable: true),
                    State = table.Column<int>(nullable: false),
                    IsDisabled = table.Column<bool>(nullable: false),
                    CompanyId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Work_Flow_Instance", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Work_Flow_Task",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    Version = table.Column<int>(nullable: false),
                    PrevTaskId = table.Column<Guid>(nullable: false),
                    PrevStepId = table.Column<Guid>(nullable: false),
                    FlowId = table.Column<Guid>(nullable: false),
                    FlowName = table.Column<string>(nullable: true),
                    StepId = table.Column<Guid>(nullable: false),
                    StepName = table.Column<string>(nullable: true),
                    InstanceId = table.Column<Guid>(nullable: false),
                    GroupId = table.Column<Guid>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    SenderId = table.Column<Guid>(nullable: false),
                    SenderName = table.Column<string>(nullable: true),
                    SendTime = table.Column<DateTime>(nullable: false),
                    ReceiverId = table.Column<Guid>(nullable: false),
                    ReceiverName = table.Column<string>(nullable: true),
                    ReceiveTime = table.Column<DateTime>(nullable: false),
                    OpenedTime = table.Column<DateTime>(nullable: true),
                    PlannedTime = table.Column<DateTime>(nullable: true),
                    ExecutedTime = table.Column<DateTime>(nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    IsNeedSign = table.Column<bool>(nullable: false),
                    State = table.Column<int>(nullable: false),
                    Note = table.Column<string>(nullable: true),
                    Sort = table.Column<int>(nullable: false),
                    SubFlowInstanceId = table.Column<Guid>(nullable: false),
                    IsDisabled = table.Column<bool>(nullable: false),
                    CompanyId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Work_Flow_Task", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
