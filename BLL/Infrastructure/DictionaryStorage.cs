using BankStatementApi.DAL.EF;
using BankStatementApi.DAL.Entities.Interfaces;
using DictionaryManagment.Dictionary;

namespace BankStatementApi.BLL.Infrastructure
{
    /// <summary>
    /// Хранилище словарей
    /// </summary>
    public class DictionaryStorage
    {
        private readonly IServiceProvider _scopeFactory;
        private Dictionary<Type, object> _repositories;

        public DictionaryStorage( IServiceProvider scopeFactory )
        {
            _scopeFactory = scopeFactory;
        }

        public SafeDictionary<TId,TEntity> GetDictionaryStorage<TId, TEntity>() 
            where TEntity : class, ITable<TId>
            where TId : struct
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<BankStatementContext>();

            if (_repositories == null)
                _repositories = new Dictionary<Type, object>();
            var type = typeof(TEntity);
            if (!_repositories.ContainsKey(type))
                _repositories[type] = new SafeDictionary<TId, TEntity>(context.Set<TEntity>().ToDictionary(m => m.Id)); ;
            return (SafeDictionary<TId, TEntity>)_repositories[type];
        }
    }
}
