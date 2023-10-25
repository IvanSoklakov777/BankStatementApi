using System.Linq.Expressions;

namespace BankStatementApi.DAL.Interfaces
{
    /// <summary>
    /// Интерфейчс базовых функций репозитория
    /// </summary>
    public interface IRepositoryBase<TEntity>
    {
        /// <summary>
        /// Получение всех объектов с параметрами
        /// </summary>
        /// <param name="predicate">Параметры выборки</param>
        /// <returns>Список объектов</returns>
        IEnumerable<TEntity> Where( Expression<Func<TEntity , bool>> predicate = null );
        /// <summary>
        /// Получение одного объекта по ключу
        /// </summary>
        /// <param name="predicate">Предикат</param>
        /// <returns>Один объект или пусто если нет объекта с таким ключом</returns>
        TEntity SingleOrDefault( Func<TEntity , bool> predicate );
        /// <summary>
        /// Добавление объекта
        /// </summary>
        /// <param name="item">Объект</param>
        void Add( TEntity item );
        /// <summary>
        /// Удаление объекта
        /// </summary>
        /// <param name="predicate">Предикат</param>
        void Delete( Expression<Func<TEntity , bool>> predicate );
        /// <summary>
        /// Удаление объекта
        /// </summary>
        /// <param name="item">Объект</param>
        void Delete( TEntity item );
        /// <summary>
        /// Редактирование объекта
        /// </summary>
        /// <param name="item">Объект</param>
        void Update( TEntity item );
    }
    /// <summary>
    /// Интерефейс хранилища БанковскихВыписок 
    /// </summary>
    public interface IBankStatementRepository<TEntity> : IRepositoryBase<TEntity> where TEntity : class
    {
        /// <summary>
        /// Тип Передачи 
        /// </summary>
        /// <param name="predicate">Предикат</param>
        /// <returns>Логическое значение</returns>
        bool Any( Func<TEntity , bool> predicate);
        /// <summary>
        /// Объемная вставка 
        /// </summary>
        /// <param name="itemList">Колеекция объектов</param>
        void BulkInsert( ICollection<TEntity> itemList );
        /// <summary>
        /// Добавить коллекцию  
        /// </summary>
        /// <param name="itemList">Колеекция объектов</param>
        void AddRange( ICollection<TEntity> itemList );
        /// <summary>
        ///  Получить список c загрузкой связанных данных  
        /// </summary>
        IEnumerable<TEntity> WhereWithInclude( params Expression<Func<TEntity, object>>[] includeProperties);
        /// <summary>
        /// Получить список c загрузкой связанных данных 
        /// </summary>        
        IEnumerable<TEntity> WhereWithInclude( Func<TEntity, bool> predicate, params Expression<Func<TEntity, object>>[] includeProperties);
        /// <summary>
        /// Получить сущность c загрузкой связанных данных 
        /// </summary>   
        TEntity SingleOrDefaultWithInclude(Func<TEntity, bool> predicate, Expression<Func<TEntity, object>> includeProperties);
    }
}
