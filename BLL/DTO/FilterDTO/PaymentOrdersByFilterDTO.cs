using System.ComponentModel.DataAnnotations;

namespace BankStatementApi.BLL.DTO.FilterDTO
{
    /// <summary>
    /// ПлатежноеПоручение по номеру БанковскогоСчета DTO
    /// </summary>
    public class PaymentOrdersByFilterDTO
    {
        [Required]
        [StringLength(20, MinimumLength = 20, ErrorMessage = "Размер должен быть 20 цифр.")]      
        [RegularExpression(@"[0-9]\d*", ErrorMessage = "Используйте только цифры.")]
        public string BankAccountNumber { get; set; }
        public Guid? OperationTypeId { get; set; }
        public int? DataLogId { get; set; }
        [Required]
        public DateTime DateStart { get; set; }
        [Required]
        public DateTime DateEnd { get; set; }
    }
}
