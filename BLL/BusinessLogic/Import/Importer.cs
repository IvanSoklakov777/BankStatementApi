using IbZKH_CustomTypes.SingleTypes;
using IbZKH_CustomTypes.Constants;
using System.Text;
using BankStatementApi.BLL.BusinessLogiс.Tools.ImportData.DataParcer;
using BankStatementApi.BLL.Infrastructure.Extensions;
using BankStatementApi.DAL.Entities;
using System.Text.RegularExpressions;
using BankStatementApi.DAL.Entities.Enum;
using BankStatementApi.BLL.Infrastructure;
using BankStatementApi.BLL.BusinessLogic.Event.Model;
using BankStatementApi.BLL.DTO.SetDTO;

namespace BankStatementApi.BLL.BusinessLogiс.Import
{
    /// <summary>
    /// Клиент-Банк импортер
    /// </summary>  
    public class Importer
    {
        private const string AddMessageConst = "Просмотрите журнал для получения более полной информации.";
        public DataLogBl DataLogBl { get; set; }
        private readonly IServiceInfrastructure _serviceInfrastructure;

        #region CTOR      
        public Importer(DataLogBl importerLog, IServiceInfrastructure serviceInfrastructure)
        {
            DataLogBl = importerLog;
            _serviceInfrastructure = serviceInfrastructure;

        }
        #endregion

        #region Parcing & Save
        /// <summary>
        /// Очистить данные парсинга от реализации модели
        /// </summary>  
        public void ClearParcingDataFromModelImpl()
        {
            _serviceInfrastructure.Repository.GetRepository<BankAccountDocument>().Delete(x => x.DataLogId == DataLogBl.Id);
            _serviceInfrastructure.Repository.GetRepository<PaymentOrder>().Delete(x => x.BankAccountDocument.DataLogId == DataLogBl.Id);
        }

        /// <summary>
        /// Старт парсинга
        /// </summary>  
        public OperationResult StartParcing(DataLogSetDTO datalog)
        {
            UpdateLog("Начало разбора файла");
            ParcerDocument parcingObject;
            try
            {
                parcingObject = ParceDataImplementation(datalog.FileData, _serviceInfrastructure);
            }
            catch (Exception ex)
            {
                UpdateLog($"Ошибка при разборе (парсинге) файла {DevelopmentConstants.StringConstant.NewLine}{ex.GetFullExceptionMessage()}", ImportResultEnum.ProcessedWithError);
                return OperationResult.CreateErrorResult($"Ошибка при разборе (парсинге) файла {DataLogBl.ModelWrapper.ModelInstance.FileName}. {AddMessageConst}");
            }
            UpdateLog("Завершение разбора файла");
            UpdateLog("Начало проверки модели данных");
            ValidateResult parcingValidate;
            try
            {
                parcingValidate = parcingObject.ParcingValidate();
            }
            catch (Exception ex)
            {
                parcingValidate = ValidateResult.Failed($"При проверке модели данных произошла неизвестная ошибка:{Environment.NewLine}{ex.Message}.");
            }
            var parseResult = parcingValidate.HasError
                ? ImportResultEnum.ProcessedWithError
                : ImportResultEnum.FileInProcessing;

            if (parcingValidate.HasError)
            {
                UpdateLog(parcingValidate.ToString(), parseResult);
                UpdateLog("Ошибка при проверке модели.");
                return OperationResult.CreateErrorResult("Ошибка при проверке модели.");
            }
            UpdateLog("Завершена проверка модели данных");

            UpdateLog("Начало определение типа операции ПП");

            var resultAssignmentOperationType = IdentificateOperationType(parcingObject.PaymentOrders);
            if (resultAssignmentOperationType.HasError)
            {
                return resultAssignmentOperationType;
            }
            UpdateLog("Завершение определение типа операции ПП");

            UpdateLog("Начало определения идентификатора банк акаунта по номеру расчетного счета.");
            var operationResult = IdentificateBankAccountId(parcingObject);
            if (operationResult.HasError)
            {
                UpdateLog(operationResult.GetErrors());
            }
            UpdateLog("Завершение определения идентификатора банк акаунта по номеру расчетного счета");

            UpdateLog("Добавление даты периода документа", parcingObject);
            try
            {
                UpdateLog("Начало сохранения данных в модель");
                SaveDataToModelImpl(parcingObject, datalog.WorkerId);
                UpdateLog("Завершение сохранения импортируемых данных в модель", ImportResultEnum.ProcessedSuccessfully);
            }
            catch (Exception ex)
            {
                UpdateLog($"Ошибка при сохранении данных в модель{DevelopmentConstants.StringConstant.NewLine}{ex.GetFullExceptionMessage()}", ImportResultEnum.ProcessedWithError);
                return OperationResult.CreateErrorResult($"Ошибка при сохранении данных в модель {AddMessageConst}.");
            }

            try
            {
                UpdateLog("Начало публикации ПП в RMQ");
                var bankAccountIds = parcingObject.BankAccounts.Select(x => x.Id).ToList();
                PublishPaymentOrdersByBankAccountsIds(bankAccountIds, datalog.WorkerId, datalog.LegalEntityId);
                UpdateLog("Завершение публикации ПП в RMQ");
            }
            catch (Exception ex)
            {
                UpdateLog($"Ошибка публикации ПП в RMQ {DevelopmentConstants.StringConstant.NewLine}{ex.GetFullExceptionMessage()}", ImportResultEnum.ProcessedWithError);
                return OperationResult.CreateErrorResult($"Ошибка публикации ПП в RMQ {AddMessageConst}.");
            }

            return OperationResult.CreateSuccessResult("Разбор и сохранение данных завершенно успешно.");
        }

