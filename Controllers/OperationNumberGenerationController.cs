using BankStatementApi.BLL.DTO.SetDTO;
using BankStatementApi.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog;

namespace BankStatementApi.Controllers
{
    /// <summary>
    /// Контроллер для генерации номера операции
    /// </summary>  
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OperationNumberGenerationController : Controller
    {
        private readonly IOperationNumberGenerationServices _operationNumberGenerationServices;
        private readonly NLog.ILogger _logger;

        #region CTOR
        public OperationNumberGenerationController(IOperationNumberGenerationServices operationNumberGenerationServices, ILoggerFactory loggerFactory)
        {
            _logger = LogManager.GetCurrentClassLogger(typeof(OperationNumberGenerationController));
            _operationNumberGenerationServices = operationNumberGenerationServices;
        }
        #endregion

        /// <summary>
        /// Получить номер операции
        /// </summary>
        /// <param name="parametr"></param>  
        [HttpPost("GetOperationNumber")]       
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [Authorize]
        public IActionResult GetOperationNumber([FromBody] IntegrationModuleSetDTO integrationModule)
        {
            try
            {
               var result = _operationNumberGenerationServices.GetOperationNumber(integrationModule);
               return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);           
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }      
    }
}
