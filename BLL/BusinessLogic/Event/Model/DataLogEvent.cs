using MediatR;
using RMQCommonCore.Model;
using Newtonsoft.Json;
using Ibzkh_SecurityNET6.IdentityProvider;

namespace BankStatementApi.BLL.BusinessLogic.Event.Model
{
    public class DataLogEvent : IntegrationEvent, IRequest<Unit>
    {
        [JsonProperty]
        public int DataLogId { get; set; }
        [JsonConstructor]
        public DataLogEvent(IIdentityProvider provider):base(provider) { }
        public DataLogEvent(int? workerId, int? legalEntityId) : base(workerId, legalEntityId) { }
        public DataLogEvent(int dataLogId, IIdentityProvider provider): this(provider)
        {
            DataLogId = dataLogId;
        }
    }
}
