using System.ComponentModel;

namespace BankStatementApi.BLL.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static T Convert<T>(this string stringKey)
        {
            var typeConverter = TypeDescriptor.GetConverter(typeof(T));
            if (typeConverter != null && typeConverter.CanConvertFrom(typeof(string)) && typeConverter.IsValid(stringKey))
            {
                return (T)typeConverter.ConvertFrom(stringKey);
            }
            return default(T);

        }
    }
}
