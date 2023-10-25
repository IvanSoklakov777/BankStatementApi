using AutoMapper;
using BankStatementApi.BLL.BusinessLogic.Event;
using BankStatementApi.DAL.Interfaces;
using Ibzkh_SecurityNET6.IdentityProvider;

namespace BankStatementApi.BLL.Infrastructure
{
    public interface IServiceInfrastructure
    {     
        string Encoding { get; }
        IMapper Mapper { get; }
        IBankStatementUnitOfWork Repository { get; }
        IConfiguration Configuration { get; }
        IProducerUnitOfWork Producer { get; }
        IIdentityProvider IdentityProvider { get; }
    }
    public class ServiceInfrastructure : IServiceInfrastructure
    {
        public string Encoding { get; }
        public IMapper Mapper { get; }
        public IBankStatementUnitOfWork Repository { get; }      
        public IConfiguration Configuration { get; }
        public IProducerUnitOfWork Producer { get; }
        public IIdentityProvider IdentityProvider { get; }

        public ServiceInfrastructure(IMapper mapper, IBankStatementUnitOfWork repository, IConfiguration configuration, IProducerUnitOfWork producer, IIdentityProvider identityProvider)
        {
            Mapper = mapper;
            Repository = repository;         
            Configuration = configuration;
            Producer = producer;
            IdentityProvider = identityProvider;
            Encoding = configuration.GetSection("Project").GetSection("Encoding").Value;
        }        
    }
}
