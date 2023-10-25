using BankStatementApi.BLL.BusinessLogic.Converter;
using BankStatementApi.BLL.DTO.FilterDTO;
using BankStatementApi.BLL.DTO.GetDTO;
using BankStatementApi.BLL.DTO.SetDTO;
using BankStatementApi.BLL.Infrastructure;
using BankStatementApi.DAL.Entities;
using IbZKH_CustomTypes.SingleTypes;
using System.Linq.Expressions;
using System.Text;

namespace BankStatementApi.BLL.BusinessLogic
{
    public class PaymentOrderBl
    {
        private readonly IServiceInfrastructure _serviceInfrastructure;

        #region CTOR
        public PaymentOrderBl( IServiceInfrastructure serviceInfrastructure)
        {
            _serviceInfrastructure = serviceInfrastructure;
        }
        #endregion

        /// <summary>
        /// Привязать платежное поручение к типу операции
        /// </summary>
        public OperationResult BindPaymentOrderToOperationType(PaymentOrdeToModuleSetDTO parametr)
        {
            var companyINN = _serviceInfrastructure.Configuration.GetSection("Project").GetSection("CompanyINN").Value;
            var result = new OperationResult();
            var operationType = _serviceInfrastructure.Repository.GetDictionaryRepository<OperationType, Guid>().Get(parametr.OperationTypeId);
            if (operationType == null)
            {
                return OperationResult.CreateErrorResult($"Вид операции отсутствует в БД.");
            }
            var paymentOrders = _serviceInfrastructure.Repository.GetRepository<PaymentOrder>().Where(x => parametr.PaymentOrderIds.Contains(x.Id));
            parametr.PaymentOrderIds.Except(paymentOrders.Select(paymentOrder => paymentOrder.Id)).ToList().ForEach(paymentOrderId => result.AddError($"П/П с идентификатором {paymentOrderId} отсутствует в БД."));

            foreach (var paymentOrder in paymentOrders)
            {
                if (paymentOrder.OperationTypeId.HasValue)
                {
                    result.AddError($"П/П №{paymentOrder.Number} на сумму {paymentOrder.Sum} ранее был назначен тип операции {_serviceInfrastructure.Repository.GetDictionaryRepository<OperationType, Guid>().Get(paymentOrder.OperationTypeId.Value).Title}.");
                    continue;
                }            
                paymentOrder.OperationType = operationType;
                paymentOrder.OperationTypeHistory.Add(
                    new OperationTypeHistory()
                    {
                        ChangeDate = DateTime.UtcNow,
                        WorkerChangedById = _serviceInfrastructure.IdentityProvider.WorkerId,
                        OperationType = operationType                  
                    });
                _serviceInfrastructure.Repository.GetRepository<PaymentOrder>().Update(paymentOrder);
                OperationResult.CreateSuccessResult($"П/П №{paymentOrder.Number} на сумму {paymentOrder.Sum} назначен тип операции {operationType.Title}");
            }
            return result;
        }

        /// <summary>
        /// Отвязать платежное поручение от типа операции
        /// </summary>
        public OperationResult UnBindPaymentOrderToOperationType(List<int> paymentOrderIds)
        {
            var errorResultMessage = new List<string>();
            var paymentOrders = _serviceInfrastructure.Repository.GetRepository<PaymentOrder>().Where(x => paymentOrderIds.Contains(x.Id)).ToList();
            paymentOrderIds.Except(paymentOrders.Select(x => x.Id)).ToList().ForEach(paymentOrderId => errorResultMessage.Add($"П/П с идентификатором {paymentOrderId} отсутствует в БД."));
            foreach (var paymentOrder in paymentOrders)
            {
                if (paymentOrder.OperationTypeId == null)
                {
                    errorResultMessage.Add($"П/П №{paymentOrder.Number} на сумму {paymentOrder.Sum} не назначен тип операции");
                }
                paymentOrder.OperationTypeId = null;
                paymentOrder.OperationTypeHistory.Add(
                    new OperationTypeHistory()
                    {
                        ChangeDate = DateTime.UtcNow,
                        WorkerChangedById = _serviceInfrastructure.IdentityProvider.WorkerId
                    });
                _serviceInfrastructure.Repository.GetRepository<PaymentOrder>().Update(paymentOrder);
            }
            return errorResultMessage.Any() ?
                OperationResult.CreateErrorResult(errorResultMessage.ToArray()) :
                OperationResult.CreateSuccessResult("Успешно");
        }

