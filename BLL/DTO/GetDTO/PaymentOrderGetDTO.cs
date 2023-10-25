namespace BankStatementApi.BLL.DTO.GetDTO
{
    /// <summary>
    /// Платежное поручение DTO
    /// </summary>
    public class PaymentOrderGetDTO
    {
        public int Id { get; set; }
        public string Number { get; set; }      
        public DateTime Date { get; set; }
        public decimal Sum { get; set; }
        #region Плательщик
        public string Payer { get; set; }
        public string PayerAccount { get; set; }
        public string PayerCorAccount { get; set; }
        public string PayerCalcAccount { get; set; }
        public string PayerBank { get; set; }
        public string PayerINN { get; set; }
        public string PayerBIK { get; set; }
        public string PayerKPP { get; set; }
        #endregion
        #region Получатель
        public string Recipient { get; set; }        
        public string RecipientAccount { get; set; }
        public string RecipientINN { get; set; } 
        public string RecipientBIK { get; set; }      
        public string RecipientKPP { get; set; }    
        public string RecipientCorAccount { get; set; } 
        public string RecipientCalcAccount { get; set; }
        public string RecipientBank { get; set; }
        #endregion
        public DateTime? ReceivedDate { get; set; }
        public DateTime? WriteOffDate { get; set; }
        public string PaymentType { get; set; }    
        public DateTime PaymentTerm { get; set; }         
        public string Priority { get; set; }
        public string PaymentPurpose { get; set; }
        public int? WorkerChangedById { get; set; }
        public bool Recognized { get; set; }
        public DateTime ChangeDate { get; set; }
        public Guid? OperationTypeId { get; set; }

    }
}
