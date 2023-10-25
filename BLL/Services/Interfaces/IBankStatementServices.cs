using BankStatementApi.BLL.DTO.FilterDTO;
using BankStatementApi.BLL.DTO.GetDTO;
using BankStatementApi.BLL.DTO.SetDTO;
using IbZKH_CustomTypes.GenericTypes;
using IbZKH_CustomTypes.SingleTypes;

namespace BankStatementApi.BLL.Services.Interfaces
{
    /// <summary>
    /// Интерфейс для сервиса выписок из банка
    /// </summary>
    public interface IBankStatementServices
    {
        /// <summary>
        /// Импорт файла банковской выписки
        /// </summary>
        OperationResult ImportBankStatementFile( FileGetDTO parametr );
        /// <summary>
        /// Получение платежных поручений по ТипуОперации
        /// </summary>
        List<PaymentOrderGetDTO> GetPaymentOrdersByOperationType( PaymentOrderByOperationTypeFilterDTO parametr );
        /// <summary>
        /// Получать платежные поручения по идентификатору ДокументаБанковскогоСчета
        /// </summary>
        List<PaymentOrderGetDTO> GetPaymentOrdersByBankAccountDocumentId( PaymentOrdersByBankAccountDocumentIdFilterDTO parametr );
        /// <summary>
        /// Получать платежные поручения по НомеруБанковскогоСчета
        /// </summary>
        List<PaymentOrderGetDTO> GetPaymentOrdersByBankAccountNumber( PaymentOrdersByFilterDTO parametr );
        /// <summary>
        /// Получить отчет ПлатежныхПоручений за указанный период в диапазоне одного месяца
        /// </summary>
        FileGetDTO GetPaymentOrdersWithinDays(PaymentOrdersByFilterDTO parametr);
        /// <summary>
        /// Получить отчет ПлатежныхПоручений за указанный период в диапазоне одного месяца
        /// </summary>
        BankAccountDocumentGetDTO GetBankAccountDocument( int id );
        /// <summary>
        /// Удаление записи журнала данных
        /// </summary>
        OperationResult DeleteDataLog( int id );
        /// <summary>
        /// Получить журналы данных
        /// </summary>
        List<DataLogGetDTO> GetDataLogsByImportResult(DataLogsFilterDTO parametr );
        /// <summary>
        /// Получить журнал данных по идентификтору
        /// </summary>
        DataLogGetDTO GetDataLog( int id );
        /// <summary>
        /// Перезапустить обработку записи журнала данных
        /// </summary>
        OperationResult RestartDataLogProcessing( int id );
        /// <summary>
        /// Привязать платежное поручение к типу операции
        /// </summary>
        OperationResult BindPaymentOrderToOperationType( PaymentOrdeToModuleSetDTO parametr);
        /// <summary>
        /// Отвязать платежное поручение от типа операции
        /// </summary>
        OperationResult UnBindPaymentOrderToOperationType( List<int> ids );
        /// <summary>
        /// Получить платежные поручения по фильтру
        /// </summary>
        List<PaymentOrderGetDTO> GetPaymentOrdersByFilter(PaymentOrderByFeaturesFilterDTO parametr);
        /// <summary>
        /// Парсинг и сохранение данных
        /// </summary>
        OperationResult ParsingAndSavingData(DataLogSetDTO dataLog);
        /// <summary>
        /// Получение журналов импорта
        /// </summary>
        FileGetDTO GetImportLogFile(int dataLogId);
        /// <summary>
        /// Получить имена файлов журнал регистрации данных по маске
        /// </summary>
        List<KeyValueItem<int>> GetDataLogFileNameByMask(string fileNameMask);
        /// <summary>
        /// Получить документы банковского счета по идентификатору журнала данных
        /// </summary>
        List<BankAccountDocumentGetDTO> GetBankAccountDocumentsByDataLogId(int dataLogId);
    }
}
