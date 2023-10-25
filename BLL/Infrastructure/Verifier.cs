using BankStatementApi.BLL.DTO.FilterDTO;
using BankStatementApi.BLL.DTO.SetDTO;

namespace BankStatementApi.BLL.Infrastructure
{
    /// <summary>
    /// Верификатор
    /// </summary>
    public static class Verifier
    {
        /// <summary>
        /// Проверка валидности фильтра
        /// </summary>
        public static bool CheckFilterForCorrect(PaymentOrderByFeaturesFilterDTO filter)
        {
            if (string.IsNullOrEmpty(filter.PayerBankAccount) &&
                string.IsNullOrEmpty(filter.PayerINN) &&
                string.IsNullOrEmpty(filter.PayerKPP) &&
                string.IsNullOrEmpty(filter.RecipientBankAccount) &&
                string.IsNullOrEmpty(filter.RecipientINN) &&
                string.IsNullOrEmpty(filter.PayerKPP) &&
                filter.Sum == null &&
                filter.Number == null &&
                filter.DateFrom == null &&
                filter.DateTo == null
               )
                return true;
            return false;
        }
    }
}
