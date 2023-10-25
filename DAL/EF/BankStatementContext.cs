using BankStatementApi.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BankStatementApi.DAL.EF
{
    /// <summary>
    /// Контекст БД
    /// </summary>
    public class BankStatementContext : DbContext
    {
        public BankStatementContext(DbContextOptions<BankStatementContext> options) : base(options) { }
        /// <summary>
        /// Преобразование времени к utc
        /// </summary>
        private readonly static ValueConverter<DateTime, DateTime> _dateConverter = new(
            d => DateTime.SpecifyKind(d, DateTimeKind.Utc),
            d => d);
        /// <summary>
        /// Преобразование времени к utc
        /// </summary>
        private readonly static ValueConverter<DateTime?, DateTime?> _dateWithNullConverter = new(
            d => d != null ? DateTime.SpecifyKind(d.Value, DateTimeKind.Utc) : null,
            d => d);

        /// <summary>
        /// При создании моделей
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DataLog>(DataLog);
            modelBuilder.Entity<BankAccountDocument>(BankAccountDocument);
            modelBuilder.Entity<PaymentOrder>(PaymentOrder);
            modelBuilder.Entity<OperationTypeHistory>(OperationTypeHistory);
            modelBuilder.Entity<OperationType>(OperationType);
            modelBuilder.Entity<ImportResult>(ImportResult);
            modelBuilder.Entity<TransferType>(TransferType);
            modelBuilder.Entity<DataStorage>(DataStorage);
            modelBuilder.Entity<IntegrationModuleOperation>(IntegrationModule);

        }

        /// <summary>
        /// Настройки для модели
        /// </summary>
        /// <param name="builder"></param>
        public void DataLog(EntityTypeBuilder<DataLog> builder)
        {
            builder.Property(u => u.DateStartLoad).HasConversion(_dateWithNullConverter);
            builder.Property(u => u.DateEndLoad).HasConversion(_dateWithNullConverter);
            builder.Property(u => u.DateFileStart).HasConversion(_dateWithNullConverter);
            builder.Property(u => u.DateFileEnd).HasConversion(_dateWithNullConverter);
            builder.Property(u => u.ChangeDate).HasConversion(_dateWithNullConverter);
        }
        /// <summary>
        /// Настройки для модели
        /// </summary>
        /// <param name="builder"></param>
        public void DataStorage(EntityTypeBuilder<DataStorage> builder)
        {
            builder.Property(u => u.ChangeDate).HasConversion(_dateWithNullConverter);
        }
        /// <summary>
        /// Настройки для модели
        /// </summary>
        /// <param name="builder"></param>
        public void BankAccountDocument(EntityTypeBuilder<BankAccountDocument> builder)
        {
            builder.Property(u => u.DateStart).HasConversion(_dateWithNullConverter);
            builder.Property(u => u.DateEnd).HasConversion(_dateWithNullConverter);
            builder.Property(u => u.ChangeDate).HasConversion(_dateWithNullConverter);
        }
        /// <summary>
        /// Настройки для модели
        /// </summary>
        /// <param name="builder"></param>
        public void PaymentOrder(EntityTypeBuilder<PaymentOrder> builder)
        {
            builder.Property(u => u.PaymentTerm).HasConversion(_dateWithNullConverter);
            builder.Property(u => u.Date).HasConversion(_dateWithNullConverter);
            builder.Property(u => u.ReceivedDate).HasConversion(_dateWithNullConverter);
            builder.Property(u => u.WriteOffDate).HasConversion(_dateWithNullConverter);
            builder.Property(u => u.ChangeDate).HasConversion(_dateWithNullConverter);
        }

        /// <summary>
        /// Настройки для модели
        /// </summary>
        /// <param name="builder"></param>
        public void IntegrationModule(EntityTypeBuilder<IntegrationModuleOperation> builder)
        {
            builder.Property(u => u.ChangeDate).HasConversion(_dateWithNullConverter);
        }

        /// <summary>
        /// Настройки для модели
        /// </summary>
        /// <param name="builder"></param>
        public void OperationTypeHistory(EntityTypeBuilder<OperationTypeHistory> builder)
        {
            builder.Property(u => u.ChangeDate).HasConversion(_dateWithNullConverter);
        }

        /// <summary>
        /// Настройки для модели
        /// </summary>
        /// <param name="builder"></param>
        public void OperationType(EntityTypeBuilder<OperationType> builder)
        {
            builder.Property(u => u.ChangeDate).HasConversion(_dateWithNullConverter);
        }

        /// <summary>
        /// Настройки для модели
        /// </summary>
        /// <param name="builder"></param>
        public void ImportResult(EntityTypeBuilder<ImportResult> builder)
        {
            builder.Property(u => u.ChangeDate).HasConversion(_dateWithNullConverter);
        }

        /// <summary>
        /// Настройки для модели
        /// </summary>
        /// <param name="builder"></param>
        public void TransferType(EntityTypeBuilder<TransferType> builder)
        {
            builder.Property(u => u.ChangeDate).HasConversion(_dateWithNullConverter);
        }
    }
}
