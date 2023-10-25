using BankStatementApi.DAL.Entities.Interfaces;

namespace BankStatementApi.DAL.Entities
{
    /// <summary>
    /// ХранилищеДанных
    /// </summary>
    public class DataStorage: IBaseEntityFields<int>
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Двоичное представление загружаемого документа
        /// </summary>
        public byte [] BinaryContent { get; set; }
        /// <summary>
        /// Идентификатор работника
        /// </summary>
        public int? WorkerChangedById { get; set; }
        /// <summary>
        /// ДатаИзменения
        /// </summary>
        public DateTime ChangeDate { get; set; }

        #region Navigation
        /// <summary>
        /// Внешний ключ ИмпортЖурналаДанных
        /// </summary>
        public int DataLogId { get; set; }
        /// <summary>
        /// Навигационное свойство ИмпортЖурналаДанных
        /// </summary>
        public virtual DataLog DataLog { get; set; }
        #endregion
    }
}
