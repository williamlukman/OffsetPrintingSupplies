using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface ICustomerStockAdjustmentDetailRepository : IRepository<CustomerStockAdjustmentDetail>
    {
        IQueryable<CustomerStockAdjustmentDetail> GetQueryable();
        IList<CustomerStockAdjustmentDetail> GetAll();
        IList<CustomerStockAdjustmentDetail> GetAllByMonthCreated();
        IList<CustomerStockAdjustmentDetail> GetObjectsByCustomerStockAdjustmentId(int customerStockAdjustmentId);
        IList<CustomerStockAdjustmentDetail> GetObjectsByItemId(int itemId);
        CustomerStockAdjustmentDetail GetObjectById(int Id);
        CustomerStockAdjustmentDetail CreateObject(CustomerStockAdjustmentDetail customerStockAdjustmentDetail);
        CustomerStockAdjustmentDetail UpdateObject(CustomerStockAdjustmentDetail customerStockAdjustmentDetail);
        CustomerStockAdjustmentDetail SoftDeleteObject(CustomerStockAdjustmentDetail customerStockAdjustmentDetail);
        bool DeleteObject(int Id);
        CustomerStockAdjustmentDetail ConfirmObject(CustomerStockAdjustmentDetail customerStockAdjustmentDetail);
        CustomerStockAdjustmentDetail UnconfirmObject(CustomerStockAdjustmentDetail customerStockAdjustmentDetail);
        string SetObjectCode(string ParentCode);
    }
}