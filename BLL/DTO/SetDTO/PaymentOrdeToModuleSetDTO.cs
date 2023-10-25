using System.ComponentModel.DataAnnotations;

namespace BankStatementApi.BLL.DTO.SetDTO
{
    /// <summary>
    /// Платежное Поручение Модулям DTO
    /// </summary>
    public class PaymentOrdeToModuleSetDTO
    {
        [Required]
        public List<int> PaymentOrderIds { get; set; }
        public Guid OperationTypeId { get; set; }
    }
}
