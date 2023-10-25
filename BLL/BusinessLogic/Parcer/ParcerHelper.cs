using System.Globalization;
using System.Reflection;

namespace BankStatementApi.BLL.BusinessLogiс.Tools.ImportData.DataParcer
{
    /// <summary>
    /// Чтение свойств
    /// </summary> 
    public class ParcerHelper
    {
        /// <summary>
        /// Получить информацию об атрибутах
        /// </summary> 
        public static List<KeyValuePair<PropertyInfo , ParcerPropertyAttribute>> GetParcerAttributeInfo<T>() where T : class, new()
        {
            var descriptionAttributes = typeof(T)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .ToDictionary(p => p ,
                    x => Attribute.GetCustomAttribute(x , typeof(ParcerPropertyAttribute)) as
                        ParcerPropertyAttribute)
                .Where(x => x.Value != null)
                .ToList();
            return descriptionAttributes;
        }

        /// <summary>
        /// Установленние значений
        /// </summary> 
        public static void SetValue<T>( PropertyInfo propertyInfo , T targetObject , string value ) where T : class, new()
        {
            if( propertyInfo.PropertyType == typeof(DateTime) || propertyInfo.PropertyType == typeof(DateTime?) )
            {          
                propertyInfo.SetValue(targetObject , DateTime.SpecifyKind(DateTime.Parse(value), DateTimeKind.Utc));
            }
            else if( propertyInfo.PropertyType == typeof(Decimal) )
            {
                propertyInfo.SetValue(targetObject ,
                    Decimal.TryParse(value , out var result) ? result : decimal.Parse(value , CultureInfo.InvariantCulture));
            }
            else if( propertyInfo.PropertyType == typeof(int) )
            {
                propertyInfo.SetValue(targetObject ,
                    Int32.TryParse(value , out var result) ? result : Int32.Parse(value , CultureInfo.InvariantCulture));
            }
            else if( propertyInfo.PropertyType == typeof(string) )
            {
                var exisistsValue = ( string ) propertyInfo.GetValue(targetObject);
                if( exisistsValue == null )
                    exisistsValue = value;
                else
                    exisistsValue += $" {value}";

                propertyInfo.SetValue(targetObject , exisistsValue);
            }
            else if( propertyInfo.PropertyType == typeof(List<string>) )
            {
                var existsValue = ( List<string> ) propertyInfo.GetValue(targetObject);
                existsValue.Add(value);
                propertyInfo.SetValue(targetObject , existsValue);
            }
            else
            {
                propertyInfo.SetValue(targetObject , value);
            }
        }
    }
}
