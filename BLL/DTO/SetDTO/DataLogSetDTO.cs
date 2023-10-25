namespace BankStatementApi.BLL.DTO.SetDTO
{
    public class DataLogSetDTO
    {
        public int Id { get; set; }
        public int? WorkerId { get; set; }
        public int? LegalEntityId { get; set; }
        public byte[] FileData { get; set; }
    }
}
