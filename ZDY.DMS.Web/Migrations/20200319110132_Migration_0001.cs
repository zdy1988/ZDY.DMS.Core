using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZDY.DMS.Web.Migrations
{
    public partial class Migration_0001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Business_Order",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    Version = table.Column<int>(nullable: false),
                    CompanyId = table.Column<Guid>(nullable: false),
                    SerialCode = table.Column<string>(nullable: true),
                    SalerId = table.Column<Guid>(nullable: false),
                    CustomerId = table.Column<Guid>(nullable: false),
                    CustomerName = table.Column<string>(nullable: false),
                    CustomerMobile = table.Column<string>(nullable: false),
                    CustomerAddress = table.Column<string>(nullable: true),
                    BusinessOrderType = table.Column<int>(nullable: false),
                    TotalCount = table.Column<int>(nullable: false),
                    TotalAmount = table.Column<decimal>(nullable: false),
                    BusinessOrderState = table.Column<int>(nullable: false),
                    Note = table.Column<string>(nullable: true),
                    IsDisabled = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Business_Order", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Business_Order_Item",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    Version = table.Column<int>(nullable: false),
                    BusinessOrderId = table.Column<Guid>(nullable: false),
                    ProductId = table.Column<Guid>(nullable: false),
                    ProductCode = table.Column<string>(nullable: true),
                    Count = table.Column<int>(nullable: false),
                    Attributes = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(nullable: false),
                    SubTotal = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Business_Order_Item", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Business_Order_Item_Attribute",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    Version = table.Column<int>(nullable: false),
                    BusinessOrderItemId = table.Column<Guid>(nullable: false),
                    Attribute = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Business_Order_Item_Attribute", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    Version = table.Column<int>(nullable: false),
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
                name: "Customer",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    Version = table.Column<int>(nullable: false),
                    CompanyId = table.Column<Guid>(nullable: false),
                    CustomerName = table.Column<string>(nullable: false),
                    Mobile = table.Column<string>(nullable: false),
                    Contact = table.Column<string>(nullable: true),
                    Provinces = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Fax = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    ZipCode = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: true),
                    IsDisabled = table.Column<bool>(nullable: false),
                    Order = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Department",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    Version = table.Column<int>(nullable: false),
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
                    Version = table.Column<int>(nullable: false),
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
                    Version = table.Column<int>(nullable: false),
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
                    Version = table.Column<int>(nullable: false),
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
                name: "Group_Page_Authority",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    Version = table.Column<int>(nullable: false),
                    GroupId = table.Column<Guid>(nullable: false),
                    PageId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Group_Page_Authority", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Log",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    Version = table.Column<int>(nullable: false),
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
                    Version = table.Column<int>(nullable: false),
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
                    Version = table.Column<int>(nullable: false),
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
                    Version = table.Column<int>(nullable: false),
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
                    Version = table.Column<int>(nullable: false),
                    PageId = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Page_Action", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    Version = table.Column<int>(nullable: false),
                    CompanyId = table.Column<Guid>(nullable: false),
                    Cover = table.Column<Guid>(nullable: false),
                    ProductName = table.Column<string>(nullable: false),
                    ProductCode = table.Column<string>(nullable: false),
                    Tags = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(nullable: false),
                    Cost = table.Column<decimal>(nullable: false),
                    SupplierId = table.Column<Guid>(nullable: false),
                    WarehouseId = table.Column<Guid>(nullable: false),
                    ProductType = table.Column<string>(nullable: false),
                    ProductColors = table.Column<string>(nullable: true),
                    ProductModels = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: true),
                    IsDisabled = table.Column<bool>(nullable: false),
                    ProductState = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Order = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Supplier",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    Version = table.Column<int>(nullable: false),
                    CompanyId = table.Column<Guid>(nullable: false),
                    SupplierName = table.Column<string>(nullable: false),
                    Mobile = table.Column<string>(nullable: false),
                    Contact = table.Column<string>(nullable: true),
                    Provinces = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Fax = table.Column<string>(nullable: true),
                    ZipCode = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: true),
                    IsDisabled = table.Column<bool>(nullable: false),
                    Order = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Supplier", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    Version = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Avatar = table.Column<Guid>(nullable: false),
                    AvatarUrl = table.Column<string>(nullable: true),
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
                    Version = table.Column<int>(nullable: false),
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
                    Version = table.Column<int>(nullable: false),
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
                    Version = table.Column<int>(nullable: false),
                    GroupId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Group_Member", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Warehouse",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    Version = table.Column<int>(nullable: false),
                    CompanyId = table.Column<Guid>(nullable: false),
                    WarehouseName = table.Column<string>(nullable: false),
                    Phone = table.Column<string>(nullable: true),
                    Provinces = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: true),
                    IsDisabled = table.Column<bool>(nullable: false),
                    Order = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warehouse", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Warehouse_Order",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    Version = table.Column<int>(nullable: false),
                    CompanyId = table.Column<Guid>(nullable: false),
                    SerialCode = table.Column<string>(nullable: true),
                    WarehouseId = table.Column<Guid>(nullable: false),
                    SupplierId = table.Column<Guid>(nullable: false),
                    CustomerId = table.Column<Guid>(nullable: false),
                    BusinessOrderId = table.Column<Guid>(nullable: false),
                    WarehouseOrderType = table.Column<int>(nullable: false),
                    WarehouseOrderState = table.Column<int>(nullable: false),
                    TotalCount = table.Column<int>(nullable: false),
                    TotalAmount = table.Column<decimal>(nullable: false),
                    Note = table.Column<string>(nullable: true),
                    IsDisabled = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warehouse_Order", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Warehouse_Order_Item",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    Version = table.Column<int>(nullable: false),
                    WarehouseOrderId = table.Column<Guid>(nullable: false),
                    ProductId = table.Column<Guid>(nullable: false),
                    ProductCode = table.Column<string>(nullable: true),
                    Count = table.Column<int>(nullable: false),
                    Attributes = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(nullable: false),
                    SubTotal = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warehouse_Order_Item", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Warehouse_Order_Item_Attribute",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    Version = table.Column<int>(nullable: false),
                    WarehouseOrderItemId = table.Column<Guid>(nullable: false),
                    Attribute = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warehouse_Order_Item_Attribute", x => x.Id);
                });

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
                    Manager = table.Column<string>(nullable: true),
                    InstanceManager = table.Column<string>(nullable: true),
                    CreaterId = table.Column<Guid>(nullable: false),
                    DesignJson = table.Column<string>(nullable: true),
                    RunJson = table.Column<string>(nullable: true),
                    InstallTime = table.Column<DateTime>(nullable: true),
                    InstallerId = table.Column<Guid>(nullable: false),
                    State = table.Column<int>(nullable: false),
                    IsRemoveCompleted = table.Column<bool>(nullable: false),
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
                    UserId = table.Column<Guid>(nullable: false),
                    StartTime = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: false),
                    FlowId = table.Column<Guid>(nullable: true),
                    ToUserId = table.Column<Guid>(nullable: false),
                    SettingTime = table.Column<DateTime>(nullable: false),
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
                    InstanceID = table.Column<Guid>(nullable: false),
                    FlowID = table.Column<Guid>(nullable: false),
                    FlowName = table.Column<string>(nullable: true),
                    FormJson = table.Column<string>(nullable: true),
                    DataJson = table.Column<string>(nullable: true),
                    DesignJson = table.Column<string>(nullable: true),
                    FlowJson = table.Column<string>(nullable: true),
                    LastExecuteTaskID = table.Column<Guid>(nullable: false),
                    LastExecuteStepID = table.Column<Guid>(nullable: false),
                    LastExecuteStepName = table.Column<string>(nullable: true),
                    LastModifyTime = table.Column<DateTime>(nullable: false),
                    CreaterID = table.Column<Guid>(nullable: false),
                    CreaterName = table.Column<string>(nullable: true),
                    State = table.Column<int>(nullable: false),
                    IsDisabled = table.Column<bool>(nullable: false),
                    CompanyID = table.Column<Guid>(nullable: false)
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
                    IsSign = table.Column<bool>(nullable: false),
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
                name: "Business_Order");

            migrationBuilder.DropTable(
                name: "Business_Order_Item");

            migrationBuilder.DropTable(
                name: "Business_Order_Item_Attribute");

            migrationBuilder.DropTable(
                name: "Company");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "Department");

            migrationBuilder.DropTable(
                name: "Dictionary_Key");

            migrationBuilder.DropTable(
                name: "Dictionary_Value");

            migrationBuilder.DropTable(
                name: "File");

            migrationBuilder.DropTable(
                name: "Group_Page_Authority");

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
                name: "Product");

            migrationBuilder.DropTable(
                name: "Supplier");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "User_Group");

            migrationBuilder.DropTable(
                name: "User_Group_Action_Permission");

            migrationBuilder.DropTable(
                name: "User_Group_Member");

            migrationBuilder.DropTable(
                name: "Warehouse");

            migrationBuilder.DropTable(
                name: "Warehouse_Order");

            migrationBuilder.DropTable(
                name: "Warehouse_Order_Item");

            migrationBuilder.DropTable(
                name: "Warehouse_Order_Item_Attribute");

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
