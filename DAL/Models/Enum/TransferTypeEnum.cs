namespace BankStatementApi.DAL.Entities.Enum
{
    /// <summary>
    /// Перечисление ТипаПередачи 
    /// </summary>
    public enum TransferTypeEnum : int
    {
        /// <summary>
        /// ПолученныйПлатеж 
        /// </summary>
        ReceivedPayment = 1,
        /// <summary>
        /// ОтправленныйПлатеж
        /// </summary>
        SentPayment = 2,
    }
}
