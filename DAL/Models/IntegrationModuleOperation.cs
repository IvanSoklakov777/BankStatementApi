using BankStatementApi.DAL.Entities.Interfaces;

namespace BankStatementApi.DAL.Entities
{
    public class IntegrationModuleOperation : IBaseEntityFields<int>
    {
        public int Id { get; set; }
        public string ModuleName { get;set;}
        public int? WorkerChangedById { get; set; }
        public DateTime ChangeDate { get; set; }
        public Guid OperationTypeId { get; set; }
        public OperationType OperationType { get; set; }
    }
}
