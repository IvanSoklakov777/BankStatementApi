using BankStatementApi.BLL.DTO.SetDTO;
using BankStatementApi.BLL.Infrastructure;
using BankStatementApi.BLL.Services.Interfaces;
using BankStatementApi.DAL.Entities;
using BankStatementApi.DAL.Entities.Enum;
using DictionaryManagment.Model;
using IbZKH_CustomTypes.GenericTypes;
using System.Reflection;

namespace BankStatementApi.BLL.Services
{
    /// <summary>
    /// Сервис взаимодействия с словарями
    /// </summary> 
    public class DictionaryServices : IDictionaryServices
    {
        private readonly IServiceInfrastructure _serviceInfrastructure;
        #region CTOR
        public DictionaryServices(IServiceInfrastructure serviceInfrastructure)
        {
            _serviceInfrastructure = serviceInfrastructure;
        }
        #endregion

        /// <summary>
        /// Получение словаря РезультатовИмпорта
        /// </summary> 
        public List<KeyValueItem<int>> GetImportResultDictionary()
        {
            var dictionary = _serviceInfrastructure.Repository.GetDictionaryRepository<ImportResult,int>().GetList();
            return _serviceInfrastructure.Mapper.Map<List<KeyValueItem<int>>>(source: dictionary);
        }
        /// <summary>
        /// Получение словаря ТиповОпераций
        /// </summary>
        public List<KeyValueItem<Guid>> GetOperationTypesDictionary()
        {
            var dictionary = _serviceInfrastructure.Repository.GetDictionaryRepository<OperationType,Guid>().GetList();
            return _serviceInfrastructure.Mapper.Map<List<KeyValueItem<Guid>>>(source: dictionary);
        }
        /// <summary>
        /// Получение словаря  ТиповПередачи
        /// </summary>
        public List<KeyValueItem<int>> GetTransferTypesDictionary()
        {
            var dictionary = _serviceInfrastructure.Repository.GetDictionaryRepository<TransferType,int>().GetList();
            return _serviceInfrastructure.Mapper.Map<List<KeyValueItem<int>>>(source: dictionary);
        }
        /// <summary>
        /// Добавить записи в словарь типов операций
        /// </summary>
        public void AddOperationTypesDictioanryItem(ICollection<DictionaryOperationTypeSetDTO> dictionaries)
        {
            foreach (var dictionary in dictionaries)
            {
                if (!_serviceInfrastructure.Repository.GetDictionaryRepository<OperationType, Guid>().Exist(dictionary.Id))
                {
                    var model = _serviceInfrastructure.Mapper.Map<OperationType>(source: dictionary);
                    model.ChangeDate = DateTime.UtcNow;
                    model.WorkerChangedById = _serviceInfrastructure.IdentityProvider.WorkerId;
                    _serviceInfrastructure.Repository.GetDictionaryRepository<OperationType, Guid>().Create(model);
                }
            }
        }

        /// <summary>
        /// Получение всех значений словаря по типу словаря
        /// </summary>
        public List<KeyValueItem<object>> GetShortList(TypeDictionary refType)
        {
            try
            {
                var dictionaryList = new Dictionary<TypeDictionary, List<KeyValueItem<object>>>(); 
                
                dictionaryList.Add(refType, GetShortListImpl(refType));
                return dictionaryList[refType];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Private
        /// <summary>
        /// Нужно доработать (метод работает, реализация неоднозначная)
        /// </summary>
        private List<KeyValueItem<object>> GetShortListImpl(TypeDictionary refType)
        {             
                var modelType = Assembly.GetAssembly(refType.GetType())
                               .GetTypes().ToList()
                               .SingleOrDefault(p => p.GetCustomAttribute<DictionaryTypeAttribute>() != null && p.GetCustomAttribute<DictionaryTypeAttribute>().Type == refType);
                var typeId = modelType.GetProperty("Id").PropertyType;

                var repositoryDictionary = _serviceInfrastructure.Repository.GetDictionaryRepository<OperationType, Guid>();

                if (modelType == null)
                    throw new Exception("Переданный тип словаря не определен. ");
                var method = repositoryDictionary.GetType().GetMethod("Where", BindingFlags.Public | BindingFlags.Instance)?.MakeGenericMethod(modelType, typeId);
                var dict = method.Invoke(repositoryDictionary, new object[] { });
               
                if (typeId == typeof(int))
                    return Convert<int>(dict);
                if (typeId == typeof(Guid))
                    return Convert<Guid>(dict);
                return null;                       
        }

        private static List<KeyValueItem<object>> Convert<T>(object dict) 
        {
            return ((IEnumerable<IDictionaryModel<T>>)dict).Select(p => new KeyValueItem<object> { Id = p.Id, Name = p.Title }).ToList();
        }
        #endregion
    }
}
