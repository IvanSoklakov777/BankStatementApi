using BankStatementApi.BLL.DTO.SetDTO;

namespace BankStatementApi.BLL.Services.Interfaces
{
    public interface IOperationNumberGenerationServices
    {
        int GetOperationNumber(IntegrationModuleSetDTO integrationModule);
    }
}
