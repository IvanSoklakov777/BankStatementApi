using BankStatementApi.BLL.DTO.Interfaces;

namespace BankStatementApi.BLL.BusinessLogic.Converter
{
    /// <summary>
    /// Конвертер
    /// </summary>  
    public static class Converter<T> where T : class, ICsv
    {
        /// <summary>
        /// Конвертер Csv формата
        /// </summary> 
        public static string CsvConverter(List<T> entitys)
       {
            string resultStr="";
            foreach(var property in typeof(T).GetProperties() )
            {
                resultStr += property.Name +";";
            }
            resultStr += Environment.NewLine;
            foreach(var entity in entitys )
            {
                resultStr += entity.ToCsv();
                resultStr += Environment.NewLine;
            }
            return resultStr;
       }
    }
}
