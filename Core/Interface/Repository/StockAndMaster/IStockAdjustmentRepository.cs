using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IStockAdjustmentRepository : IRepository<StockAdjustment>
    {
        IQueryable<StockAdjustment> GetQueryable();
        IList<StockAdjustment> GetAll();
        IList<StockAdjustment> GetAllByMonthCreated();
        StockAdjustment GetObjectById(int Id);
        StockAdjustment CreateObject(StockAdjustment stockAdjustment);
        StockAdjustment UpdateObject(StockAdjustment stockAdjustment);
        StockAdjustment SoftDeleteObject(StockAdjustment stockAdjustment);
        bool DeleteObject(int Id);
        StockAdjustment ConfirmObject(StockAdjustment stockAdjustment);
        StockAdjustment UnconfirmObject(StockAdjustment stockAdjustment);
        string SetObjectCode();
    }
}