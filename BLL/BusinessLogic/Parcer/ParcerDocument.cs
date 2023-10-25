using BankStatementApi.BLL.BusinessLogiс.Import;
using BankStatementApi.DAL.Entities;
using System.ComponentModel.DataAnnotations;

namespace BankStatementApi.BLL.BusinessLogiс.Tools.ImportData.DataParcer
{
    /// <summary>
    /// Модель для разбора документа
    /// </summary>
    public class ParcerDocument
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Формат версии
        /// </summary>
        [ParcerProperty(new[] { "ВерсияФормата" })]
        [MaxLength(8, ErrorMessage = "Превышена макс длина для VersionFormat")]
        public string VersionFormat { get; set; }
        /// <summary>
        /// Кодировка
        /// </summary>
        [ParcerProperty(new[] { "Кодировка" })]
        [MaxLength(32, ErrorMessage = "Превышена макс длина для Encoding")]
        public string Encoding { get; set; }
        /// <summary>
        /// ДатаНачала
        /// </summary>
        [ParcerProperty(new[] { "ДатаНачала" })]
        public DateTime DateStart { get; set; }
        /// <summary>
        /// ДатаКонца
        /// </summary>
        [ParcerProperty(new[] { "ДатаКонца" })]
        public DateTime DateEnd { get; set; }
        /// <summary>
        /// Расчетный счет
        /// </summary>
        [ParcerProperty(new[] { "РасчСчет" })]
        public string CalcAccount { get; set; }
        /// <summary>
        /// Коллекция платежных поручений
        /// </summary>
        public List<PaymentOrder> PaymentOrders { get; set; }
        /// <summary>
        /// Коллекция банк аккаунтов
        /// </summary>
        public List<BankAccountDocument> BankAccounts { get; set; }

        #region Implementation of IImportValidationRule
        /// <summary>
        /// Валидация разобранных данных
        /// </summary>
        public ValidateResult ParcingValidate()
        {
            ValidateResult result = ValidateResult.Success();
            if (DateStart == DateTime.MinValue)
                result.AddMessage("Не распознан параметр ДатаНачала(DateStart)");
            if (DateEnd == DateTime.MinValue)
                result.AddMessage("Не распознан параметр ДатаКонца(DateEnd)");
            if (CalcAccount==null)
                result.AddMessage("Не распознан параметр РасчетныйСчет(CalcAccount)");
            if (BankAccounts==null)
                result.AddMessage("Не распознано ни одной СекцияРасчСчет.");
            else
                BankAccounts.ForEach(bankAccount => ParcingValidateBankAccount(bankAccount, result));
            if (PaymentOrders==null)
                result.AddMessage("Не распознано ни одной СекцияДокумент.");
            else
                PaymentOrders.ForEach(paymentOrder => ParcingValidatePaymentOrder(paymentOrder, result));
            return result;
        }

        /// <summary>
        /// Валидация платежного поручения
        /// </summary>
        public void ParcingValidatePaymentOrder(PaymentOrder paymentOrder, ValidateResult result)
        {
            if (paymentOrder.Number == null)
                result.AddMessage("Не распознана СекцияДокумент параметр Номер(Number)");
            if (paymentOrder.Date == DateTime.MinValue)
                result.AddMessage("Не распознана СекцияДокумент параметр Дата(Date)");
            if (paymentOrder.Sum == null)
                result.AddMessage("Не распознана СекцияДокумент параметр Сумма(Sum)");
            if (paymentOrder.PayerAccount == null)
                result.AddMessage("Не распознана СекцияДокумент параметр ПлательщикСчет(PayerAccount)");         
            if (paymentOrder.PayerCalcAccount == null)
                result.AddMessage("Не распознана СекцияДокумент параметр ПлательщикКорсчет(PayerCalcAccount)");
            if (paymentOrder.PayerBIK == null)
                result.AddMessage("Не распознана СекцияДокумент параметр ПлательщикБИК(PayerBIK)");          
            if (paymentOrder.RecipientAccount == null)
                result.AddMessage("Не распознана СекцияДокумент параметр ПолучательСчет(RecipientAccount)");
            if (paymentOrder.RecipientCalcAccount == null)
                result.AddMessage("Не распознана СекцияДокумент параметр ПолучательРасчСчет(RecipientCalcAccount)");
            if (paymentOrder.RecipientCorAccount == null)
                result.AddMessage("Не распознана СекцияДокумент параметр ПолучательКорсчет(RecipientCorAccount)");
        }

        /// <summary>
        /// Валидация банк аккаунта
        /// </summary>
        public void ParcingValidateBankAccount(BankAccountDocument bankAccount, ValidateResult result)
        {
            if (bankAccount.DateStart == DateTime.MinValue)
                result.AddMessage("Не распознана СекцияРасчСчет параметр ДатаНачала(DateStart)");
            if (bankAccount.DateEnd == DateTime.MinValue)
                result.AddMessage("Не распознана СекцияДокумент параметр ДатаКонца(DateEnd)");
            if (bankAccount.BankAccountNumber == null)
                result.AddMessage($"Не распознана СекцияДокумент параметр РасчСчет(BankAccountNumber).");
        }
        #endregion
    }
}
