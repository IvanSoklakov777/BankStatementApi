namespace BankStatementApi.BLL.DTO.GetDTO
{
    /// <summary>
    /// БанкАккаунтДокумент DTO
    /// </summary>
    public class BankAccountDocumentGetDTO 
    {
        public int Id { get; set; }
        public string BankAccountNumber { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public decimal StartBalance { get; set; }
        public decimal TotalReceived { get; set; }
        public decimal TotalCharged { get; set; }   
        public decimal FinalBalance { get; set; }
        public int? WorkerChangedById { get; set; }
        public DateTime ChangeDate { get; set; }
    }
}
