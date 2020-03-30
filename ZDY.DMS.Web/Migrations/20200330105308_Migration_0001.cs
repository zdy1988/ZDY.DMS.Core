using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZDY.DMS.Web.Migrations
{
    public partial class Migration_0001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    CompanyName = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Fax = table.Column<string>(nullable: true),
                    Province = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: true),
                    Order = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Department",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    CompanyId = table.Column<Guid>(nullable: false),
                    ParentId = table.Column<Guid>(nullable: false),
                    DepartmentName = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Fax = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: true),
                    Order = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Dictionary_Key",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    Order = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dictionary_Key", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Dictionary_Value",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    DictionaryKey = table.Column<string>(nullable: true),
                    ParentValue = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: true),
                    Order = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dictionary_Value", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "File",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    OriginalName = table.Column<string>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    FileSize = table.Column<long>(nullable: false),
                    ExtensionName = table.Column<string>(nullable: true),
                    Path = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    BusinessID = table.Column<Guid>(nullable: false),
                    Order = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_File", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Log",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    Message = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Log", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    RedirectUrl = table.Column<string>(nullable: true),
                    PushTime = table.Column<DateTime>(nullable: false),
                    OperatorId = table.Column<Guid>(nullable: false),
                    OperatorName = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    Level = table.Column<int>(nullable: false),
                    State = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Message_Inbox",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    MessageId = table.Column<Guid>(nullable: false),
                    ReceiverId = table.Column<Guid>(nullable: false),
                    IsReaded = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message_Inbox", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Page",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    PageName = table.Column<string>(nullable: true),
                    PageCode = table.Column<string>(nullable: true),
                    Level = table.Column<int>(nullable: false),
                    ParentId = table.Column<Guid>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    IsInMenu = table.Column<bool>(nullable: false),
                    MenuName = table.Column<string>(nullable: true),
                    Src = table.Column<string>(nullable: true),
                    Icon = table.Column<string>(nullable: true),
                    Order = table.Column<int>(nullable: false),
                    IsPassed = table.Column<bool>(nullable: false),
                    CompanyId = table.Column<Guid>(nullable: false),
                    IsDisabled = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Page", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Page_Action",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    PageId = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Page_Action", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Avatar = table.Column<Guid>(nullable: false),
                    AvatarUrl = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    NickName = table.Column<string>(nullable: true),
                    CompanyId = table.Column<Guid>(nullable: false),
                    DepartmentId = table.Column<Guid>(nullable: false),
                    Position = table.Column<string>(nullable: true),
                    Gender = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Mobile = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    Province = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    LastLoginIp = table.Column<string>(nullable: true),
                    LastLoginTime = table.Column<DateTime>(nullable: false),
                    Session = table.Column<string>(nullable: true),
                    WeChatOpenId = table.Column<string>(nullable: true),
                    IsDisabled = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User_Group",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    CompanyId = table.Column<Guid>(nullable: false),
                    GroupName = table.Column<string>(nullable: true),
                    GroupCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Group", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User_Group_Action_Permission",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    GroupId = table.Column<Guid>(nullable: false),
                    PageId = table.Column<Guid>(nullable: false),
                    PageActionId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Group_Action_Permission", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User_Group_Member",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    GroupId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Group_Member", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User_Group_Page_Permission",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    GroupId = table.Column<Guid>(nullable: false),
                    PageId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Group_Page_Permission", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Work_Flow",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    FormId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    Managers = table.Column<string>(nullable: true),
                    InstanceManagers = table.Column<string>(nullable: true),
                    CreaterId = table.Column<Guid>(nullable: false),
                    LastModifyTime = table.Column<DateTime>(nullable: false),
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
                    Title = table.Column<string>(nullable: true),
                    FlowId = table.Column<Guid>(nullable: false),
                    FlowName = table.Column<string>(nullable: true),
                    FlowDesignJson = table.Column<string>(nullable: true),
                    FlowRuntimeJson = table.Column<string>(nullable: true),
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
                name: "Company");

            migrationBuilder.DropTable(
                name: "Department");

            migrationBuilder.DropTable(
                name: "Dictionary_Key");

            migrationBuilder.DropTable(
                name: "Dictionary_Value");

            migrationBuilder.DropTable(
                name: "File");

            migrationBuilder.DropTable(
                name: "Log");

            migrationBuilder.DropTable(
                name: "Message");

            migrationBuilder.DropTable(
                name: "Message_Inbox");

            migrationBuilder.DropTable(
                name: "Page");

            migrationBuilder.DropTable(
                name: "Page_Action");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "User_Group");

            migrationBuilder.DropTable(
                name: "User_Group_Action_Permission");

            migrationBuilder.DropTable(
                name: "User_Group_Member");

            migrationBuilder.DropTable(
                name: "User_Group_Page_Permission");

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
