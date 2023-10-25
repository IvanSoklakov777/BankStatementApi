namespace BankStatementApi.BLL.DTO.FilterDTO
{
    /// <summary>
    /// ПлатежноеПоручение по идентификатору ДокументаБанковскогоСчета DTO
    /// </summary>
    public class PaymentOrdersByBankAccountDocumentIdFilterDTO
    {
        public int BankAccountDocumentId { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
