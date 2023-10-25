using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankStatementApi.Migrations
{
    public partial class Update_OperationType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransferType",
                table: "OperationType");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TransferType",
                table: "OperationType",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
