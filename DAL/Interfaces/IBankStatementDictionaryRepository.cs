using BankStatementApi.DAL.Entities.Interfaces;
using DictionaryManagment.Repository;

namespace BankStatementApi.DAL.Interfaces
{
    public interface IBankStatementDictionaryRepository<TEntity, TId> : IDictionaryRepository<TEntity, TId>, IDictionaryManagmentReposytory<TEntity, TId>
        where TId : struct
        where TEntity : class, ITable<TId>
    {
        bool Exist(TId id);
        void Update(TEntity itemParam);
        IEnumerable<TEntity> Where<TEntity, TId>()
            where TEntity : class, ITable<TId>
            where TId : struct;
    }
}
