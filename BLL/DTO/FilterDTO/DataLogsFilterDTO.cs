using BankStatementApi.DAL.Entities.Enum;

namespace BankStatementApi.BLL.DTO.FilterDTO
{
    /// <summary>
    /// ЖурналДанных DTO
    /// </summary>
    public class DataLogsFilterDTO
    {       
        public ImportResultEnum? ImportResult { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

    }
}
