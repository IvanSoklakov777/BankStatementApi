using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BankStatementApi.Migrations
{
    public partial class UpdateTableNameOperationHistoryType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HistoryOperationType");

            migrationBuilder.CreateTable(
                name: "OperationTypeHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WorkerChangedById = table.Column<int>(type: "integer", nullable: false),
                    ChangeDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PaymentOrderId = table.Column<int>(type: "integer", nullable: false),
                    OperationTypeId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationTypeHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OperationTypeHistory_OperationType_OperationTypeId",
                        column: x => x.OperationTypeId,
                        principalTable: "OperationType",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OperationTypeHistory_PaymentOrder_PaymentOrderId",
                        column: x => x.PaymentOrderId,
                        principalTable: "PaymentOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OperationTypeHistory_OperationTypeId",
                table: "OperationTypeHistory",
                column: "OperationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_OperationTypeHistory_PaymentOrderId",
                table: "OperationTypeHistory",
                column: "PaymentOrderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OperationTypeHistory");

            migrationBuilder.CreateTable(
                name: "HistoryOperationType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OperationTypeId = table.Column<Guid>(type: "uuid", nullable: true),
                    PaymentOrderId = table.Column<int>(type: "integer", nullable: false),
                    ChangeDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    WorkerChangedById = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoryOperationType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistoryOperationType_OperationType_OperationTypeId",
                        column: x => x.OperationTypeId,
                        principalTable: "OperationType",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HistoryOperationType_PaymentOrder_PaymentOrderId",
                        column: x => x.PaymentOrderId,
                        principalTable: "PaymentOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HistoryOperationType_OperationTypeId",
                table: "HistoryOperationType",
                column: "OperationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoryOperationType_PaymentOrderId",
                table: "HistoryOperationType",
                column: "PaymentOrderId");
        }
    }
}
