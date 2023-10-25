using BankStatementApi.DAL.Entities.Enum;

namespace BankStatementApi.BLL.DTO.GetDTO
{
    /// <summary>
    /// ЖурналДанных DTO
    /// </summary>
    public class DataLogGetDTO
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FileHash { get; set; }
        public DateTime DateStartLoad { get; set; }
        public DateTime DateEndLoad { get; set; }
        public DateTime? DateFileStart { get; set; }
        public DateTime? DateFileEnd { get; set; }
        public int FileSize { get; set; }
        public int? WorkerChangedById { get; set; }
        public DateTime ChangeDate { get; set; }
        public ImportResultEnum ImportResultId { get; set; }    
        public ICollection<BankAccountDocumentGetDTO> BankAccountDocuments { get; set; }
    }
}