        /// <summary>
        /// Публикуем ПП в RMQ для сервисов обрабатываемых конкретные платежи
        /// </summary>  
        private void PublishPaymentOrdersByBankAccountsIds(List<int> bankAccountIds, int? workerId, int? legalEntityId)
        {
            var paymentOrders = _serviceInfrastructure.Repository.GetRepository<PaymentOrder>().Where(payment => bankAccountIds.Contains(payment.BankAccountDocument.Id)).ToList();
            var sendPaymentOrders = paymentOrders.Select(payment =>
               new PaymentOrderEvent(workerId, legalEntityId)
               {
                   PaymentId = payment.Id,
                   Number = payment.Number,
                   Sum = payment.Sum,
                   Date = payment.Date,
                   PayerINN = payment.PayerINN,
                   PayerAccount = payment.PayerAccount,
                   RecipientINN = payment.RecipientINN,
                   RecipientAccount = payment.RecipientAccount,
                   PaymentPurpose = payment.PaymentPurpose,
                   IntegrationModuleOperationId = payment.IntegrationModuleOperationId                 
               }).ToList();
            sendPaymentOrders.ForEach(payment => _serviceInfrastructure.Producer.GetProducer<PaymentOrderEvent>().Publish(payment));     
        }

        /// <summary>
        /// Назначение типа операции платежным поручениям
        /// </summary>  
        private OperationResult IdentificateOperationType(List<PaymentOrder> paymentOrders)
        {
            try
            {
                var operationTypes = _serviceInfrastructure.Repository.GetRepository<OperationType>().Where();
                var integrationModuleOperationIds = GetIntegrationModuleOperationIds(paymentOrders);
                var paymentOrderWithIdList = GetPaymentOrdersWithIntegrationModuleOperationIds(integrationModuleOperationIds);
                var integrationModuleOperationList = _serviceInfrastructure.Repository.GetRepository<IntegrationModuleOperation>().Where(x => integrationModuleOperationIds.Contains(x.Id)).ToList();
               
                foreach (var paymentOrder in paymentOrders)
                {
                    var integrationModuleOperationId = GetKeyByMask<int>(paymentOrder.PaymentPurpose);
                    var pp = paymentOrderWithIdList?.FirstOrDefault(x => x.IntegrationModuleOperationId == integrationModuleOperationId);
                    if (pp != null)
                    {
                        UpdateLog($"ПП №{paymentOrder.Number} на сумму {paymentOrder.Sum} от {paymentOrder.Date.ToShortDateString()}, указан уникальный идентификатор который уже присвоен другому ПП  №{pp.Number} на сумму {pp.Sum} от {pp.Date}.");
                        continue;
                    }
                    var integrationModuleOperation = integrationModuleOperationList.FirstOrDefault(x => x.Id == integrationModuleOperationId);
                    if (integrationModuleOperation == null)
                    {
                        integrationModuleOperation = _serviceInfrastructure.Repository.GetRepository<IntegrationModuleOperation>().SingleOrDefault(x => x.Id == integrationModuleOperationId);
                    }
                    if (integrationModuleOperation == null)
                    {
                        UpdateLog($"ПП №{paymentOrder.Number} на сумму {paymentOrder.Sum} от {paymentOrder.Date.ToShortDateString()}, в назначении платежа указан идентификатор операции:{integrationModuleOperationId} который не зарегестрирован в системе.");
                        continue;
                    }                  
                    paymentOrder.OperationTypeId = integrationModuleOperation.OperationTypeId;
                    paymentOrder.IntegrationModuleOperationId = integrationModuleOperationId;
                }
                return OperationResult.CreateSuccessResult();
            }
            catch (Exception ex)
            {
                UpdateLog($"Ошибка при присвоении вида операции ПП {DevelopmentConstants.StringConstant.NewLine}{ex.GetFullExceptionMessage()}.", ImportResultEnum.ProcessedWithError);
                return OperationResult.CreateErrorResult($"Ошибка при определении типа операции ПП {AddMessageConst}.");
            }
        }

