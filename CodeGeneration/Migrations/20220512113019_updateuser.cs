using Microsoft.EntityFrameworkCore.Migrations;

namespace CodeGeneration.Migrations
{
    public partial class updateuser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2818e94d-3358-4be7-bad2-32dbb0710713");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fd5a1f2b-4af8-4fc8-bcbb-23d3d25ee59a");

            migrationBuilder.AddColumn<long>(
                name: "SerialId",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "UserSerial",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "54ebf5e7-e98c-49e5-9567-5eeaabdfe473", "37b107d3-9180-4505-81fd-f883d3248b11", "User", "User" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "98693314-7422-469f-915c-88062b49add8", "9ca2e4a7-1de5-425b-89e9-1ba6e6d74198", "Admin", "Admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "54ebf5e7-e98c-49e5-9567-5eeaabdfe473");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "98693314-7422-469f-915c-88062b49add8");

            migrationBuilder.DropColumn(
                name: "SerialId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserSerial",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "fd5a1f2b-4af8-4fc8-bcbb-23d3d25ee59a", "63a9f56a-ebb3-4267-bf4b-c1ca27554dfa", "User", "User" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "2818e94d-3358-4be7-bad2-32dbb0710713", "2213249c-610a-4f8a-8965-9ef878f6a6e4", "Admin", "Admin" });
        }
    }
}
