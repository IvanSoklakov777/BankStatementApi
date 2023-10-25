using AutoMapper;
using BankStatementApi.BLL.DTO.GetDTO;
using BankStatementApi.BLL.DTO.SetDTO;
using BankStatementApi.DAL.Entities;
using IbZKH_CustomTypes.GenericTypes;

namespace BankStatementApi.BLL.Infrastructure
{
    /// <summary>
    /// Маппер
    /// </summary>
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BankAccountDocument , BankAccountDocumentGetDTO>();
            CreateMap<BankAccountDocumentGetDTO, BankAccountDocument>();
            CreateMap<PaymentOrder , PaymentOrderGetDTO>().ForMember(x => x.Recognized, x => x.MapFrom(y => y.OperationTypeId.HasValue ));
            CreateMap<DataLog, DataLogGetDTO>();//.ForMember(x => x.BankAccountDocuments, y=>y.MapFrom(z=>z.BankAccountDocuments));
            CreateMap<DictionaryOperationTypeSetDTO , OperationType>();      
            CreateMap<PaymentOrder , PaymentOrdersWithinDaysGetDTO>();
            CreateMap<OperationType , KeyValueItem<Guid>>().ForMember(x => x.Name , x => x.MapFrom(y => y.Title));
            CreateMap<TransferType , KeyValueItem<int>>().ForMember(x => x.Name , x => x.MapFrom(y => y.Title));
            CreateMap<ImportResult , KeyValueItem<int>>().ForMember(x => x.Name , x => x.MapFrom(y => y.Title));          
        }
    }
}
