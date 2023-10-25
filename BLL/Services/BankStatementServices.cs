using IbZKH_CustomTypes.SingleTypes;
using BankStatementApi.BLL.Services.Interfaces;
using BankStatementApi.BLL.BusinessLogiс.Import;
using BankStatementApi.BLL.DTO.GetDTO;
using BankStatementApi.BLL.DTO.SetDTO;
using IbZKH_CustomTypes.GenericTypes;
using BankStatementApi.BLL.DTO.FilterDTO;
using BankStatementApi.BLL.Infrastructure;
using BankStatementApi.BLL.BusinessLogiс;
using BankStatementApi.BLL.BusinessLogic;

namespace BankStatementApi.BLL.Services
{
    /// <summary>
    /// Сервис для выписок из банка
    /// </summary>   
    public class BankStatementServices : IBankStatementServices
    {
        private readonly IServiceInfrastructure _serviceInfrastructure;
        #region CTOR
        public BankStatementServices(IServiceInfrastructure serviceInfrastructure)
        {
            _serviceInfrastructure = serviceInfrastructure;
        }
        #endregion

        /// <summary>
        /// Импорт файла банковской выписки
        /// </summary>
        public OperationResult ImportBankStatementFile(FileGetDTO parametr)
        {
            var importer = new ImportRunner(parametr, _serviceInfrastructure);
            return importer.SendDocumentInQueue();
        }

        /// <summary>
        /// Получить журнал данных по идентификтору
        /// </summary>
        public DataLogGetDTO GetDataLog(int dataLogId)
        {
            var importLogBl = new DataLogBl(dataLogId, _serviceInfrastructure);
            return _serviceInfrastructure.Mapper.Map<DataLogGetDTO>(source: importLogBl.DataLog);
        }

        /// <sammary>
        /// Получить имена файлов журнал регистрации данных по маске
        /// <sammary>
        public List<KeyValueItem<int>> GetDataLogFileNameByMask(string fileNameMask)
        {
            var importLogBl = new DataLogBl(_serviceInfrastructure);
            return importLogBl.GetDataLogFileNameByMask(fileNameMask);
        }

        /// <summary>
        /// Получить журналы данных
        /// </summary>
        public List<DataLogGetDTO> GetDataLogsByImportResult(DataLogsFilterDTO filter)
        {
            var importLogBl = new DataLogBl(_serviceInfrastructure);
            var dataLog = importLogBl.GetDataLogsByImportResult(filter);
            return _serviceInfrastructure.Mapper.Map<List<DataLogGetDTO>>(source: dataLog);
        }

        /// Перезапустить обработку записи журнала данных
        /// </summary>
        public OperationResult RestartDataLogProcessing(int dataLogId)
        {
            var importLogBl = new DataLogBl(dataLogId, _serviceInfrastructure);
            return importLogBl.RestartProcessing();
        }

        /// <summary>
        /// Удаление записи журнала данных
        /// </summary>
        public OperationResult DeleteDataLog(int dataLogId)
        {
            var importLogBl = new DataLogBl(dataLogId, _serviceInfrastructure);
            return importLogBl.DeleteDataLog();
        }

        /// <sammary>
        /// Получить журнал импорта по идентификтору
        /// </summary>
        public FileGetDTO GetImportLogFile(int dataLogId)
        {
            var importLogBl = new DataLogBl(dataLogId, _serviceInfrastructure);
            return importLogBl.GetImportLogFile();
        }

        /// <summary>
        /// Привязать платежное поручение к типу операции
        /// </summary>
        public OperationResult BindPaymentOrderToOperationType(PaymentOrdeToModuleSetDTO parametr)
        {
            var paymentOrderBl = new PaymentOrderBl(_serviceInfrastructure);
            return paymentOrderBl.BindPaymentOrderToOperationType(parametr);
        }

        /// <summary>
        /// Отвязать платежное поручение от типа операции
        /// </summary>
        public OperationResult UnBindPaymentOrderToOperationType(List<int> paymentOrderIds)
        {
            var paymentOrderBl = new PaymentOrderBl(_serviceInfrastructure);
            return paymentOrderBl.UnBindPaymentOrderToOperationType(paymentOrderIds);
        }

