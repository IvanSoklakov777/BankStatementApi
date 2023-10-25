namespace BankStatementApi.BLL.BusinessLogiс.Tools.ImportData.DataParcer
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ParcerPropertyAttribute : Attribute
    {
        /// <summary>
        /// Все возможные описания аттрибута (К примеру на различных языках)
        /// </summary>
        public readonly string [] Descriptions;

        public ParcerPropertyAttribute( string [] descriptions )
        {
            Descriptions = descriptions;
        }
    }

    /// <summary> 
    /// Аттрибут задающий описание класса. 
    /// Данный аттрибут используется в классах 
    /// для загрузки и сериализации файлов,
    /// полученных из банк-клиента
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ClientBankParcerSectionAttribute : Attribute
    {
        public string MarkStartSection { get; }
        public string MarkEndSection { get; }

        public ClientBankParcerSectionAttribute( string markStartSection , string markEndSection )
        {
            MarkStartSection = markStartSection;
            MarkEndSection = markEndSection;
        }
    }
}
