using BankStatementApi.DAL.Entities.Enum;
using BankStatementApi.DAL.Entities.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace BankStatementApi.DAL.Entities
{
    /// <summary>
    /// ЖурналДанных
    /// </summary>
    public class DataLog : IBaseEntityFields<int>
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public int Id { get; set; }
        [MaxLength(255)]
        /// <summary>
        /// ИмяФайла
        /// </summary>
        public string FileName { get; set; }
        [MaxLength(36)]
        /// <summary>
        /// ХэшФайла
        /// </summary>
        public string FileHash { get; set; }
        /// <summary>
        /// ДлинаФайла
        /// </summary>
        public int FileSize { get; set; }
        /// <summary>
        /// ДатаНачалаЗагрузки
        /// </summary>
        public DateTime DateStartLoad { get; set; }
        /// <summary>
        /// ДатаКонцаЗагрузки
        /// </summary>
        public DateTime DateEndLoad { get; set; }
        /// <summary>
        /// ДатаНачалаФайла
        /// </summary>
        public DateTime? DateFileStart { get; set; }
        /// <summary>
        /// ДатаКонцаФайла
        /// </summary>
        public DateTime? DateFileEnd { get; set; }
        /// <summary>
        ///  ЖурналИмпорта
        /// </summary>
        public string ImportLog { get; set; }
        /// <summary>
        ///  ИдентификаторРезультатаИмпорта
        /// </summary>
        public ImportResultEnum ImportResultId { get; set; }
        /// <summary>
        ///  ИдентификаторРаботника
        /// </summary>
        public int? WorkerChangedById { get; set; }
        /// <summary>
        ///  ДатаИзменения
        /// </summary>
        public DateTime ChangeDate { get; set; }
      
        #region Navigation
        public virtual DataStorage DataStorage { get; set; }
        public virtual ICollection<BankAccountDocument> BankAccountDocuments { get; set; } = new List<BankAccountDocument>();
        #endregion
    }
}