        /// <summary>
        /// Получить платежные поручения по списку идентификторов вида операции
        /// </summary> 
        private List<PaymentOrder> GetPaymentOrdersWithIntegrationModuleOperationIds(List<int> integrationModuleOperationIds)
        {
            return _serviceInfrastructure.Repository.GetRepository<PaymentOrder>()
                    .Where(x => x.IntegrationModuleOperationId != null)
                    .Where(x => integrationModuleOperationIds.Contains(x.IntegrationModuleOperationId.Value))
                    .ToList();
        }

        /// <summary>
        /// Получить идентификаторы вида операций
        /// </summary>  
        private List<int> GetIntegrationModuleOperationIds(List<PaymentOrder> paymentOrders)
        {
            var integrationModuleOperationIds = new List<int>();
            foreach (var paymentOrder in paymentOrders)
            {
                var integrationModuleOperationId = GetKeyByMask<int>(paymentOrder.PaymentPurpose);
                if (integrationModuleOperationId == default)
                {
                    UpdateLog($"ПП №{paymentOrder.Number} на сумму {paymentOrder.Sum} от {paymentOrder.Date.ToShortDateString()}, не найден уникальный идентификатор операции.");
                    continue;
                }
                integrationModuleOperationIds.Add(GetKeyByMask<int>(paymentOrder.PaymentPurpose));
            }
            return integrationModuleOperationIds;
        }

        /// <summary>
        /// Получение ключа по маске из строки
        /// </summary>  
        private static T GetKeyByMask<T>(string str)
        {
            var regex = new Regex(@"(?<=\*)(.+?)(?=\*)");
            var result = regex.Match(str).Value;
            return result.Convert<T>();
        }

        /// <summary>
        /// Парсинг и анализ данных
        /// </summary>  
        private static ParcerDocument ParceDataImplementation(byte[] fileData, IServiceInfrastructure serviceInfrastructure)
        {
            var str = Encoding.GetEncoding(serviceInfrastructure.Encoding).GetString(fileData);
            var res = ClientBankParcer.FillBankAccountStatement(str);
            return res;
        }

