using BankStatementApi.DAL.Entities.Enum;
using BankStatementApi.DAL.Entities.Interfaces;

namespace BankStatementApi.DAL.Entities
{
    /// <summary>
    /// Словарь результат импорта
    /// </summary>
    [DictionaryType(TypeDictionary.ImportResultEnum)]
    public class ImportResult : ITable<int>
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
        /// Идентификтор работника 
        /// </summary>
        public int? WorkerChangedById { get; set; }
        /// <summary>
        /// Дата изменения
        /// </summary>
        public DateTime ChangeDate { get; set; }
    }
}
