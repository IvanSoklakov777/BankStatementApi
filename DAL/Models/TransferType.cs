using BankStatementApi.DAL.Entities.Enum;
using BankStatementApi.DAL.Entities.Interfaces;

namespace BankStatementApi.DAL.Entities
{
    /// <summary>
    /// Тип Передачи 
    /// </summary>
    [DictionaryType(TypeDictionary.TransferTypeEnum)]
    public class TransferType : ITable<int>
    {
        /// <summary>
        /// Идентификтор 
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Наименование
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Дата изменения 
        /// </summary>
        public DateTime ChangeDate { get; set; }
        /// <summary>
        /// Идентификтор работника 
        /// </summary>
        public int? WorkerChangedById { get; set; }
    }    
}
