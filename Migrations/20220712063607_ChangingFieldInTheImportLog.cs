using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankStatementApi.Migrations
{
    public partial class ChangingFieldInTheImportLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankAccountDocument_DataLog_ImportId",
                table: "BankAccountDocument");

            migrationBuilder.RenameColumn(
                name: "ImportId",
                table: "BankAccountDocument",
                newName: "DataLogId");

            migrationBuilder.RenameIndex(
                name: "IX_BankAccountDocument_ImportId",
                table: "BankAccountDocument",
                newName: "IX_BankAccountDocument_DataLogId");

            migrationBuilder.AddForeignKey(
                name: "FK_BankAccountDocument_DataLog_DataLogId",
                table: "BankAccountDocument",
                column: "DataLogId",
                principalTable: "DataLog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankAccountDocument_DataLog_DataLogId",
                table: "BankAccountDocument");

            migrationBuilder.RenameColumn(
                name: "DataLogId",
                table: "BankAccountDocument",
                newName: "ImportId");

            migrationBuilder.RenameIndex(
                name: "IX_BankAccountDocument_DataLogId",
                table: "BankAccountDocument",
                newName: "IX_BankAccountDocument_ImportId");

            migrationBuilder.AddForeignKey(
                name: "FK_BankAccountDocument_DataLog_ImportId",
                table: "BankAccountDocument",
                column: "ImportId",
                principalTable: "DataLog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