        /// <summary>
        /// Получение платежных поручений по ТипуОперации
        /// </summary>
        public IEnumerable<PaymentOrder> GetPaymentOrdersByOperationType(PaymentOrderByOperationTypeFilterDTO parametr)
        {
            return _serviceInfrastructure.Repository.GetRepository<PaymentOrder>()
                .Where(x => (parametr.OperationTypeIds == null || parametr.OperationTypeIds.Contains(x.OperationTypeId.Value)) && (x.Date >= parametr.DateStart.Date && x.Date <= parametr.DateEnd.Date));       
        }

        /// <summary>
        /// Получать платежные поручения по НомеруБанковскогоСчета
        /// </summary>    
        public IEnumerable<PaymentOrder> GetPaymentOrdersByBankAccountDocumentId(PaymentOrdersByBankAccountDocumentIdFilterDTO parametr)
        {
            return _serviceInfrastructure.Repository.GetRepository<PaymentOrder>()
                .Where(x => x.BankAccountDocumentId == parametr.BankAccountDocumentId && (x.Date >= parametr.DateStart.Date && x.Date <= parametr.DateEnd.Date));        
        }

        /// <summary>
        /// Получать платежные поручения по НомеруБанковскогоСчета
        /// </summary>
        public IEnumerable<PaymentOrder> GetPaymentOrdersByBankAccountNumber(PaymentOrdersByFilterDTO filter)
        {
            return _serviceInfrastructure.Repository.GetRepository<PaymentOrder>()
                .Where(BuildPaymentOrderPredicate(filter));        
        }

        /// <summary>
        /// Получить платежные поручения по фильтру
        /// </summary>
        public IEnumerable<PaymentOrder> GetPaymentOrdersByFilter(PaymentOrderByFeaturesFilterDTO filter)
        {
            return _serviceInfrastructure.Repository.GetRepository<PaymentOrder>()
                .Where(BuildPaymentOrderPredicate(filter));
        }

        /// <summary>
        /// Получить отчет ПлатежныхПоручений за указанный период с указанием имени файла в диапазоне одного месяца
        /// </summary>
        public FileGetDTO GetPaymentOrdersWithinDays(PaymentOrdersByFilterDTO filter)
        {
            var paymentOrders = _serviceInfrastructure.Repository.GetRepository<PaymentOrder>().Where(BuildPaymentOrderPredicate(filter));
            var paymentOrdersDTO = _serviceInfrastructure.Mapper.Map<List<PaymentOrdersWithinDaysGetDTO>>(source: paymentOrders);
            var fileCsv = Converter<PaymentOrdersWithinDaysGetDTO>.CsvConverter(paymentOrdersDTO);
            return new FileGetDTO
            {
                FileName = "pay_fkr_" + filter.DateStart.ToString("MM.yyyy") + ".csv",
                FileData = Encoding.UTF8.GetBytes(fileCsv)
            };
        }

        #region Private 
        private static Expression<Func<PaymentOrder, bool>> BuildPaymentOrderPredicate(PaymentOrdersByFilterDTO filter)
        {
            return paymnet =>
                    (filter.OperationTypeId == null || paymnet.OperationTypeId == filter.OperationTypeId) &&
                    (paymnet.Date >= filter.DateStart && paymnet.Date <= filter.DateEnd) &&
                    (filter.DataLogId == null || paymnet.BankAccountDocument.DataLogId == filter.DataLogId) &&
                    (filter.BankAccountNumber == null || paymnet.BankAccountDocument.BankAccountNumber == filter.BankAccountNumber);
        }


        private static Expression<Func<PaymentOrder, bool>> BuildPaymentOrderPredicate(PaymentOrderByFeaturesFilterDTO filter)
        {
            return paymnet =>
                    (filter.PayerBankAccount == null || paymnet.PayerAccount == filter.PayerBankAccount) &&
                    (filter.PayerKPP == null || paymnet.PayerKPP == filter.PayerKPP) &&
                    (filter.PayerINN == null || paymnet.PayerINN == filter.PayerINN) &&
                    (filter.RecipientBankAccount == null || paymnet.RecipientAccount == filter.RecipientBankAccount) &&
                    (filter.RecipientKPP == null || paymnet.RecipientKPP == filter.PayerKPP) &&
                    (filter.RecipientINN == null || paymnet.RecipientINN == filter.PayerINN) &&
                    (filter.Number == null || paymnet.Number == filter.Number) &&
                    (filter.Sum == null || paymnet.Sum == filter.Sum) &&
                    (filter.DateFrom == null || paymnet.Date >= filter.DateFrom) &&
                    (filter.DateTo == null || paymnet.Date <= filter.DateTo);
        }
        #endregion
    }
}
