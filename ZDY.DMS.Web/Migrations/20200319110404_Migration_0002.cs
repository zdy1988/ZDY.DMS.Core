using Microsoft.EntityFrameworkCore.Migrations;

namespace ZDY.DMS.Web.Migrations
{
    public partial class Migration_0002 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Group_Page_Authority",
                table: "Group_Page_Authority");

            migrationBuilder.RenameTable(
                name: "Group_Page_Authority",
                newName: "User_Group_Page_Permission");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User_Group_Page_Permission",
                table: "User_Group_Page_Permission",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_User_Group_Page_Permission",
                table: "User_Group_Page_Permission");

            migrationBuilder.RenameTable(
                name: "User_Group_Page_Permission",
                newName: "Group_Page_Authority");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Group_Page_Authority",
                table: "Group_Page_Authority",
                column: "Id");
        }
    }
}
