using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BankStatementApi.Migrations
{
    public partial class InitializationBankStatementService : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DataLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FileName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    FileHash = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: true),
                    FileSize = table.Column<int>(type: "integer", nullable: false),
                    DateStartLoad = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateEndLoad = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateFileStart = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DateFileEnd = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ImportLog = table.Column<string>(type: "text", nullable: true),
                    ImportResultId = table.Column<int>(type: "integer", nullable: false),
                    WorkerChangedById = table.Column<int>(type: "integer", nullable: false),
                    ChangeDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ImportResult",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: true),
                    WorkerChangedById = table.Column<int>(type: "integer", nullable: false),
                    ChangeDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportResult", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OperationType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: true),
                    TransferType = table.Column<int>(type: "integer", nullable: false),
                    ChangeDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    WorkerChangedById = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransferType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: true),
                    ChangeDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    WorkerChangedById = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BankAccountDocument",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BankAccountNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    DateStart = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateEnd = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    StartBalance = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalReceived = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalCharged = table.Column<decimal>(type: "numeric", nullable: false),
                    FinalBalance = table.Column<decimal>(type: "numeric", nullable: false),
                    BankAccountId = table.Column<int>(type: "integer", nullable: true),
                    WorkerChangedById = table.Column<int>(type: "integer", nullable: false),
                    ChangeDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ImportId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankAccountDocument", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankAccountDocument_DataLog_ImportId",
                        column: x => x.ImportId,
                        principalTable: "DataLog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DataStorage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BinaryContent = table.Column<byte[]>(type: "bytea", nullable: true),
                    WorkerChangedById = table.Column<int>(type: "integer", nullable: false),
                    ChangeDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ImportDataLogId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataStorage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DataStorage_DataLog_ImportDataLogId",
                        column: x => x.ImportDataLogId,
                        principalTable: "DataLog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IntegrationModuleOperation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ModuleName = table.Column<string>(type: "text", nullable: true),
                    WorkerChangedById = table.Column<int>(type: "integer", nullable: false),
                    ChangeDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OperationTypeId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegrationModuleOperation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IntegrationModuleOperation_OperationType_OperationTypeId",
                        column: x => x.OperationTypeId,
                        principalTable: "OperationType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaymentOrder",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Number = table.Column<string>(type: "text", nullable: true),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Sum = table.Column<decimal>(type: "numeric", nullable: false),
                    Payer = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    PayerAccount = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    PayerCorAccount = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    PayerCalcAccount = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    PayerBank = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    PayerINN = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: true),
                    PayerBIK = table.Column<string>(type: "character varying(9)", maxLength: 9, nullable: true),
                    PayerKPP = table.Column<string>(type: "character varying(9)", maxLength: 9, nullable: true),
                    Recipient = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    RecipientAccount = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    RecipientINN = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: true),
                    RecipientBIK = table.Column<string>(type: "character varying(9)", maxLength: 9, nullable: true),
                    RecipientKPP = table.Column<string>(type: "character varying(9)", maxLength: 9, nullable: true),
                    RecipientCorAccount = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    RecipientCalcAccount = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    RecipientBank = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    ReceivedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    WriteOffDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PaymentType = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: true),
                    PaymentTerm = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Priority = table.Column<string>(type: "text", nullable: true),
                    PaymentPurpose = table.Column<string>(type: "text", nullable: true),
                    WorkerChangedById = table.Column<int>(type: "integer", nullable: false),
                    ChangeDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OperationTypeId = table.Column<Guid>(type: "uuid", nullable: true),
                    IntegrationModuleOperationId = table.Column<int>(type: "integer", nullable: true),
                    BankAccountDocumentId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentOrder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentOrder_BankAccountDocument_BankAccountDocumentId",
                        column: x => x.BankAccountDocumentId,
                        principalTable: "BankAccountDocument",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PaymentOrder_IntegrationModuleOperation_IntegrationModuleOp~",
                        column: x => x.IntegrationModuleOperationId,
                        principalTable: "IntegrationModuleOperation",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PaymentOrder_OperationType_OperationTypeId",
                        column: x => x.OperationTypeId,
                        principalTable: "OperationType",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HistoryOperationType",
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
                name: "IX_BankAccountDocument_ImportId",
                table: "BankAccountDocument",
                column: "ImportId");

            migrationBuilder.CreateIndex(
                name: "IX_DataStorage_ImportDataLogId",
                table: "DataStorage",
                column: "ImportDataLogId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HistoryOperationType_OperationTypeId",
                table: "HistoryOperationType",
                column: "OperationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoryOperationType_PaymentOrderId",
                table: "HistoryOperationType",
                column: "PaymentOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_IntegrationModuleOperation_OperationTypeId",
                table: "IntegrationModuleOperation",
                column: "OperationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentOrder_BankAccountDocumentId",
                table: "PaymentOrder",
                column: "BankAccountDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentOrder_IntegrationModuleOperationId",
                table: "PaymentOrder",
                column: "IntegrationModuleOperationId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentOrder_OperationTypeId",
                table: "PaymentOrder",
                column: "OperationTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DataStorage");

            migrationBuilder.DropTable(
                name: "HistoryOperationType");

            migrationBuilder.DropTable(
                name: "ImportResult");

            migrationBuilder.DropTable(
                name: "TransferType");

            migrationBuilder.DropTable(
                name: "PaymentOrder");

            migrationBuilder.DropTable(
                name: "BankAccountDocument");

            migrationBuilder.DropTable(
                name: "IntegrationModuleOperation");

            migrationBuilder.DropTable(
                name: "DataLog");

            migrationBuilder.DropTable(
                name: "OperationType");
        }
    }
}
