namespace BankStatementApi.BLL.Infrastructure.Extensions
{
    /// <summary>
    /// Расширение исключения
    /// </summary>
    public static class ExceptionExtension
    {
        public static string GetMessage( this Exception exception , bool includeStack )
        {
            string resultMessage = exception.Message;
            string stack = exception.StackTrace;
            while( exception.InnerException != null )
            {
                resultMessage += Environment.NewLine + exception.Message;
                exception = exception.InnerException;
            }
            return includeStack ? resultMessage + Environment.NewLine + stack : resultMessage;
        }

        public static string GetFullExceptionMessage( this Exception ex )
        {
            if( ex.InnerException != null )
                return ex.Message + Environment.NewLine + ex.InnerException.GetFullExceptionMessage();

            return ex.Message + Environment.NewLine + ex.StackTrace;
        }       
    }
}
