using Newtonsoft.Json;
using RMQCommonCore.Model;
using RMQCommonCore.Model.Enum;
using System.ComponentModel.DataAnnotations;

namespace BankStatementApi.DAL.Entities
{
    public class IntegrationEventLog
    {
        private IntegrationEventLog() { }
        public IntegrationEventLog(IntegrationEvent @event)
        {
            EventId = @event.Id;
            CreationTime = @event.CreationDate;
            EventTypeName = @event.GetType().FullName;
            Content = JsonConvert.SerializeObject(@event);
            State = EventStateEnum.NotPublished;
            ProcessingState = ProcessingStateEnum.None;
            TimesSent = 0;
        }
        [Key]
        public Guid EventId { get; private set; }
        public string EventTypeName { get; private set; }
        public EventStateEnum State { get; set; }
        public int TimesSent { get; set; }
        public DateTime CreationTime { get; private set; }
        public string Content { get; private set; }
        public ProcessingStateEnum ProcessingState { get; set; }
        public string ProcessedBy { get; set; }
        public string ErrorMessage { get; set; }
    }
}
