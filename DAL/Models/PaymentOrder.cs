using BankStatementApi.BLL.BusinessLogiс.Tools.ImportData.DataParcer;
using BankStatementApi.DAL.Entities.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace BankStatementApi.DAL.Entities
{
    /// <summary>
    /// Платежное поручение 
    /// </summary>
    public class PaymentOrder : IBaseEntityFields<int>, IEquatable<PaymentOrder>
    {
        /// <summary>
        /// Идентификтор 
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Номер 
        /// </summary>
        [ParcerProperty(new[] { "Номер" })]
        public string Number { get; set; }
        /// <summary>
        /// Дата 
        /// </summary>
        [ParcerProperty(new[] { "Дата" })]
        public DateTime Date { get; set; }
        /// <summary>
        /// Сумма
        /// </summary>
        [ParcerProperty(new[] { "Сумма" })]
        public decimal Sum { get; set; }

        #region Плательщик  
        /// <summary>
        /// Плательщик 
        /// </summary>
        [ParcerProperty(new[] { "Плательщик", "Плательщик1", "Плательщик2" })]
        [MaxLength(512)]
        public string Payer { get; set; }
        /// <summary>
        /// ПлательщикСчет
        /// </summary>
        [ParcerProperty(new[] { "ПлательщикСчет" })]
        [MaxLength(20)]
        public string PayerAccount { get; set; }//PayerBankAccount
        /// <summary>
        /// ПлательщикКорсчет
        /// </summary>
        [ParcerProperty(new[] { "ПлательщикКорсчет" })]
        [MaxLength(20)]
        public string PayerCorAccount { get; set; }
        /// <summary>
        /// ПлательщикРасчСчет 
        /// </summary>
        [ParcerProperty(new[] { "ПлательщикРасчСчет" })]
        [MaxLength(20)]
        public string PayerCalcAccount { get; set; }
        /// <summary>
        /// ПлательщикБанк
        /// </summary>
        [ParcerProperty(new[] { "ПлательщикБанк1", "ПлательщикБанк2" })]
        [MaxLength(512)]
        public string PayerBank { get; set; }
        /// <summary>
        /// ПлательщикИНН 
        /// </summary>
        [ParcerProperty(new[] { "ПлательщикИНН" })]
        [MaxLength(12)]
        public string PayerINN { get; set; }
        /// <summary>
        /// ПлательщикБИК 
        /// </summary>
        [ParcerProperty(new[] { "ПлательщикБИК" })]
        [MaxLength(9)]
        public string PayerBIK { get; set; }
        /// <summary>
        /// ПлательщикКПП
        /// </summary>
        [ParcerProperty(new[] { "ПлательщикКПП" })]
        [MaxLength(9)]
        public string PayerKPP { get; set; }
        #endregion
        #region Получатель   
        /// <summary>
        /// Получатель
        /// </summary>
        [ParcerProperty(new[] { "Получатель1" })]
        [MaxLength(512)]
        public string Recipient { get; set; }
        /// <summary>
        /// ПолучательСчет 
        /// </summary>
        [ParcerProperty(new[] { "ПолучательСчет" })]
        [MaxLength(20)]
        public string RecipientAccount { get; set; }//RecipientBankAccount
        /// <summary>
        /// ПолучательИНН
        /// </summary>
        [ParcerProperty(new[] { "ПолучательИНН" })]
        [MaxLength(12)]
        public string RecipientINN { get; set; }
        /// <summary>
        /// ПолучательБИК
        /// </summary>
        [ParcerProperty(new[] { "ПолучательБИК" })]
        [MaxLength(9)]
        public string RecipientBIK { get; set; }
        /// <summary>
        /// ПолучательКПП
        /// </summary>
        [ParcerProperty(new[] { "ПолучательКПП" })]
        [MaxLength(9)]
        public string RecipientKPP { get; set; }
        /// <summary>
        /// ПолучательКорсчет 
        /// </summary>
        [ParcerProperty(new[] { "ПолучательКорсчет" })]
        [MaxLength(20)]
        public string RecipientCorAccount { get; set; }
        /// <summary>
        /// ПолучательРасчСчет 
        /// </summary>
        [ParcerProperty(new[] { "ПолучательРасчСчет" })]
        [MaxLength(20)]
        public string RecipientCalcAccount { get; set; }
        /// <summary>
        /// ПолучательБанк 
        /// </summary>
        [ParcerProperty(new[] { "ПлательщикБанк1", "ПлательщикБанк2" })]
        [MaxLength(512)]
        public string RecipientBank { get; set; }
        #endregion

        /// <summary>
        /// ДатаПоступило 
        /// </summary>
        [ParcerProperty(new[] { "ДатаПоступило" })]
        public DateTime? ReceivedDate { get; set; }
        /// <summary>
        /// ДатаСписано 
        /// </summary>
        [ParcerProperty(new[] { "ДатаСписано" })]
        public DateTime? WriteOffDate { get; set; }
        /// <summary>
        /// ВидОплаты 
        /// </summary>
        [ParcerProperty(new[] { "ВидОплаты" })]
        [MaxLength(5)]
        public string PaymentType { get; set; }
        /// <summary>
        /// СрокПлатежа 
        /// </summary>
        [ParcerProperty(new[] { "СрокПлатежа" })]
        public DateTime PaymentTerm { get; set; }
        /// <summary>
        /// Очередность 
        /// </summary>
        [ParcerProperty(new[] { "Очередность" })]
        public string Priority { get; set; }
        /// <summary>
        /// НазначениеПлатежа 
        /// </summary>
        [ParcerProperty(new[] { "НазначениеПлатежа" })]
        public string PaymentPurpose { get; set; }
        /// <summary>
        /// Идентификтор работника
        /// </summary>
        public int? WorkerChangedById { get; set; }
        /// <summary>
        /// Дата изменения
        /// </summary>
        public DateTime ChangeDate { get; set; }

        #region Navigation
        /// <summary>
        /// Идентификтор ТипаОперации
        /// </summary>
        public Guid? OperationTypeId { get; set; }
        /// <summary>
        /// Навигационное свойство ТипаОперации
        /// </summary>
        public virtual OperationType OperationType { get; set; }
         /// <summary>
        /// Идентификтор 
        /// </summary>
        public int? IntegrationModuleOperationId { get; set; }
        /// <summary>
        /// Навигационное свойство
        /// </summary>
        public virtual IntegrationModuleOperation IntegrationModuleOperation { get; set; }  
        /// <summary>
        /// Идентификтор ДокументаБанковскогоСчета
        /// </summary>
        public int? BankAccountDocumentId { get; set; }
        /// <summary>
        /// Навигационное свойство ДокументаБанковскогоСчета
        /// </summary>
        public virtual BankAccountDocument BankAccountDocument { get; set; }  
        /// <summary>
        /// Навигационное свойство ИсторииТипаОперации
        /// </summary>
        public virtual ICollection<OperationTypeHistory> OperationTypeHistory { get; set; } = new List<OperationTypeHistory>();
        #endregion


        #region OverrideMethods
        public override bool Equals(object obj)
        {
            if (obj is PaymentOrder paymentOrder)
                return Equals(paymentOrder);
            return false;
        }

        public bool Equals(PaymentOrder paymentOrder)
        {
            return  Number == paymentOrder.Number &&
                    Date == paymentOrder.Date &&
                    Sum == paymentOrder.Sum &&
                    PayerAccount == paymentOrder.PayerAccount &&
                    PayerINN == paymentOrder.PayerINN &&
                    PayerKPP == paymentOrder.PayerKPP &&
                    PayerCalcAccount == paymentOrder.PayerCalcAccount &&
                    PayerBIK == paymentOrder.PayerBIK &&
                    PayerCorAccount == paymentOrder.PayerCorAccount &&
                    RecipientAccount == paymentOrder.RecipientAccount &&
                    RecipientINN == paymentOrder.RecipientINN &&
                    RecipientKPP == paymentOrder.RecipientKPP &&
                    RecipientCalcAccount == paymentOrder.RecipientCalcAccount &&
                    RecipientBIK == paymentOrder.RecipientBIK &&
                    RecipientCorAccount == paymentOrder.RecipientCorAccount;
        }

        public override int GetHashCode()
        {
            int hash = 5;
            hash = 37 * hash + (this.Number != null ? this.Number.GetHashCode() : 0);
            hash = 37 * hash + (this.Date != null ? this.Date.GetHashCode() : 0);
            hash = 37 * hash + (this.Sum != null ? this.Sum.GetHashCode() : 0);
            hash = 37 * hash + (this.PayerAccount != null ? this.PayerAccount.GetHashCode() : 0);
            hash = 37 * hash + (this.PayerINN != null ? this.PayerINN.GetHashCode() : 0);
            hash = 37 * hash + (this.PayerKPP != null ? this.PayerKPP.GetHashCode() : 0);
            hash = 37 * hash + (this.PayerCalcAccount != null ? this.PayerCalcAccount.GetHashCode() : 0);
            hash = 37 * hash + (this.PayerBIK != null ? this.PayerBIK.GetHashCode() : 0);
            hash = 37 * hash + (this.PayerCorAccount != null ? this.PayerCorAccount.GetHashCode() : 0); 
            hash = 37 * hash + (this.Number != null ? this.Number.GetHashCode() : 0);
            hash = 37 * hash + (this.RecipientAccount != null ? this.RecipientAccount.GetHashCode() : 0);
            hash = 37 * hash + (this.RecipientINN != null ? this.RecipientINN.GetHashCode() : 0);
            hash = 37 * hash + (this.RecipientKPP != null ? this.RecipientKPP.GetHashCode() : 0);
            hash = 37 * hash + (this.RecipientCalcAccount != null ? this.RecipientCalcAccount.GetHashCode() : 0); 
            hash = 37 * hash + (this.Number != null ? this.Number.GetHashCode() : 0);
            hash = 37 * hash + (this.RecipientBIK != null ? this.RecipientBIK.GetHashCode() : 0);
            hash = 37 * hash + (this.RecipientCorAccount != null ? this.RecipientCorAccount.GetHashCode() : 0);
            return hash;
        }
        #endregion
    }
}