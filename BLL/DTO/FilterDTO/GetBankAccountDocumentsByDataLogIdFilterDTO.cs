namespace BankStatementApi.BLL.DTO.FilterDTO
{
    public class GetBankAccountDocumentsByDataLogIdFilterDTO
    {
        public int DataLogId { get; set; }       
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
