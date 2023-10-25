using BankStatementApi.BLL.BusinessLogic.Event.Model;
using MediatR;
using RMQCommonCore.Interfaces;
using RMQCommonCore.Model;

namespace BankStatementApi.BLL.BusinessLogic.Event
{
    public class ProducerUnitOfWork : IProducerUnitOfWork
    {
        private readonly IRabbitMqProducer<DataLogEvent> _dataLogProducer;
        private readonly IRabbitMqProducer<PaymentOrderEvent> _paymentOrderProducer;
   
        public ProducerUnitOfWork(IRabbitMqProducer<DataLogEvent> dataLogProducer, IRabbitMqProducer<PaymentOrderEvent> paymentOrderProducer)
        {
            _dataLogProducer = dataLogProducer;
            _paymentOrderProducer = paymentOrderProducer;
        }

        private Dictionary<Type, object> _repositories;
        public IRabbitMqProducer<TEntity> GetProducer<TEntity>() where TEntity : IntegrationEvent, IRequest<Unit>
        {
            if (_repositories == null)
                _repositories = new Dictionary<Type, object>();
            var type = typeof(TEntity);
            if (!_repositories.ContainsKey(type))
                switch (typeof(TEntity).Name)
                {
                    case nameof(DataLogEvent):
                        {
                            _repositories[type] = _dataLogProducer;
                            break;
                        }
                    case nameof(PaymentOrderEvent):
                        {
                            _repositories[type] = _paymentOrderProducer;
                            break;
                        }
                }
            return (IRabbitMqProducer<TEntity>)_repositories[type];
        }    
    }

    public interface IProducerUnitOfWork
    {
        IRabbitMqProducer<TEntity> GetProducer<TEntity>() where TEntity : IntegrationEvent, IRequest<Unit>;
    }  
}
