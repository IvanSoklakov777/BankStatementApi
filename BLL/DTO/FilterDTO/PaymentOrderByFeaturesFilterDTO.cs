using System.ComponentModel.DataAnnotations;

namespace BankStatementApi.BLL.DTO.FilterDTO
{
    public class PaymentOrderByFeaturesFilterDTO
    {
        #region Плательщик
        public string PayerINN { get; set; }
        public string PayerKPP { get; set; }
        public string PayerBankAccount { get; set; }
        #endregion

        #region Получатель
        public string RecipientINN { get; set; }
        public string RecipientKPP { get; set; }
        public string RecipientBankAccount { get; set; }
        #endregion
        [Required]
        public DateTime? DateFrom { get; set; }
        [Required]
        public DateTime? DateTo { get; set; }
        public decimal? Sum { get; set; }
        public string Number { get; set; }
       
    }
}
