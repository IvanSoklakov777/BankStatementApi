﻿// <auto-generated />
using System;
using BankStatementApi.DAL.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BankStatementApi.Migrations
{
    [DbContext(typeof(BankStatementContext))]
    partial class BankStatementContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("BankStatementApi.DAL.Entities.BankAccountDocument", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("BankAccountId")
                        .HasColumnType("integer");

                    b.Property<string>("BankAccountNumber")
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<DateTime>("ChangeDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("DataLogId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("DateEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DateStart")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal>("FinalBalance")
                        .HasColumnType("numeric");

                    b.Property<decimal>("StartBalance")
                        .HasColumnType("numeric");

                    b.Property<decimal>("TotalCharged")
                        .HasColumnType("numeric");

                    b.Property<decimal>("TotalReceived")
                        .HasColumnType("numeric");

                    b.Property<int?>("WorkerChangedById")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("DataLogId");

                    b.ToTable("BankAccountDocument");
                });

            modelBuilder.Entity("BankStatementApi.DAL.Entities.DataLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("ChangeDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DateEndLoad")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("DateFileEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("DateFileStart")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DateStartLoad")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("FileHash")
                        .HasMaxLength(36)
                        .HasColumnType("character varying(36)");

                    b.Property<string>("FileName")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<int>("FileSize")
                        .HasColumnType("integer");

                    b.Property<string>("ImportLog")
                        .HasColumnType("text");

                    b.Property<int>("ImportResultId")
                        .HasColumnType("integer");

                    b.Property<int?>("WorkerChangedById")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("DataLog");
                });

            modelBuilder.Entity("BankStatementApi.DAL.Entities.DataStorage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<byte[]>("BinaryContent")
                        .HasColumnType("bytea");

                    b.Property<DateTime>("ChangeDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("DataLogId")
                        .HasColumnType("integer");

                    b.Property<int?>("WorkerChangedById")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("DataLogId")
                        .IsUnique();

                    b.ToTable("DataStorage");
                });

            modelBuilder.Entity("BankStatementApi.DAL.Entities.ImportResult", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("ChangeDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.Property<int?>("WorkerChangedById")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("ImportResult");
                });

            modelBuilder.Entity("BankStatementApi.DAL.Entities.IntegrationModuleOperation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("ChangeDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ModuleName")
                        .HasColumnType("text");

                    b.Property<Guid>("OperationTypeId")
                        .HasColumnType("uuid");

                    b.Property<int?>("WorkerChangedById")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("OperationTypeId");

                    b.ToTable("IntegrationModuleOperation");
                });

            modelBuilder.Entity("BankStatementApi.DAL.Entities.OperationType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("ChangeDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.Property<int?>("WorkerChangedById")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("OperationType");
                });

            modelBuilder.Entity("BankStatementApi.DAL.Entities.OperationTypeHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("ChangeDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("OperationTypeId")
                        .HasColumnType("uuid");

                    b.Property<int>("PaymentOrderId")
                        .HasColumnType("integer");

                    b.Property<int?>("WorkerChangedById")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("OperationTypeId");

                    b.HasIndex("PaymentOrderId");

                    b.ToTable("OperationTypeHistory");
                });

            modelBuilder.Entity("BankStatementApi.DAL.Entities.PaymentOrder", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("BankAccountDocumentId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("ChangeDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("IntegrationModuleOperationId")
                        .HasColumnType("integer");

                    b.Property<string>("Number")
                        .HasColumnType("text");

                    b.Property<Guid?>("OperationTypeId")
                        .HasColumnType("uuid");

                    b.Property<string>("Payer")
                        .HasMaxLength(512)
                        .HasColumnType("character varying(512)");

                    b.Property<string>("PayerAccount")
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<string>("PayerBIK")
                        .HasMaxLength(9)
                        .HasColumnType("character varying(9)");

                    b.Property<string>("PayerBank")
                        .HasMaxLength(512)
                        .HasColumnType("character varying(512)");

                    b.Property<string>("PayerCalcAccount")
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<string>("PayerCorAccount")
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<string>("PayerINN")
                        .HasMaxLength(12)
                        .HasColumnType("character varying(12)");

                    b.Property<string>("PayerKPP")
                        .HasMaxLength(9)
                        .HasColumnType("character varying(9)");

                    b.Property<string>("PaymentPurpose")
                        .HasColumnType("text");

                    b.Property<DateTime>("PaymentTerm")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("PaymentType")
                        .HasMaxLength(5)
                        .HasColumnType("character varying(5)");

                    b.Property<string>("Priority")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ReceivedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Recipient")
                        .HasMaxLength(512)
                        .HasColumnType("character varying(512)");

                    b.Property<string>("RecipientAccount")
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<string>("RecipientBIK")
                        .HasMaxLength(9)
                        .HasColumnType("character varying(9)");

                    b.Property<string>("RecipientBank")
                        .HasMaxLength(512)
                        .HasColumnType("character varying(512)");

                    b.Property<string>("RecipientCalcAccount")
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<string>("RecipientCorAccount")
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<string>("RecipientINN")
                        .HasMaxLength(12)
                        .HasColumnType("character varying(12)");

                    b.Property<string>("RecipientKPP")
                        .HasMaxLength(9)
                        .HasColumnType("character varying(9)");

                    b.Property<decimal>("Sum")
                        .HasColumnType("numeric");

                    b.Property<int?>("WorkerChangedById")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("WriteOffDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("BankAccountDocumentId");

                    b.HasIndex("IntegrationModuleOperationId");

                    b.HasIndex("OperationTypeId");

                    b.ToTable("PaymentOrder");
                });

            modelBuilder.Entity("BankStatementApi.DAL.Entities.TransferType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("ChangeDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.Property<int?>("WorkerChangedById")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("TransferType");
                });

            modelBuilder.Entity("BankStatementApi.DAL.Entities.BankAccountDocument", b =>
                {
                    b.HasOne("BankStatementApi.DAL.Entities.DataLog", "DataLog")
                        .WithMany("BankAccountDocuments")
                        .HasForeignKey("DataLogId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DataLog");
                });

            modelBuilder.Entity("BankStatementApi.DAL.Entities.DataStorage", b =>
                {
                    b.HasOne("BankStatementApi.DAL.Entities.DataLog", "DataLog")
                        .WithOne("DataStorage")
                        .HasForeignKey("BankStatementApi.DAL.Entities.DataStorage", "DataLogId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DataLog");
                });

            modelBuilder.Entity("BankStatementApi.DAL.Entities.IntegrationModuleOperation", b =>
                {
                    b.HasOne("BankStatementApi.DAL.Entities.OperationType", "OperationType")
                        .WithMany()
                        .HasForeignKey("OperationTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("OperationType");
                });

            modelBuilder.Entity("BankStatementApi.DAL.Entities.OperationTypeHistory", b =>
                {
                    b.HasOne("BankStatementApi.DAL.Entities.OperationType", "OperationType")
                        .WithMany()
                        .HasForeignKey("OperationTypeId");

                    b.HasOne("BankStatementApi.DAL.Entities.PaymentOrder", "PaymentOrder")
                        .WithMany("OperationTypeHistory")
                        .HasForeignKey("PaymentOrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("OperationType");

                    b.Navigation("PaymentOrder");
                });

            modelBuilder.Entity("BankStatementApi.DAL.Entities.PaymentOrder", b =>
                {
                    b.HasOne("BankStatementApi.DAL.Entities.BankAccountDocument", "BankAccountDocument")
                        .WithMany("PaymentOrders")
                        .HasForeignKey("BankAccountDocumentId");

                    b.HasOne("BankStatementApi.DAL.Entities.IntegrationModuleOperation", "IntegrationModuleOperation")
                        .WithMany()
                        .HasForeignKey("IntegrationModuleOperationId");

                    b.HasOne("BankStatementApi.DAL.Entities.OperationType", "OperationType")
                        .WithMany()
                        .HasForeignKey("OperationTypeId");

                    b.Navigation("BankAccountDocument");

                    b.Navigation("IntegrationModuleOperation");

                    b.Navigation("OperationType");
                });

            modelBuilder.Entity("BankStatementApi.DAL.Entities.BankAccountDocument", b =>
                {
                    b.Navigation("PaymentOrders");
                });

            modelBuilder.Entity("BankStatementApi.DAL.Entities.DataLog", b =>
                {
                    b.Navigation("BankAccountDocuments");

                    b.Navigation("DataStorage");
                });

            modelBuilder.Entity("BankStatementApi.DAL.Entities.PaymentOrder", b =>
                {
                    b.Navigation("OperationTypeHistory");
                });
#pragma warning restore 612, 618
        }
    }
}
