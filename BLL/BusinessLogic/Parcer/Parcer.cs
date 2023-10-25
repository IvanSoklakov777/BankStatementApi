using BankStatementApi.BLL.Infrastructure;
using BankStatementApi.DAL.Entities;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace BankStatementApi.BLL.BusinessLogiс.Tools.ImportData.DataParcer
{
    /// <summary>
    /// Клиент-Банк импортер
    /// </summary> 
    public static class ClientBankParcer
    {
        /// <summary>
        /// Заполнить выписку с банковского счета
        /// </summary> 
        public static ParcerDocument FillBankAccountStatement( byte [] fileData, IServiceInfrastructure serviceInfrastructure)
        {
            var str = Encoding.GetEncoding(serviceInfrastructure.Encoding).GetString(fileData);
            return FillBankAccountStatement(str);
        }

        /// <summary>
        /// Заполнение банковской учетной записи
        /// </summary> 
        public static ParcerDocument FillBankAccountStatement( string document )
        {
            ParcerDocument clientBankParcingDocument = null;
            document = string.Join(Environment.NewLine ,
                document.Split(new string [] { "\n" , "\r\n" } , StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries));

            using( var stringReader = new StringReader(document) )
            {
                var typeLoad = stringReader.ReadLine();
                if( typeLoad != null && typeLoad.Contains("1CClientBank") )
                {
                    clientBankParcingDocument = SetPropertiesObject<ParcerDocument>(stringReader);
                    clientBankParcingDocument.BankAccounts = SetCollectionPropertiesObject<BankAccountDocument>(stringReader);
                    clientBankParcingDocument.PaymentOrders = new List<PaymentOrder>();
                    do
                    {                      
                        var order = SetPropertiesObject<PaymentOrder>(stringReader);
                        if( order != null )
                            clientBankParcingDocument.PaymentOrders.Add(order);
                    } while( stringReader.ReadLine() != null && stringReader.Peek() != -1 );
                }
            }
            return clientBankParcingDocument;
        }

        /// <summary>
        /// Набор свойств объекта
        /// </summary> 
        private static T SetPropertiesObject<T>( StringReader reader ) where T : class, new()
        {
            var line = reader.ReadLine();
            if( line == null )
                return null;

            var resultObject = new T();
            var descriptionAttributes = ParcerHelper.GetParcerAttributeInfo<T>();

            do
            {
                if( line != null )
                {
                    var data = line.Split('=');
                    if( data.Length == 1 )
                        break;

                    var value = data [ 1 ].Trim();
                    if( string.IsNullOrEmpty(value) )
                        continue;

                    descriptionAttributes.ForEach(propertyData =>
                    {
                        if( propertyData.Value.Descriptions.Contains(data [ 0 ].Trim()) )
                        {
                            if( !string.IsNullOrEmpty(value) )
                            {
                                ParcerHelper.SetValue(propertyData.Key , resultObject , value);
                            }
                        }
                    });
                }
            } while( ( line = reader.ReadLine() ) != null );
            return resultObject;
        }

        /// <summary>
        /// Установить свойства коллекции
        /// </summary> 
        private static List<T> SetCollectionPropertiesObject<T>( StringReader reader) where T : class, new()
        {
            var resultObject = new List<T>();
            var sectionPdAttribute = typeof(T).GetCustomAttribute<ClientBankParcerSectionAttribute>();
            if( sectionPdAttribute == null )
                return resultObject;
            var startSection = sectionPdAttribute.MarkStartSection;
            var endSection = sectionPdAttribute.MarkEndSection;
            do
            {
                resultObject.Add(SetPropertiesObject<T>(reader));
            } while( reader.ReadLine() == startSection );
            return resultObject;
        }

        /// <summary>
        /// Чтение свойств
        /// </summary> 
        public static string ReadProperty<TObj>( Expression<Func<TObj>> property )
        {
            var propertyInfo = ( ( MemberExpression ) property.Body ).Member as PropertyInfo;
            if( propertyInfo == null )
                return $"ReadProperty Error - {property.ToString()}";
            var attribute = propertyInfo.GetCustomAttribute<ParcerPropertyAttribute>();
            var propertyDescription = attribute != null ? string.Join(String.Empty , attribute.Descriptions) : propertyInfo.Name;
            var res = $"{propertyDescription} - {property.Compile()()}";
            return res;
        }
    }
}

