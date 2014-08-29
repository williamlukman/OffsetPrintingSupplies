using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IPurchaseOrderDetailRepository : IRepository<PurchaseOrderDetail>
    {
        IQueryable<PurchaseOrderDetail> GetQueryable();
        IList<PurchaseOrderDetail> GetAll();
        IList<PurchaseOrderDetail> GetAllByMonthCreated();
        IQueryable<PurchaseOrderDetail> GetQueryableObjectsByPurchaseOrderId(int purchaseOrderId);
        IList<PurchaseOrderDetail> GetObjectsByPurchaseOrderId(int purchaseOrderId);
        IList<PurchaseOrderDetail> GetObjectsByItemId(int itemId);
        PurchaseOrderDetail GetObjectById(int Id);
        PurchaseOrderDetail CreateObject(PurchaseOrderDetail purchaseOrderDetail);
        PurchaseOrderDetail UpdateObject(PurchaseOrderDetail purchaseOrderDetail);
        PurchaseOrderDetail SoftDeleteObject(PurchaseOrderDetail purchaseOrderDetail);
        bool DeleteObject(int Id);
        PurchaseOrderDetail ConfirmObject(PurchaseOrderDetail purchaseOrderDetail);
        PurchaseOrderDetail UnconfirmObject(PurchaseOrderDetail purchaseOrderDetail);
        string SetObjectCode(string ParentCode);
    }
}