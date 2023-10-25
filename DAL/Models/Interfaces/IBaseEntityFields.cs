namespace BankStatementApi.DAL.Entities.Interfaces
{
    /// <summary>
    /// Интерфейс полей базовых сущностей
    /// </summary>
    public interface IBaseEntityFields<TId> 
    {   
        /// <summary>
        /// Идентификатор работника
        /// </summary>
        public int? WorkerChangedById { get; set; }
        /// <summary>
        /// Дата изменения 
        /// </summary>
        public DateTime ChangeDate { get; set;}
    }
}
