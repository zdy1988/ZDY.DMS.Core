using Microsoft.EntityFrameworkCore.Migrations;

namespace ZDY.DMS.Web.Migrations
{
    public partial class Migration_0007 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Version",
                table: "Work_Flow_Task");

            migrationBuilder.DropColumn(
                name: "FlowJson",
                table: "Work_Flow_Instance");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Work_Flow_Instance");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Work_Flow_Form");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Work_Flow_Delegation");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Work_Flow_Comment");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Work_Flow");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Warehouse_Order_Item_Attribute");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Warehouse_Order_Item");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Warehouse_Order");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Warehouse");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "User_Group_Page_Permission");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "User_Group_Member");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "User_Group_Action_Permission");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "User_Group");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Supplier");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Page_Action");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Page");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Message_Inbox");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Log");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "File");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Dictionary_Value");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Dictionary_Key");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Department");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Business_Order_Item_Attribute");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Business_Order_Item");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Business_Order");

            migrationBuilder.AddColumn<string>(
                name: "FlowRuntimeJson",
                table: "Work_Flow_Instance",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FlowRuntimeJson",
                table: "Work_Flow_Instance");

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "Work_Flow_Task",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "FlowJson",
                table: "Work_Flow_Instance",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "Work_Flow_Instance",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "Work_Flow_Form",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "Work_Flow_Delegation",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "Work_Flow_Comment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "Work_Flow",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "Warehouse_Order_Item_Attribute",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "Warehouse_Order_Item",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "Warehouse_Order",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "Warehouse",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "User_Group_Page_Permission",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "User_Group_Member",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "User_Group_Action_Permission",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "User_Group",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "Supplier",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "Page_Action",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "Page",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "Message_Inbox",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "Message",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "Log",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "File",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "Dictionary_Value",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "Dictionary_Key",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "Department",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "Customer",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "Company",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "Business_Order_Item_Attribute",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "Business_Order_Item",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "Business_Order",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
