
namespace BankStatementApi.BLL.BusinessLogiс.Import
{
    /// <summary>
    /// Добавить сообщение
    /// </summary> 
    public class ValidationMessage
    {
        public string Message { get; set; }
        public ValidationLevel Level { get; set; }
    }

    /// <summary>
    /// Перечисление уровней проверки
    /// </summary> 
    public enum ValidationLevel
    {
        Warning,
        Error,
    }

    /// <summary>
    /// Проверка результата
    /// </summary> 
    public class ValidateResult
    {
        private readonly ValidationLevel defaultMesasgeLevel = ValidationLevel.Error;
        public List<ValidationMessage> Message { get; } = new List<ValidationMessage>();

        #region CTOR
        private ValidateResult() { }
        #endregion

        public static ValidateResult Success() => new ValidateResult();

        /// <summary>
        /// Проврека наличие ошибок
        /// </summary> 
        public bool HasError => Message.Any(mes => mes.Level == ValidationLevel.Error);

        /// <summary>
        /// Добавление ошибки
        /// </summary> 
        public static ValidateResult Failed( string errorMessage )
        {
            return new ValidateResult
            {
                Message =
                {
                    new ValidationMessage
                    {
                        Level = ValidationLevel.Error, Message = errorMessage
                    }
                }
            };
        }

        #region Add Message
        /// <summary>
        /// Добавить сообщение
        /// </summary> 
        public void AddMessage( List<ValidationMessage> messages ) => Message.AddRange(messages);
        /// <summary>
        /// Добавить сообщение
        /// </summary> 
        public void AddMessage( params ValidationMessage [] messages ) => Message.AddRange(messages);
        /// <summary>
        /// Добавить сообщение
        /// </summary> 
        public void AddMessage( string message )
        {
            Message.Add(
                new ValidationMessage()
                {
                    Message = message ,
                    Level = defaultMesasgeLevel
                });
        }
        /// <summary>
        /// Добавить сообщение
        /// </summary> 
        public void AddMessage( string message , ValidationLevel level )
        {
            Message.Add(
                new ValidationMessage()
                {
                    Message = message ,
                    Level = level
                });
        }
        /// <summary>
        /// Добавить сообщение
        /// </summary> 
        public void AddMessage( params string [] messages )
        {
            Message.AddRange(
                messages.Select(x =>
                new ValidationMessage()
                {
                    Message = x ,
                    Level = defaultMesasgeLevel
                })
            );
        }
        /// <summary>
        /// Добавить сообщение
        /// </summary> 
        public void AddMessage( IEnumerable<string> messages )
        {
            Message.AddRange(
                messages.Select(x =>
                new ValidationMessage()
                {
                    Message = x ,
                    Level = defaultMesasgeLevel
                })
            );
        }
        /// <summary>
        /// Добавить сообщение
        /// </summary> 
        public void AddMessage( ValidationLevel level , params string [] messages )
        {
            Message.AddRange(
                messages.Select(x =>
                new ValidationMessage()
                {
                    Message = x ,
                    Level = level
                })
            );
        }
        #endregion

        /// <summary>
        /// Получить одну строку
        /// </summary> 
        public string ToString( string messageDelimiter = null )
        {
            if( messageDelimiter == null )
                messageDelimiter = $";{Environment.NewLine}";
            return string.Join(messageDelimiter , Message.Where(mes => mes.Level == ValidationLevel.Error ||  mes.Level == ValidationLevel.Warning).Select(x=>x.Message));
        }
    }    
}
