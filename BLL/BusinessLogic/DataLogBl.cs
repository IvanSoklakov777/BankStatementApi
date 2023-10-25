using BankStatementApi.BLL.BusinessLogiс.Import;
using BankStatementApi.BLL.BusinessLogiс.Tools.ImportData.DataParcer;
using BankStatementApi.BLL.DTO.FilterDTO;
using BankStatementApi.BLL.DTO.GetDTO;
using BankStatementApi.BLL.DTO.SetDTO;
using BankStatementApi.BLL.Infrastructure;
using BankStatementApi.DAL.Entities;
using BankStatementApi.DAL.Entities.Enum;
using IbZKH_CustomTypes.GenericTypes;
using IbZKH_CustomTypes.SingleTypes;
using IbZKH_Extensions.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace BankStatementApi.BLL.BusinessLogiс
{
    /// <summary>
    /// Бизнес-логика журнала импорта
    /// </summary> 
    public class DataLogBl
    {
        public int Id => ModelWrapper.ModelInstance?.Id ?? 0;
        public DataLog DataLog { get; set; }
        public ObjectModelWrapper ModelWrapper { get; }
        private readonly IServiceInfrastructure _serviceInfrastructure;

        #region CTOR
        public DataLogBl(IServiceInfrastructure serviceInfrastructure)
        {
            _serviceInfrastructure = serviceInfrastructure;
        }
        public DataLogBl( DataLog importLog , IServiceInfrastructure serviceInfrastructure)
        {
            ModelWrapper = new ObjectModelWrapper(importLog.Id , _serviceInfrastructure);
            DataLog = importLog;
            ModelWrapper.SetInstance(DataLog);
            _serviceInfrastructure = serviceInfrastructure;
        }
        public DataLogBl( int idImportLog , IServiceInfrastructure serviceInfrastructure)
        {         
            ModelWrapper = new ObjectModelWrapper(idImportLog , _serviceInfrastructure);
            DataLog = serviceInfrastructure.Repository.GetRepository<DataLog>().SingleOrDefault(x => x.Id == idImportLog);
            ModelWrapper.SetInstance(DataLog);
            _serviceInfrastructure = serviceInfrastructure;
        }
        #endregion
        /// <summary>
        /// Создать сеанс ведения журнала
        /// </summary> 
        public DataLogBl CreateLogSession( byte[] fileData , string fileName , IServiceInfrastructure serviceInfrastructure)
        {
            var importLog = FindOrCreateLog(fileData , fileName , serviceInfrastructure);
            return new DataLogBl(importLog , serviceInfrastructure);
        }

        /// <summary>
        /// Поиск или создание журнала
        /// </summary> 
        private DataLog FindOrCreateLog( byte[] fileData , string fileName, IServiceInfrastructure serviceInfrastructure)
        {
            var importerLog = serviceInfrastructure.Repository.GetRepository<DataLog>().SingleOrDefault(x => x.FileHash == GetFileHashCode(fileData));          
            if( importerLog == null )
            {
                importerLog = new DataLog()
                {
                    FileHash = fileData.GetFileHashCode() ,
                    DataStorage = new DataStorage()
                    {
                        BinaryContent = fileData,
                        ChangeDate = DateTime.UtcNow,
                        WorkerChangedById = _serviceInfrastructure.IdentityProvider.WorkerId
                    }
                };
                serviceInfrastructure.Repository.GetRepository<DataLog>().Add(importerLog);
            }
            importerLog.ImportLog = $"{Environment.NewLine}{DateTime.UtcNow}{Environment.NewLine}{"Инициализация"}";
            importerLog.DateStartLoad = DateTime.UtcNow;
            importerLog.FileName = fileName;
            importerLog.FileSize = fileData.Length;
            importerLog.ChangeDate = DateTime.UtcNow;
            importerLog.WorkerChangedById = _serviceInfrastructure.IdentityProvider.WorkerId;
            return importerLog;
        }

        /// <summary>
        /// Перезапуск обработки
        /// </summary> 
        public OperationResult RestartProcessing()
        {     
            if (DataLog == null)
            {
                return OperationResult.CreateErrorResult($"Документ с идентификатором {Id} отсутствует в базе данных.");
            }
            if (DataLog.ImportResultId == ImportResultEnum.ProcessedSuccessfully)
            {
                return OperationResult.CreateErrorResult($"Cтатус файла <Обработан успешно>, перезапуск обработки запрещен.");
            }
            var importDataStorage = _serviceInfrastructure.Repository.GetRepository<DataStorage>().SingleOrDefault(x => x.DataLogId == ModelWrapper.ModelInstance.Id);
            var importer = new Importer(this , _serviceInfrastructure);
            UpdateLog("Перезапуск парсинга данных" , ImportResultEnum.FileInProcessing);
            UpdateLog("Очистка данных" , ImportResultEnum.FileInProcessing);
            importer.ClearParcingDataFromModelImpl();
            var datalog = new DataLogSetDTO
            {
                FileData = importDataStorage.BinaryContent,
                WorkerId = _serviceInfrastructure.IdentityProvider.WorkerId,
                LegalEntityId = _serviceInfrastructure.IdentityProvider.LegalEntityId
            };
            return importer.StartParcing(datalog);
        }

        /// <summary>
        /// Удаление журнала данных 
        /// </summary> 
        public OperationResult DeleteDataLog()
        {
            if (DataLog == null)
            {
                return OperationResult.CreateErrorResult($"Файл с идентификатором {Id} отсутствует в базе данных.");
            }
            if (DataLog.ImportResultId == ImportResultEnum.ProcessedSuccessfully)
            {
                return OperationResult.CreateErrorResult("Cтатус файла обработан успешно, удаление запрещено.");
            }
            _serviceInfrastructure.Repository.GetRepository<DataLog>().Delete(DataLog);
            return OperationResult.CreateSuccessResult("Файл удален.");
        }

        /// <summary>
        /// Получить файл журнала импорта 
        /// </summary> 
        public FileGetDTO GetImportLogFile()
        {
            return DataLog != null ?
                new FileGetDTO
                {
                    FileName = $"ImportLog_{Id}_{DataLog?.ChangeDate.ToShortDateString()}.txt",
                    FileData = Encoding.UTF8.GetBytes(DataLog.ImportLog)
                } :
                null;
        }

        /// <sammary>
        /// Получить имена файлов журнал регистрации данных по маске
        /// <sammary>
        public List<KeyValueItem<int>> GetDataLogFileNameByMask(string fileNameMask)
        {          
            return _serviceInfrastructure.Repository.GetRepository<DataLog>()
                .Where(p => EF.Functions.Like(p.FileName, $"%{fileNameMask}%")).Select(x => new KeyValueItem<int> { Id = x.Id, Name = x.FileName }).ToList();
        }

        /// <summary>
        /// Получить журналы данных
        /// </summary>
        public IEnumerable<DataLog> GetDataLogsByImportResult(DataLogsFilterDTO parametr)
        {
            return _serviceInfrastructure.Repository.GetRepository<DataLog>()
                 .Where(d => (parametr.ImportResult == null || d.ImportResultId == parametr.ImportResult) && (d.DateStartLoad.Date >= parametr.DateStart.Date && d.DateStartLoad.Date <= parametr.DateEnd.Date));          
        }

        #region Log & State
        /// <summary>
        /// Обновлений журнала
        /// </summary> 
        public void UpdateLog( string message )
        {
            var log = $"{Environment.NewLine}{DateTime.UtcNow}{Environment.NewLine}{message}";
            var dataStorage = _serviceInfrastructure.Repository.GetRepository<DataLog>().SingleOrDefault(x => x.Id == ModelWrapper.ModelInstance.Id);
            dataStorage.ImportLog += log;
            _serviceInfrastructure.Repository.GetRepository<DataLog>().Update(dataStorage);
        }
        /// <summary>
        /// Обновлений журнала
        /// </summary> 
        public void UpdateLog( string message, ParcerDocument parcerDocument)
        {
            var log = $"{Environment.NewLine}{DateTime.UtcNow}{Environment.NewLine}{message}";
            var dataStorage = _serviceInfrastructure.Repository.GetRepository<DataLog>().SingleOrDefault(x => x.Id == ModelWrapper.ModelInstance.Id);
            dataStorage.DateFileStart = parcerDocument.DateStart;
            dataStorage.DateFileEnd = parcerDocument.DateEnd;
            _serviceInfrastructure.Repository.GetRepository<DataLog>().Update(dataStorage);
        }
        /// <summary>
        /// Обновлений журнала
        /// </summary> 
        public void UpdateLog( string message , ImportResultEnum importResult )
        {
            var log = $"{Environment.NewLine}{DateTime.UtcNow}{Environment.NewLine}{message}";
            var dataStorage = _serviceInfrastructure.Repository.GetRepository<DataLog>().SingleOrDefault(x => x.Id == ModelWrapper.ModelInstance.Id);
            dataStorage.ImportLog += log;
            dataStorage.ImportResultId = importResult;
            dataStorage.DateEndLoad = DateTime.UtcNow;
            _serviceInfrastructure.Repository.GetRepository<DataLog>().Update(dataStorage);
        }
        
        /// <summary>
        /// Проверка на существование журнала
        /// </summary> 
        public static bool ExistLog( byte [] fileData , IServiceInfrastructure serviceInfrastructure)
        {
            return ExistLog(GetFileHashCode(fileData) , fileData.Length , serviceInfrastructure);
        }
        /// <summary>
        /// Проверка на существование журнала
        /// </summary> 
        public static bool ExistLog( string fileHash , int fileSize , IServiceInfrastructure serviceInfrastructure)
        {
            return serviceInfrastructure.Repository.GetRepository<DataLog>().Any(x => x.FileHash == fileHash && x.FileSize == fileSize);
        }
        /// <summary>
        /// Получить Хэш-Код файла
        /// </summary> 
        public static string GetFileHashCode(byte[] fileData)
        {
            return fileData.GetFileHashCode();
        }
        #endregion
    }
}
