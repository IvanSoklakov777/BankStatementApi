namespace BankStatementApi.BLL.DTO.Interfaces
{
    /// <summary>
    /// Интерфейс конвертера Csv
    /// </summary>
    public interface ICsv
    {
        /// <summary>
        /// Преобразовать в Csv
        /// </summary>
        string ToCsv();
    }
}
