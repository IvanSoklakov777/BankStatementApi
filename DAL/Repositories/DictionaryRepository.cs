using BankStatementApi.DAL.EF;
using BankStatementApi.DAL.Entities.Interfaces;
using BankStatementApi.DAL.Interfaces;
using DictionaryManagment.Repository;

namespace BankStatementApi.DAL.Repositories
{
    public class DictionaryRepository<TEntity, TId> : DictionaryRepositoryAndMemory<BankStatementContext, TEntity, TId>, IBankStatementDictionaryRepository<TEntity, TId>
        where TId : struct
        where TEntity : class, ITable<TId>
    {
        /// <summary>
        /// Констуктор
        /// </summary>
        /// <param name="context">Контекст БД</param>
        /// <param name="dictionaryStorage">Хранилище словарей</param>
        public DictionaryRepository(IBankStatementUnitOfWork unitOfWork) : base(unitOfWork.Context, unitOfWork.DictionaryStorage.GetDictionaryStorage<TId, TEntity>())
        {
        }

        /// <summary>
        /// Проверка существования 
        /// </summary>
        /// <param name="id">Идентификатор ТипаОперации</param>
        /// <returns>Логическое значение</returns>
        public bool Exist(TId id)
        {
            var result = dictionary.Any(cp => cp.Value.Id.Equals(id));
            if (!result)
            {
                result = context.Set<TEntity>().Any(cp => cp.Id.Equals(id));
            }
            return result;
        }
        /// <summary>
        /// Редактирование объекта
        /// </summary>
        /// <param name="itemParam">Объект</param>
        public override void Update(TEntity itemParam)
        {
            if (itemParam != null)
            {
                var item = Get(itemParam.Id);
                item.Title = itemParam.Title;
                item.ChangeDate = DateTime.Now;
                item.WorkerChangedById = itemParam.WorkerChangedById;
                base.Update(item);
                Save();
            }
        }

        public virtual IEnumerable<TEntity> Where<TEntity, TId>()
            where TEntity : class, ITable<TId>
            where TId : struct
        {
            return context.Set<TEntity>().Where(x => x != null).AsEnumerable();
        }
    }
}
