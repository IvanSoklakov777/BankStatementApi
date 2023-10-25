using BankStatementApi.BLL.Infrastructure;
using BankStatementApi.DAL.EF;
using BankStatementApi.DAL.Entities.Interfaces;
using BankStatementApi.DAL.Repositories;

namespace BankStatementApi.DAL.Interfaces
{
    /// <summary>
    /// Интерфейс для реализации патерна UnitOfWork
    /// </summary>
    public interface IBankStatementUnitOfWork : IDisposable
    {
        BankStatementContext Context { get; }
        DictionaryStorage DictionaryStorage { get; }
        /// <summary>
        /// Получить Репозиторий
        /// </summary>
        /// <returns>Один объект или пусто если нет объекта с таким ключом</returns>
        IBankStatementRepository<TEntity> GetRepository<TEntity>()
            where TEntity : class;

        /// <summary>
        /// Получить Репозиторий для словарей
        /// </summary>
        /// <returns>Один объект или пусто если нет объекта с таким ключом</returns>
        IBankStatementDictionaryRepository<TEntity,TId> GetDictionaryRepository<TEntity, TId>() 
            where TId : struct 
            where TEntity : class, ITable<TId>;
        /// <summary>
        /// Сохрение изменений
        /// </summary>
        /// <returns>Число записей о состоянии, записанных в базу данных.</returns>
        int SaveChanges();
    }
}
