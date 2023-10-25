using BankStatementApi.BLL.DTO.SetDTO;
using BankStatementApi.DAL.Entities.Enum;
using IbZKH_CustomTypes.GenericTypes;

namespace BankStatementApi.BLL.Services.Interfaces
{
    /// <summary>
    /// Интерфейс для сервиса взаимодействия с словарями
    /// </summary>
    public interface IDictionaryServices
    {
        /// <summary>
        /// Получение всех значений словаря РезультатИмпорта
        /// </summary>
        List<KeyValueItem<int>> GetImportResultDictionary();
        /// <summary>
        /// Получение всех значений словаря ТипПередачи
        /// </summary>
        List<KeyValueItem<int>> GetTransferTypesDictionary();
        /// <summary>
        /// Получение всех значений словаря ТипОперации
        /// </summary>
        List<KeyValueItem<Guid>> GetOperationTypesDictionary();
        /// <summary>
        /// Добавить записи в словарь типов операций
        /// </summary>
        void AddOperationTypesDictioanryItem(ICollection<DictionaryOperationTypeSetDTO> dictionaryList);
        /// <summary>
        /// Получение всех значений словаря по типу словаря
        /// </summary>
        List<KeyValueItem<object>> GetShortList(TypeDictionary refType);
    }
}
