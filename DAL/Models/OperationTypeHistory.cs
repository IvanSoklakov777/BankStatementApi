using BankStatementApi.DAL.Entities.Interfaces;

namespace BankStatementApi.DAL.Entities
{
    /// <summary>
    /// История изменения типа операции 
    /// </summary>
    public class OperationTypeHistory : IBaseEntityFields<int>
    {
        /// <summary>
        /// Идентификтор 
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Идентификтор работника 
        /// </summary>
        public int? WorkerChangedById { get; set; }
        /// <summary>
        /// Дата изменения 
        /// </summary>
        public DateTime ChangeDate { get; set; }

        #region Navigation
        /// <summary>
        /// Идентификтор ПлатежногоПоручения
        /// </summary>
        public int PaymentOrderId { get; set; }
        /// <summary>
        /// Навигационное свойство ПлатежногоПоручения
        /// </summary>
        public virtual PaymentOrder PaymentOrder {get;set;}
        /// <summary>
        /// Идентификтор ТипаОперации 
        /// </summary>
        public Guid? OperationTypeId { get; set; }
        /// <summary>
        /// Навигационное свойство ТипаОперации
        /// </summary>
        public virtual OperationType OperationType { get; set; }    
        #endregion
    }
}
