using BankStatementApi.BLL.BusinessLogic.Event.Model;
using BankStatementApi.BLL.DTO.SetDTO;
using BankStatementApi.BLL.Services.Interfaces;
using MediatR;
using NLog;

namespace BankStatementApi.BLL.BusinessLogic.Event.Consumer
{
    /// <summary>
    /// Обработчик событий ЖурналаДанных
    /// <summary>  
    public class ReceivedDataLogEventHandler : IRequestHandler<DataLogEvent>
    {
        private readonly NLog.ILogger _logger;
        private readonly IBankStatementServices _services;

        public ReceivedDataLogEventHandler(IServiceProvider servicesProvider)
        {
            _logger = LogManager.GetCurrentClassLogger();
            _services = servicesProvider.CreateAsyncScope().ServiceProvider.GetRequiredService<IBankStatementServices>();
        }

        /// <summary>
        /// Получение идентификтора ЖурналаДанных из очереди
        /// <summary>  
        public Task<Unit> Handle(DataLogEvent request, CancellationToken cancellationToken)
        {
            try
            {
                object locker = new();
                lock (locker)
                {
                    if (request.DataLogId != 0)
                    {
                        var dataLog = new DataLogSetDTO
                        {
                            Id = request.DataLogId,
                            WorkerId = request.WorkerId,
                            LegalEntityId = request.LegalEntityId
                        };
                        var operationResult = _services.ParsingAndSavingData(dataLog);

                        return Task.FromResult(Unit.Value);
                    }
                    throw new Exception("Полученное значение из очереди не корректное.");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);              
                return Task.FromResult(Unit.Value);
            }
        }
    }
}