        /// <summary>
        /// Получение платежных поручений по ТипуОперации
        /// </summary>
        public List<PaymentOrderGetDTO> GetPaymentOrdersByOperationType(PaymentOrderByOperationTypeFilterDTO filter)
        {
            var paymentOrderBl = new PaymentOrderBl(_serviceInfrastructure);
            var paymentOrders = paymentOrderBl.GetPaymentOrdersByOperationType(filter);
            return _serviceInfrastructure.Mapper.Map<List<PaymentOrderGetDTO>>(source: paymentOrders).OrderBy(x => x.Id).ToList();
        }

        /// <summary>
        /// Получать платежные поручения по НомеруБанковскогоСчета
        /// </summary>    
        public List<PaymentOrderGetDTO> GetPaymentOrdersByBankAccountDocumentId(PaymentOrdersByBankAccountDocumentIdFilterDTO filter)
        {
            var paymentOrderBl = new PaymentOrderBl(_serviceInfrastructure);
            var paymentOrders = paymentOrderBl.GetPaymentOrdersByBankAccountDocumentId(filter);
            return _serviceInfrastructure.Mapper.Map<List<PaymentOrderGetDTO>>(source: paymentOrders).OrderBy(x => x.Id).ToList();
        }

        /// <summary>
        /// Получать платежные поручения по НомеруБанковскогоСчета
        /// </summary>
        public List<PaymentOrderGetDTO> GetPaymentOrdersByBankAccountNumber(PaymentOrdersByFilterDTO filter)
        {
            var paymentOrderBl = new PaymentOrderBl(_serviceInfrastructure);
            var paymentOrders = paymentOrderBl.GetPaymentOrdersByBankAccountNumber(filter);
            return _serviceInfrastructure.Mapper.Map<List<PaymentOrderGetDTO>>(source: paymentOrders).OrderBy(x => x.Id).ToList();
        }

        /// <summary>
        /// Получить отчет ПлатежныхПоручений за указанный период с указанием имени файла в диапазоне одного месяца
        /// </summary>
        public FileGetDTO GetPaymentOrdersWithinDays(PaymentOrdersByFilterDTO filter)
        {
            var paymentOrderBl = new PaymentOrderBl(_serviceInfrastructure);
            return paymentOrderBl.GetPaymentOrdersWithinDays(filter);          
        }

        /// <summary>
        /// Получить платежные поручения по фильтру
        /// </summary>
        public List<PaymentOrderGetDTO> GetPaymentOrdersByFilter(PaymentOrderByFeaturesFilterDTO filter)
        {
            var paymentOrderBl = new PaymentOrderBl(_serviceInfrastructure);
            var paymentOrders = paymentOrderBl.GetPaymentOrdersByFilter(filter);
            return _serviceInfrastructure.Mapper.Map<List<PaymentOrderGetDTO>>(source: paymentOrders);
        }

        /// <summary>
        /// Получить документы банковского счета по идентификатору журнала данных
        /// </summary>
        public List<BankAccountDocumentGetDTO> GetBankAccountDocumentsByDataLogId(int dataLogId)
        {
            var bankAccountDocumentBl = new BancAccountDocumentBl(_serviceInfrastructure);
            var dataLog = bankAccountDocumentBl.GetBankAccountDocumentsByDataLogId(dataLogId);
            return _serviceInfrastructure.Mapper.Map<List<BankAccountDocumentGetDTO>>(source: dataLog);
        }

        /// <summary>
        /// Получить документ о банковском счете
        /// </summary>
        public BankAccountDocumentGetDTO GetBankAccountDocument(int documentId)
        {
            var bankAccountDocumentBl = new BancAccountDocumentBl(_serviceInfrastructure);
            var document = bankAccountDocumentBl.GetBankAccountDocument(documentId);
            return _serviceInfrastructure.Mapper.Map<BankAccountDocumentGetDTO>(source: document);
        }

        /// <summary>
        /// Парсинг и сохранение данных
        /// </summary>
        public OperationResult ParsingAndSavingData(DataLogSetDTO datalog)
        {
            var importer = new ImportRunner(_serviceInfrastructure);
            return importer.TryParce(datalog);
        }      
    }
}
