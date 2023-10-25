using System.ComponentModel.DataAnnotations;

namespace BankStatementApi.BLL.DTO.SetDTO
{
    /// <summary>
    /// Словарь ТипОперации DTO
    /// </summary>
    public class DictionaryOperationTypeSetDTO
    {
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
    }
}
