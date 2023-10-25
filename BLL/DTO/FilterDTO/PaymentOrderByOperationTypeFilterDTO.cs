namespace BankStatementApi.BLL.DTO.FilterDTO
{
    /// <summary>
    /// ПлатежноеПоручение DTO
    /// </summary>
    public class PaymentOrderByOperationTypeFilterDTO
    {
        public ICollection<Guid> OperationTypeIds { get; set;} 
        public DateTime DateStart {get;set;}    
        public DateTime DateEnd {get;set;}
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
