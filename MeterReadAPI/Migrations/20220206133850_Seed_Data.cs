using Microsoft.EntityFrameworkCore.Migrations;

namespace MeterReadAPI.Migrations
{
    public partial class Seed_Data : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(table: "CustomerAccount", columns: new[] { "AccountId", "Fname", "Lname" }, values: new object[] { 2344, "Tommy", "Test" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
