using BankStatementApi.BLL.DTO.SetDTO;
using BankStatementApi.BLL.Infrastructure;
using BankStatementApi.BLL.Services.Interfaces;
using BankStatementApi.DAL.Entities;

namespace BankStatementApi.BLL.Services
{
    public class OperationNumberGenerationServices : IOperationNumberGenerationServices
    {
        private readonly IServiceInfrastructure _serviceInfrastructure;
        #region Private
        public OperationNumberGenerationServices(IServiceInfrastructure serviceInfrastructure)
        {
            _serviceInfrastructure = serviceInfrastructure;
        }
        #endregion

        /// <summary>
        /// Получить идентификатор внешнего модуля
        /// </summary>
        public int GetOperationNumber(IntegrationModuleSetDTO integrationModule)
        {
            var module = new IntegrationModuleOperation
            {
                ModuleName = integrationModule.ModuleName,
                OperationTypeId = integrationModule.OperationTypeId,
                ChangeDate = DateTime.UtcNow,
                WorkerChangedById = _serviceInfrastructure.IdentityProvider.WorkerId

            };
            _serviceInfrastructure.Repository.GetRepository<IntegrationModuleOperation>().Add(module);
            return module.Id;
        }            
    }
}
