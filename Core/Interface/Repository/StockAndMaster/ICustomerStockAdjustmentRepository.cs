using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface ICustomerStockAdjustmentRepository : IRepository<CustomerStockAdjustment>
    {
        IQueryable<CustomerStockAdjustment> GetQueryable();
        IList<CustomerStockAdjustment> GetAll();
        IList<CustomerStockAdjustment> GetAllByMonthCreated();
        CustomerStockAdjustment GetObjectById(int Id);
        CustomerStockAdjustment CreateObject(CustomerStockAdjustment customerStockAdjustment);
        CustomerStockAdjustment UpdateObject(CustomerStockAdjustment customerStockAdjustment);
        CustomerStockAdjustment SoftDeleteObject(CustomerStockAdjustment customerStockAdjustment);
        bool DeleteObject(int Id);
        CustomerStockAdjustment ConfirmObject(CustomerStockAdjustment customerStockAdjustment);
        CustomerStockAdjustment UnconfirmObject(CustomerStockAdjustment customerStockAdjustment);
        string SetObjectCode();
    }
}