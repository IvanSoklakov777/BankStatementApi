namespace BankStatementApi.DAL.Entities.Enum
{
    /// <summary>
    /// Перечисление результатов импорта 
    /// </summary>
    public enum ImportResultEnum : int
    {
        /// <summary>
        /// Файл в обработке 
        /// </summary>
        FileInProcessing = 1,
        /// <summary>
        /// Файл обработано с ошибкой
        /// </summary>
        ProcessedWithError = 2,
        /// <summary>
        /// Файл обработано успешно 
        /// </summary>
        ProcessedSuccessfully = 3,
        /// <summary>
        /// Файл в очереди 
        /// </summary>
        InQueue = 4,
    }
}
