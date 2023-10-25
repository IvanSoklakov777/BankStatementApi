using BankStatementApi.BLL.BusinessLogiс.Tools.ImportData.DataParcer;
using BankStatementApi.DAL.Entities.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace BankStatementApi.DAL.Entities
{
    /// <summary>
    /// Модель БанкАккаунтДокумент
    /// </summary>
    [ClientBankParcerSection("СекцияРасчСчет" , "КонецРасчСчет")]
    public class BankAccountDocument : IBaseEntityFields<int>
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// РасчетныйСчет
        /// </summary>
        [ParcerProperty(new [] { "РасчСчет" })]   
        [StringLength(20 , MinimumLength = 20, ErrorMessage = "Размер должен быть 20 цифр.")]
        public string BankAccountNumber { get; set; }
        /// <summary>
        /// ДатаНачала
        /// </summary>
        [ParcerProperty(new [] { "ДатаНачала" })]
        public DateTime DateStart { get; set; }
        /// <summary>
        /// ДатаКонца
        /// </summary>
        [ParcerProperty(new [] { "ДатаКонца" })]
        public DateTime DateEnd { get; set; }
        /// <summary>
        /// НачальныйОстаток
        /// </summary>
        [ParcerProperty(new [] { "НачальныйОстаток" })]
        public decimal StartBalance { get; set; }
        /// <summary>
        /// ВсегоПоступило
        /// </summary>
        [ParcerProperty(new [] { "ВсегоПоступило" })]
        public decimal TotalReceived { get; set; }
        /// <summary>
        /// ВсегоСписанно
        /// </summary>
        [ParcerProperty(new [] { "ВсегоСписано" })]
        public decimal TotalCharged { get; set; }
        /// <summary>
        /// КонечныйОстаток
        /// </summary>
        [ParcerProperty(new [] { "КонечныйОстаток" })]
        public decimal FinalBalance { get; set; }
        /// <summary>
        /// Идентификатор РасчетногоСчета из внешенго сервиса
        /// </summary>
        public int? BankAccountId { get; set; }
        /// <summary>
        /// Идентификатор работника
        /// </summary>
        public int? WorkerChangedById { get; set; }
        /// <summary>
        /// Дата изменения
        /// </summary>
        public DateTime ChangeDate { get; set; }

        #region Navigation
        /// <summary>
        /// Навигационное свойство ПлатежногоПоручения
        /// </summary>
        public virtual ICollection<PaymentOrder> PaymentOrders { get; set; } = new List<PaymentOrder>();
        /// <summary>
        /// Внешний ключ ЖурналаДанных
        /// </summary>
        public int DataLogId { get; set; }
        /// <summary>
        /// Навигационное свойство ЖурналаДанных
        /// </summary>
        public virtual DataLog DataLog { get; set; }
        #endregion
    }
}

