namespace BankStatementApi.DAL.Entities.Interfaces
{   
    /// <summary>
    /// Интерфейс поддерживания удаления 
    /// </summary>
    public interface IIsDeletedSupport
    {
        /// <summary>
        /// Флаг удаления
        /// </summary>
        bool IsDeleted { get; set; }
    }
}
