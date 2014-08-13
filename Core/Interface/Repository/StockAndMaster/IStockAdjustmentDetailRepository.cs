using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IStockAdjustmentDetailRepository : IRepository<StockAdjustmentDetail>
    {
        IList<StockAdjustmentDetail> GetAll();
        IList<StockAdjustmentDetail> GetAllByMonthCreated();
        IList<StockAdjustmentDetail> GetObjectsByStockAdjustmentId(int stockAdjustmentId);
        IList<StockAdjustmentDetail> GetObjectsByItemId(int itemId);
        StockAdjustmentDetail GetObjectById(int Id);
        StockAdjustmentDetail CreateObject(StockAdjustmentDetail stockAdjustmentDetail);
        StockAdjustmentDetail UpdateObject(StockAdjustmentDetail stockAdjustmentDetail);
        StockAdjustmentDetail SoftDeleteObject(StockAdjustmentDetail stockAdjustmentDetail);
        bool DeleteObject(int Id);
        StockAdjustmentDetail ConfirmObject(StockAdjustmentDetail stockAdjustmentDetail);
        StockAdjustmentDetail UnconfirmObject(StockAdjustmentDetail stockAdjustmentDetail);
        string SetObjectCode(string ParentCode);
    }
}