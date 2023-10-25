using BankStatementApi.BLL.BusinessLogic.Event.Model;
using BankStatementApi.BLL.DTO.GetDTO;
using BankStatementApi.BLL.DTO.SetDTO;
using BankStatementApi.BLL.Infrastructure;
using BankStatementApi.DAL.Entities;
using BankStatementApi.DAL.Entities.Enum;
using IbZKH_CustomTypes.SingleTypes;
using IbZKH_Extensions.ExtendedTools.Zip;
using System.Text;

namespace BankStatementApi.BLL.BusinessLogiс.Import
{
    /// <summary>
    /// Бизнес-логика журнала импорта
    /// </summary> 
    public class ImportRunner
    {
        private readonly byte [] _fileData;
        private readonly string _fileName;
        private readonly IServiceInfrastructure _serviceInfrastructure;

        #region Ctor   
        public ImportRunner(IServiceInfrastructure serviceInfrastructure)
        {
            _serviceInfrastructure = serviceInfrastructure;
        }

        public ImportRunner( FileGetDTO importParametr, IServiceInfrastructure serviceInfrastructure)
        {
            _fileData = importParametr.FileData;
            _fileName = importParametr.FileName;
            _serviceInfrastructure = serviceInfrastructure;
        }
        #endregion

        /// <summary>
        /// Отправить документ в очередь
        /// </summary> 
        public OperationResult SendDocumentInQueue()
        {
            var previouslyCheck = PreviouslyCheck();
            if( previouslyCheck != null )
                return OperationResult.CreateErrorResult(previouslyCheck);

            var dataLogBl = new DataLogBl(_serviceInfrastructure);
            var importLog = dataLogBl.CreateLogSession(_fileData , _fileName , _serviceInfrastructure);
            _serviceInfrastructure.Producer.GetProducer<DataLogEvent>().Publish(
                new DataLogEvent(_serviceInfrastructure.IdentityProvider)
                {
                    DataLogId = importLog.Id
                });
            importLog.UpdateLog("Документ добавлен в очередь" , ImportResultEnum.InQueue);
            return OperationResult.CreateSuccessResult("Документ принят в обработку.");
        }

        /// <summary>
        /// Разбор данных
        /// </summary> 
        public OperationResult TryParce(DataLogSetDTO datalog)
        {
            var prov = CodePagesEncodingProvider.Instance;
            Encoding.RegisterProvider(prov);
            datalog.FileData = _serviceInfrastructure.Repository.GetRepository<DataStorage>().SingleOrDefault(x => x.DataLogId == datalog.Id).BinaryContent;
            var dataLog = _serviceInfrastructure.Repository.GetRepository<DataLog>().SingleOrDefault(x => x.Id == datalog.Id);
            var importLogBl = new DataLogBl(dataLog , _serviceInfrastructure);
            importLogBl.UpdateLog("Взят из очереди" , ImportResultEnum.FileInProcessing);
            var importer = new Importer(importLogBl , _serviceInfrastructure);
      
            if( DataLogBl.ExistLog(datalog.FileData, _serviceInfrastructure) )
            {
                importer.ClearParcingDataFromModelImpl();
                importer.DataLogBl.UpdateLog("Очистка данных" , ImportResultEnum.FileInProcessing);
            }
            var zipUtil = new ZipUtil(datalog.FileData, dataLog.FileName);
            if( zipUtil.IsZip() )
            {
                var fileNamesList = zipUtil.GetFileNamesFromZip();

                if( fileNamesList.Count > 1 )
                {
                    var badMessage = "Импорт не произведён, т.к. был получен архив, содержащий более одного файла!";
                    return OperationResult.CreateErrorResult(badMessage);
                }
                else
                {
                    datalog.FileData = zipUtil.ExtractFile(fileNamesList.First());
                    return importer.StartParcing(datalog);
                }
            }
            return importer.StartParcing(datalog);
        }

        /// <summary>
        /// Проврека наличия журнала
        /// </summary> 
        private string PreviouslyCheck()
        {
            return DataLogBl.ExistLog(_fileData , _serviceInfrastructure) ?
                     "Переданный файл уже был ранее импортирован" : null;
        }
    }
}
