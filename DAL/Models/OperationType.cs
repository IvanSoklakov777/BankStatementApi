using BankStatementApi.DAL.Entities.Enum;
using BankStatementApi.DAL.Entities.Interfaces;

namespace BankStatementApi.DAL.Entities
{
    /// <summary>
    /// Словарь типы операций
    /// </summary>
    [DictionaryType(TypeDictionary.OperationTypeEnum)]
    public class OperationType : ITable<Guid>
    {
        /// <summary>
        /// Идентификтор 
        /// </summary>
        public Guid Id { get; set; }
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