        /// <summary>
        /// Сохранение данных для реализации модели
        /// </summary>  
        private OperationResult SaveDataToModelImpl(ParcerDocument parcingObject, int? workerId)
        {
            var bankAccounts = parcingObject.BankAccounts;
            var paymentOrders = GetUniquePaymentOrders(parcingObject);
            foreach (var bankAccount in bankAccounts)
            {
                bankAccount.DataLog = DataLogBl.DataLog;
                bankAccount.ChangeDate = DateTime.UtcNow;
                bankAccount.WorkerChangedById = workerId;
            }
            _serviceInfrastructure.Repository.GetRepository<BankAccountDocument>().AddRange(bankAccounts);
           

            foreach (var bankAccount in bankAccounts)
            {
                foreach (var paymentOrder in paymentOrders)
                {
                    if (bankAccount.BankAccountNumber == paymentOrder.PayerAccount || bankAccount.BankAccountNumber == paymentOrder.RecipientAccount)
                    {
                        paymentOrder.BankAccountDocumentId = bankAccount.Id;
                        paymentOrder.ChangeDate = DateTime.UtcNow;
                        paymentOrder.WorkerChangedById = workerId;
                    }
                }
            }
            _serviceInfrastructure.Repository.GetRepository<PaymentOrder>().BulkInsert(paymentOrders);
            return OperationResult.CreateSuccessResult();
        }

        /// <summary>
        /// Получение уникальных ПлатежныхПоручений
        /// </summary>  
        private List<PaymentOrder> GetUniquePaymentOrders(ParcerDocument parcerDocument)
        {
            var PaymentOrderResult = new List<PaymentOrder>();
            var bancAccountDocuments = _serviceInfrastructure.Repository.GetRepository<BankAccountDocument>()
                .Where(x => x.DateStart <= parcerDocument.DateStart && x.DateEnd >= parcerDocument.DateStart).Select(x=>x.Id).ToList();          
            var paymentOrders = _serviceInfrastructure.Repository.GetRepository<PaymentOrder>()
                   .Where(p => bancAccountDocuments.Contains(p.BankAccountDocumentId.Value)).ToList();

            foreach (var paymentOrder in parcerDocument.PaymentOrders)
            {
                if (!paymentOrders.Contains(paymentOrder))
                {
                    PaymentOrderResult.Add(paymentOrder);
                }
            }
            return PaymentOrderResult;
        }

        /// <summary>
        /// Определить идентификатор банк акаунтов по номеру расчетного счета
        /// </summary>  
        private OperationResult IdentificateBankAccountId(ParcerDocument parcerDocument)
        {
            try
            {
                var bankAccountNumbers = parcerDocument.BankAccounts.Select(x => x.BankAccountNumber).ToList();
                var result = GetBankAccountIdByBankAccountNumbers(bankAccountNumbers).Result;               
                foreach (var bankAccount in parcerDocument.BankAccounts)
                {
                    if (result.ContainsKey(bankAccount.BankAccountNumber))
                    {
                        bankAccount.BankAccountId = result[bankAccount.BankAccountNumber];
                    }
                    else
                    {
                        UpdateLog($"Не найден идентификатор счета по номеру расчетного счета {bankAccount.BankAccountNumber} ");
                    }
                }
                return OperationResult.CreateSuccessResult();
            }
            catch(Exception ex)
            {
                return OperationResult.CreateErrorResult(ex.Message);
            }                    
        }

        /// <summary>
        /// Получить идентификатор банк акаунта по номеру расчетного счета
        /// </summary>  
        private async Task<Dictionary<string,int>> GetBankAccountIdByBankAccountNumbers(List<string> bankAccountNumbers)
        {
            try
            {
                var sectionRequestName = "LegalEntityCoreUrl";
                var path = "api/bank/BankAccount/BankAccountIdsByNumbers";
                var requester = new Requester(_serviceInfrastructure);
                return await requester.PostRequest<Dictionary<string, int>>(sectionRequestName, path, bankAccountNumbers);
            }
              catch(Exception ex)
            {
                throw new Exception("Ошибка получения идентификатора счета по номеру расчетного счета", ex);
            }
        }
        #endregion

        #region Import Log Tools
        /// <summary>
        /// Обнорвление журнала
        /// </summary>  
        protected void UpdateLog(string message) => DataLogBl.UpdateLog(message);
        /// <summary>
        /// Обнорвление журнала
        /// </summary>  
        protected void UpdateLog(string message, ImportResultEnum importResult) => DataLogBl.UpdateLog(message, importResult);
        /// <summary>
        /// Обнорвление журнала
        /// </summary>  
        protected void UpdateLog(string messege, ParcerDocument parcerDocument) => DataLogBl.UpdateLog(messege, parcerDocument);
        #endregion        
    }
}
