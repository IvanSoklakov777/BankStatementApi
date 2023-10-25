using BankStatementApi.BLL.Infrastructure;
using BankStatementApi.DAL.Entities;

namespace BankStatementApi.BLL.BusinessLogic
{
    public class BancAccountDocumentBl
    {
        private readonly IServiceInfrastructure _serviceInfrastructure;

        #region CTOR
        public BancAccountDocumentBl(IServiceInfrastructure serviceInfrastructure)
        {
            _serviceInfrastructure = serviceInfrastructure;
        }
        #endregion

        /// <summary>
        /// Получить документы банковского счета по идентификатору журнала данных
        /// </summary>
        public IEnumerable<BankAccountDocument> GetBankAccountDocumentsByDataLogId(int dataLogId)
        {
            return _serviceInfrastructure.Repository.GetRepository<BankAccountDocument>().Where(x => x.DataLogId == dataLogId);        
        }

        /// <summary>
        /// Получить документ о банковском счете
        /// </summary>
        public BankAccountDocument GetBankAccountDocument(int documentId)
        {
            return _serviceInfrastructure.Repository.GetRepository<BankAccountDocument>().SingleOrDefault(x => x.Id == documentId);
        }
    }
}
