using BankStatementApi.BLL.Infrastructure;
using BankStatementApi.DAL.EF;
using BankStatementApi.DAL.Entities.Interfaces;
using BankStatementApi.DAL.Interfaces;

namespace BankStatementApi.DAL.Repositories
{
    /// <summary>
    /// Паттерн Unit of Work позволяет упростить работу с различными репозиториями и дает уверенность, что все репозитории будут использовать один и тот же контекст данных.
    /// </summary>  
    public class BankStatementUnitOfWork : IBankStatementUnitOfWork
    {
        public BankStatementContext Context { get; }
        public DictionaryStorage DictionaryStorage { get; }
        private bool _disposed;
        private Dictionary<Type , object> _repositories;

        /// <summary>
        /// Констуктор
        /// </summary>
        /// <param name="context">Контекст БД</param>
        /// <param name="operationTypeRepository">Хранилище типов операций</param>
        /// <param name="importResultRepository">Хранилище результатов импорта</param>
        /// <param name="transferTypeRepository">Хранилище типа передачи</param>
        public BankStatementUnitOfWork( BankStatementContext context, DictionaryStorage dictionaryStorage)
        {
            DictionaryStorage = dictionaryStorage;
            Context = context;   
            _disposed = false;        
        }
        /// <summary>
        /// Получить Репозиторий
        /// </summary>
        /// <returns>Один объект или пусто если нет объекта с таким ключом</returns>
        public IBankStatementRepository<TEntity> GetRepository<TEntity>() 
            where TEntity : class
        {
            if( _repositories == null )
                _repositories = new Dictionary<Type , object>();
            var type = typeof(TEntity);
            if( !_repositories.ContainsKey(type) )
                _repositories [ type ] = new BankStatementRepository<TEntity>(this);
            return ( IBankStatementRepository<TEntity> ) _repositories [ type ];
        }

        /// <summary>
        /// Получить Репозиторий для словарей
        /// </summary>
        /// <returns>Один объект или пусто если нет объекта с таким ключом</returns>
        public IBankStatementDictionaryRepository<TEntity, TId> GetDictionaryRepository<TEntity, TId>()
            where TId : struct
            where TEntity : class, ITable<TId>
        {
            if (_repositories == null)
                _repositories = new Dictionary<Type, object>();
            var type = typeof(TEntity);
            if (!_repositories.ContainsKey(type))
                _repositories[type] = new DictionaryRepository<TEntity, TId>(this);
            return (IBankStatementDictionaryRepository<TEntity, TId>)_repositories[type];
        }

        /// <summary>
        /// Не вызывать
        ///
        /// Сводка:
        /// Releases the allocated resources for this context.
        ///
        /// Примечания:
        /// See DbContext lifetime, configuration, and initialization for more information.
        /// </summary>
        public virtual void Dispose( bool disposing )
        {
            if( !this._disposed )
            {
                if( disposing )
                {
                    Context.Dispose();
                }
                this._disposed = true;
            }
        }
        /// <summary>
        /// Не вызывать
        ///
        /// Сводка:
        /// Releases the allocated resources for this context.
        ///
        /// Примечания:
        /// See DbContext lifetime, configuration, and initialization for more information.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Сохрение изменений
        /// </summary>
        /// <returns>Число записей о состоянии, записанных в базу данных.</returns>
        public int SaveChanges()
        {
            return Context.SaveChanges();
        }
    }
}
