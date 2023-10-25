using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankStatementApi.Migrations
{
    public partial class ChangingFieldInTheDataStorage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DataStorage_DataLog_ImportDataLogId",
                table: "DataStorage");

            migrationBuilder.RenameColumn(
                name: "ImportDataLogId",
                table: "DataStorage",
                newName: "DataLogId");

            migrationBuilder.RenameIndex(
                name: "IX_DataStorage_ImportDataLogId",
                table: "DataStorage",
                newName: "IX_DataStorage_DataLogId");

            migrationBuilder.AddForeignKey(
                name: "FK_DataStorage_DataLog_DataLogId",
                table: "DataStorage",
                column: "DataLogId",
                principalTable: "DataLog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DataStorage_DataLog_DataLogId",
                table: "DataStorage");

            migrationBuilder.RenameColumn(
                name: "DataLogId",
                table: "DataStorage",
                newName: "ImportDataLogId");

            migrationBuilder.RenameIndex(
                name: "IX_DataStorage_DataLogId",
                table: "DataStorage",
                newName: "IX_DataStorage_ImportDataLogId");

            migrationBuilder.AddForeignKey(
                name: "FK_DataStorage_DataLog_ImportDataLogId",
                table: "DataStorage",
                column: "ImportDataLogId",
                principalTable: "DataLog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
