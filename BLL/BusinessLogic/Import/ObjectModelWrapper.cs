using BankStatementApi.BLL.Infrastructure;
using BankStatementApi.DAL.Entities;

namespace BankStatementApi.BLL.BusinessLogiс.Import
{
    /// <summary>
    /// Оболочка объектной модели
    /// </summary> 
    public class ObjectModelWrapper
    {
        #region CTOR
        private readonly int _id;
        private readonly IServiceInfrastructure _serviceInfrastructure;
        private DataLog modelInstance;

        public ObjectModelWrapper( int id, IServiceInfrastructure serviceInfrastructure)
        {
            _id = id;
            _serviceInfrastructure = serviceInfrastructure;
        }
        #endregion
        public DataLog ModelInstance
        {
            get
            {
                if( modelInstance == null )
                    modelInstance = _serviceInfrastructure.Repository.GetRepository<DataLog>().SingleOrDefault(x => x.Id == _id);

                return modelInstance;
            }
        }

        /// <summary>
        /// Экземпляр набора
        /// </summary> 
        public void SetInstance( DataLog model )
        {
            modelInstance = model;
        }
    }
}
