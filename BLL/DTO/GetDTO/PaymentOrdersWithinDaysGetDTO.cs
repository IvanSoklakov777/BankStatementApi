using BankStatementApi.BLL.DTO.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace BankStatementApi.BLL.DTO.GetDTO
{
    /// <summary>
    /// ПлатежныеПоручения за период DTO
    /// </summary>
    public class PaymentOrdersWithinDaysGetDTO : ICsv
    {
        [MaxLength(20)]
        public string RecipientCalcAccount { get; set; }
        [MaxLength(9)]
        public string RecipientBIK { get; set; }
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public decimal Sum { get; set; }
        public string PaymentPurpose { get; set; }

        public string ToCsv() => $"{RecipientCalcAccount};{RecipientBIK};{Number};{Date};{ReceivedDate};{Sum};{PaymentPurpose};";
    }
}
