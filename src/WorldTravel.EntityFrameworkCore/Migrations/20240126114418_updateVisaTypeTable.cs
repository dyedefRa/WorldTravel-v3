using Microsoft.EntityFrameworkCore.Migrations;

namespace WorldTravel.Migrations
{
    public partial class updateVisaTypeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VisaDuration",
                table: "AppVisaTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VisaFee",
                table: "AppVisaTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VisaDuration",
                table: "AppVisaTypes");

            migrationBuilder.DropColumn(
                name: "VisaFee",
                table: "AppVisaTypes");
        }
    }
}
