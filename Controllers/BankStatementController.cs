using Microsoft.AspNetCore.Mvc;
using BankStatementApi.BLL.Services.Interfaces;
using BankStatementApi.BLL.DTO.SetDTO;
using BankStatementApi.BLL.DTO.GetDTO;
using BankStatementApi.BLL.Infrastructure;
using IbZKH_CustomTypes.GenericTypes;
using BankStatementApi.BLL.DTO.FilterDTO;
using NLog;
using Microsoft.AspNetCore.Authorization;

namespace BankStatementApi.Controllers
{
    /// <summary>
    /// Контроллер для выписок из банка
    /// </summary>   
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BankStatementController : Controller
    {
        private readonly IBankStatementServices _services;
        private readonly NLog.ILogger _logger;
        
        #region CTOR
        public BankStatementController(IBankStatementServices documentServices)
        {         
            _logger = LogManager.GetCurrentClassLogger(typeof(BankStatementController));   
            _services = documentServices;
        }
        #endregion

        /// <summary>
        /// Импорт файла банковской выписки
        /// </summary>
        /// <param name="file">Файл</param>      
        [HttpPost("ImportBankStatementFile")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        public IActionResult ImportBankStatementFile(IFormFile file)
        {
            try
            {
                if (file == null)
                {
                    return BadRequest();
                }
                using var fileStream = file.OpenReadStream();
                var fileData = new byte[file.Length];
                fileStream.Read(fileData, 0, (int)file.Length);

                if (!fileData.Any())
                {
                    return BadRequest("Загружаемый файл не может быть пустым.");
                }
                var result = _services.ImportBankStatementFile(
                    new FileGetDTO()
                    {
                        FileData = fileData,
                        FileName = file.FileName
                    });
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Получить журналы данных
        /// </summary>
        /// <param name="parametr"></param>  
        [HttpGet("GetDataLogsByImportResult")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ICollection<DataLogGetDTO>))]
        [Authorize]
        public IActionResult GetDataLogsByImportResult([FromQuery] DataLogsFilterDTO parametr)
        {
            try
            {
                var result = _services.GetDataLogsByImportResult(parametr);
                return Ok(
                    new
                    {
                        files = result
                        .Skip((parametr.PageNumber - 1) * parametr.PageSize)
                        .Take(parametr.PageSize),
                        totalCount = result.Count
                    });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <sammary>
        /// Получить журналы импорта в виде файла
        /// </summary>
        /// <param name="parametr"></param>  
        [HttpGet("GetImportLogFile")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileGetDTO))]
        [Authorize]
        public IActionResult GetImportLogFile([FromQuery] int dataLogId)
        {
            try
            {
                var result = _services.GetImportLogFile(dataLogId);
                if(result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        
        /// Получение платежных поручений по ТипуОперации
        /// </summary>
        /// <param name="parametr"></param>  
        [HttpGet("GetPaymentOrdersByOperationType")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ICollection<PaymentOrderGetDTO>))]
        [Authorize]
        public IActionResult GetPaymentOrdersByOperationType([FromQuery] PaymentOrderByOperationTypeFilterDTO parametr)
        {
            try
            {
                var result = _services.GetPaymentOrdersByOperationType(parametr);
                return Ok(
                    new
                    {
                        paymentOrders = result
                        .Skip((parametr.PageNumber - 1) * parametr.PageSize)
                        .Take(parametr.PageSize),
                        totalCount = result.Count
                    });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Получать платежные поручения по идентификатору ДокументаБанковскогоСчета
        /// </summary>
        /// <param name="parametr"></param>  
        [HttpGet("GetPaymentOrdersByBankAccountDocumentId")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ICollection<PaymentOrderGetDTO>))]
        [Authorize]
        public IActionResult GetPaymentOrdersByBankAccountDocumentId([FromQuery] PaymentOrdersByBankAccountDocumentIdFilterDTO parametr)
        {
            try
            {
                var result = _services.GetPaymentOrdersByBankAccountDocumentId(parametr);
                return Ok(
                    new
                    {
                        paymentOrders = result
                        .Skip((parametr.PageNumber - 1) * parametr.PageSize)
                        .Take(parametr.PageSize),
                        totalCount = result.Count
                    });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        /// <sammary>
        /// Получить имена файлов журнал регистрации данных
        /// </summary>
        /// <param name="parametr"></param>  
        [HttpGet("GetDataLogFileNameByMask")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<KeyValueItem<int>>))]
        [Authorize]
        public IActionResult GetDataLogFileNameByMask([FromQuery] string fileNameMask)
        {
            try
            {
                return Ok(_services.GetDataLogFileNameByMask(fileNameMask));
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Получать платежные поручения по НомеруБанковскогоСчета
        /// </summary>
        /// <param name="parametr"></param>  
        [HttpGet("GetPaymentOrdersByBankAccountNumber")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ICollection<PaymentOrderGetDTO>))]
        [Authorize]
        public IActionResult GetPaymentOrdersByBankAccountNumber([FromQuery] PaymentOrdersByFilterDTO parametr, int PageNumber, int PageSize)
        {
            try
            {
                var result = _services.GetPaymentOrdersByBankAccountNumber(parametr);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(
                    new
                    {
                        paymentOrders = result
                        .Skip((PageNumber - 1) * PageSize)
                        .Take(PageSize),
                        totalCount = result.Count
                    });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Получить журнал данных по идентификтору
        /// </summary>
        /// <param name="id">Идентификатор журнала данных</param>  
        [HttpGet("GetDataLog/{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DataLogGetDTO))]
        [Authorize]
        public IActionResult GetDataLog(int id)
        {
            try
            {
                var result = _services.GetDataLog(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Получить документы банковского счета по идентификатору журнала данных
        /// </summary>
        /// <param name="parametr"></param> 
        [HttpGet("GetBankAccountDocumentsByDataLogId")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<BankAccountDocumentGetDTO>))]
        [Authorize]
        public IActionResult GetBankAccountDocumentsByDataLogId([FromQuery] GetBankAccountDocumentsByDataLogIdFilterDTO parametr)
        {
            try
            {
                var result = _services.GetBankAccountDocumentsByDataLogId(parametr.DataLogId);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(new
                {
                    paymentOrders = result
                        .Skip((parametr.PageNumber - 1) * parametr.PageSize)
                        .Take(parametr.PageSize),
                    totalCount = result.Count
                });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Перезапустить обработку записи журнала данных
        /// </summary>
        /// <param name="id">Идентификтор записи журнала данных</param>  
        [HttpGet("RestartDataLogProcessing/{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        public IActionResult RestartDataLogProcessing(int id)
        {
            try
            {
                return Ok(_services.RestartDataLogProcessing(id));
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Удаление записи журнала данных
        /// </summary>
        /// <param name="id">Идентификатор записи журнала данных</param>  
        [HttpDelete("DeleteDataLog/{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        public IActionResult DeleteDataLog(int id)
        {
            try
            {
                return Ok(_services.DeleteDataLog(id));
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Привязать платежное поручение к типу операции
        /// </summary>
        /// <param name="parametr"></param>  
        [HttpPost("BindPaymentOrderToOperationType")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        public IActionResult BindPaymentOrderToOperationType([FromForm] PaymentOrdeToModuleSetDTO parametr)
        {
            try
            {
                var result = _services.BindPaymentOrderToOperationType(parametr);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Отвязать платежное поручение от типа операции
        /// </summary>
        /// <param name="ids">Коллекция идентификторов ПлатежныхПоручений</param>  
        [HttpPost("UnBindPaymentOrderToOperationType")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        public IActionResult UnBindPaymentOrderToOperationType([FromForm] List<int> ids)
        {
            try
            {
                var result = _services.UnBindPaymentOrderToOperationType(ids);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Получить отчет ПлатежныхПоручений за указанный период в диапазоне одного месяца
        /// </summary>
        /// <param name="parametr"></param>  
        [HttpGet("GetPaymentOrdersWithinDays")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileGetDTO))]
        [Authorize]
        public IActionResult GetPaymentOrdersWithinDays([FromQuery] PaymentOrdersByFilterDTO parametr)
        {
            try
            {
                if (parametr.DateEnd.Month != parametr.DateStart.Month || parametr.DateStart.Year != parametr.DateEnd.Year)
                {
                    return BadRequest("Диапазон даты не должен выходить за пределы одного месяца.");
                }
                return Ok(_services.GetPaymentOrdersWithinDays(parametr));
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Получить коллекцию ПлатежныхПоручений по переданному фильтру
        /// </summary>
        /// <param name="parametr"></param>  
        [HttpGet("GetPaymentOrdersByFilter")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileGetDTO))]
        [Authorize]
        public IActionResult GetPaymentOrdersByFilter([FromQuery] PaymentOrderByFeaturesFilterDTO filter)
        {
            try
            {
                if (filter == null || Verifier.CheckFilterForCorrect(filter))
                {
                    return BadRequest("Фильтр должен содержать хотя бы одно значение.");
                }

                var result = _services.GetPaymentOrdersByFilter(filter);
                if (result == null)
                    return NotFound();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        #region Ignore
        /// <summary>
        /// Получить документ о банковском счете
        /// </summary>
        /// <param name="id">Идентификатор БанкАккаунтДокумент</param>  
        [HttpGet("GetBankAccountDocument/{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BankAccountDocumentGetDTO))]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult GetBankAccountDocument(int id)
        {
            try
            {
                var result = _services.GetBankAccountDocument(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        #endregion      
    }
}
