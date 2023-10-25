using Ibzkh_SecurityNET6.IdentityProvider;
using MediatR;
using Newtonsoft.Json;
using RMQCommonCore.Model;

namespace BankStatementApi.BLL.BusinessLogic.Event.Model
{
    public class PaymentOrderEvent : IntegrationEvent, IRequest<Unit>
    {
        [JsonProperty]
        public int PaymentId { get; set; }
        [JsonProperty]
        public string Number { get; set; }
        [JsonProperty]
        public decimal Sum { get; set; }
        [JsonProperty]
        public DateTime Date { get; set; }
        [JsonProperty]
        public string PayerINN { get; set; }
        [JsonProperty]
        public string PayerAccount { get; set; }
        [JsonProperty]
        public string RecipientINN { get; set; }
        [JsonProperty]
        public string RecipientAccount { get; set; }
        [JsonProperty]
        public string PaymentPurpose { get; set; }
        [JsonProperty]
        public int? IntegrationModuleOperationId { get; set; }
        [JsonConstructor]
        public PaymentOrderEvent(IIdentityProvider provider) :base(provider) {}
        public PaymentOrderEvent(int? workerId, int? legalEntityId) : base(workerId, legalEntityId) { }
        public PaymentOrderEvent(int paymentId, string number, decimal sum, DateTime date, string payerINN, string payerAccount, string recipientINN, string recipientAccount, string paymentPurpose, int? integrationModuleOperationId, IIdentityProvider provider) : this(provider)
        {
            PaymentId = paymentId;
            Number = number;
            Sum = sum;
            Date = date;
            PayerINN = payerINN;
            PayerAccount = payerAccount;
            RecipientINN = recipientINN;
            RecipientAccount = recipientAccount;
            PaymentPurpose = paymentPurpose;
            IntegrationModuleOperationId = integrationModuleOperationId;
        }
    }
}
