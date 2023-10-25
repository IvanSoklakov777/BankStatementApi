using BankStatementApi.DAL.Interfaces;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BankStatementApi.DAL.Repositories
{
    /// <summary>
    /// Хранилище банковских выписок 
    /// </summary>
    public class BankStatementRepository<TEntity> : IBankStatementRepository<TEntity> where TEntity : class
    {

        private readonly IBankStatementUnitOfWork _unitOfWork;
        public BankStatementRepository( IBankStatementUnitOfWork unitOfWork )
        {
            _unitOfWork = unitOfWork;
        }
        /// <summary>
        /// Добавление сущности
        /// </summary>
        /// <param name="item">Объект</param>
        public void Add(TEntity item)
        {
            _unitOfWork.Context.Add(item);
            _unitOfWork.SaveChanges();
        }
        /// <summary>
        /// Добавить коллекцию  
        /// </summary>
        /// <param name="itemList">Колеекция объектов</param>
        public void AddRange(ICollection<TEntity> itemList)
        {
            _unitOfWork.Context.AddRange(itemList);
            _unitOfWork.SaveChanges();
        }
        /// <summary>
        /// Объемная вставка 
        /// </summary>
        /// <param name="itemList">Колеекция объектов</param>
        public void BulkInsert(ICollection<TEntity> itemList) 
        { 
            _unitOfWork.Context.BulkInsert(itemList.ToList());
            _unitOfWork.SaveChanges();
        }
        /// <summary>
        /// Получение сущности 
        /// </summary>
        /// <param name="predicate">Предикат</param>
        /// <returns>Один объект или пусто если нет объекта с таким ключом</returns>
        public TEntity SingleOrDefault( Func<TEntity , bool> predicate ) => _unitOfWork.Context.Set<TEntity>().SingleOrDefault(predicate);

        /// <summary>
        /// Проверка наличия сущности по условию 
        /// </summary>
        /// <param name="predicate">Предикат</param>
        /// <returns>Логическое значение</returns>
        public bool Any( Func<TEntity , bool> predicate ) => _unitOfWork.Context.Set<TEntity>().Any(predicate);

        /// <summary>
        /// Удаление сущности
        /// </summary>
        /// <param name="predicate">Предикат</param>
        public void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            var entities = Where(predicate).ToList();
            if (entities.Any())
            {
                _unitOfWork.Context.Set<TEntity>().RemoveRange(entities);
                _unitOfWork.SaveChanges();
            }          
        }

        /// <summary>
        /// Редактирование сущности
        /// </summary>
        /// <param name="item">Объект</param>
        public void Update( TEntity item )
        {
            _unitOfWork.Context.Set<TEntity>().Attach(item);
            _unitOfWork.Context.Entry(item).State = EntityState.Modified;
            _unitOfWork.SaveChanges();
        }

        /// <summary>
        /// Удаление сущность
        /// </summary>
        /// <param name="item">Объект</param>
        public void Delete( TEntity item )
        {
            if( _unitOfWork.Context.Entry(item).State == EntityState.Detached )
            {
                _unitOfWork.Context.Attach(item);
            }
            _unitOfWork.Context.Remove(item);
            _unitOfWork.SaveChanges();
        }

        /// <summary>
        /// Получение всех сущностей
        /// </summary>
        /// <param name="item">Объект</param>
        public IEnumerable<TEntity> Where(Expression<Func<TEntity, bool>> predicate = null)
        {
            if (predicate != null)
            {
                return _unitOfWork.Context.Set<TEntity>().Where(predicate).ToList();
            }
            return _unitOfWork.Context.Set<TEntity>().ToList();
        }

        /// <summary>
        /// Получите сущность c помощью Include
        /// </summary>
        /// <param name="item">Объект</param>
        public TEntity SingleOrDefaultWithInclude(Func<TEntity, bool> predicate, Expression<Func<TEntity, object>> includeProperties) => Include(includeProperties).SingleOrDefault(predicate);

        /// <summary>
        /// Получите список сущностей c помощью Include
        /// </summary>
        /// <param name="item">Объект</param>
        public IEnumerable<TEntity> WhereWithInclude(params Expression<Func<TEntity, object>>[] includeProperties) => Include(includeProperties).ToList();       

        /// <summary>
        /// Получите список сущностей c помощью Include
        /// </summary>
        /// <param name="item">Объект</param>
        public IEnumerable<TEntity> WhereWithInclude(Func<TEntity, bool> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = Include(includeProperties);
            return query.Where(predicate).ToList();
        }

        #region Private
        /// <summary>
        /// Получите c помощью Include
        /// </summary>
        /// <param name="item">Объект</param>
        private IQueryable<TEntity> Include(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = _unitOfWork.Context.Set<TEntity>().AsNoTracking();
            return includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }
        #endregion 
    }
}
