using System.Runtime.Serialization;

namespace BankStatementApi.DAL.Entities.Enum
{
    public class DictionaryTypeAttribute : Attribute
    {
        public DictionaryTypeAttribute(TypeDictionary type)
        {
            Type = type;
        }
        public TypeDictionary Type { get; set; }
    }

    [DataContract(Name = "TypeDictionary")]
    public enum TypeDictionary
    {

        TransferTypeEnum = 1,
        ImportResultEnum = 2,
        OperationTypeEnum = 3
    }
}

