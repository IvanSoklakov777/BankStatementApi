using IbZKH_CustomTypes.GenericTypes;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using BankStatementApi.BLL.Services.Interfaces;
using BankStatementApi.DAL.Entities.Enum;
using BankStatementApi.BLL.DTO.SetDTO;
using NLog;
using Microsoft.AspNetCore.Authorization;

namespace BankStatementApi.Controllers
{
    /// <summary>
    /// Контроллер для словарей
    /// </summary>    
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DictionaryController : ControllerBase
    {
        private readonly IDictionaryServices _services;
        private readonly NLog.ILogger _logger;       

        #region CTOR
        public DictionaryController( IDictionaryServices services)
        {
            _logger = LogManager.GetCurrentClassLogger(typeof(DictionaryController));
            _services = services;
        }
        #endregion

        /// <summary>
        /// Получение всех значений словаря РезультатИмпорта
        /// </summary>
        /// <returns>Коллекция словарей</returns>
        [HttpGet("GetImportResults")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]      
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status200OK , Type = typeof(List<KeyValueItem<string>>))]
        [Authorize]
        public IActionResult GetImportResults()
        {
            try
            {
                var result = _services.GetImportResultDictionary();
                return result == null ? NotFound() : Ok(result);
            }
            catch( ValidationException ex )
            {
                _logger.Error(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Получение всех значений словаря ТипПередачи
        /// </summary>
        /// <returns>Коллекция словарей</returns>
        [HttpGet("GetTransferTypes")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status200OK , Type = typeof(List<KeyValueItem<string>>))]
        [Authorize]
        public IActionResult GetTransferTypes()
        {
            try
            {
                var result = _services.GetTransferTypesDictionary();
                return result == null ? NotFound() : Ok(result);
            }
            catch( ValidationException ex )
            {
                _logger.Error(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Получение всех значений словаря ТипОперации
        /// </summary>
        /// <returns>Коллекция словарей</returns>
        [HttpGet("GetOperationTypes")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]    
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status200OK , Type = typeof(List<KeyValueItem<string>>))]
        [Authorize]
        public IActionResult GetOperationTypes()
        {
            try
            {
                var result = _services.GetOperationTypesDictionary();
                return result==null?NotFound():Ok(result);
            }
            catch( Exception ex )
            {
                _logger.Error(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Добавить записи в словарь типов операций
        /// </summary>
        /// <param name="dictionaryList">Коллекция записей для словаря</param>  
        [HttpPost("AddOperationTypesDictioanryItem")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        public IActionResult AddOperationTypesDictioanryItem([FromBody] ICollection<DictionaryOperationTypeSetDTO> dictionaryList)
        {
            try
            {
                if (!dictionaryList.Any())
                {
                    return BadRequest();
                }
                _services.AddOperationTypesDictioanryItem(dictionaryList);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        #region Ignore
        [HttpPost("GetShortList")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ApiExplorerSettings(IgnoreApi = true)]
        [Authorize]
        public IActionResult GetShortList([FromQuery] TypeDictionary refType)
        {
            try
            {
                if (refType == default)
                {
                    return BadRequest();
                }
                return Ok(_services.GetShortList(refType));
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
