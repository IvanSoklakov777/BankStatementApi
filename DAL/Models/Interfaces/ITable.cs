using DictionaryManagment.Model;

namespace BankStatementApi.DAL.Entities.Interfaces
{
    public interface ITable<T> : IBaseEntityFields<T>, IDictionaryModel<T>
    {
    }
}
