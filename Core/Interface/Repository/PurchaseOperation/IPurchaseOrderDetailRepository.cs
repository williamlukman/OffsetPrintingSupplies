using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IPurchaseOrderDetailRepository : IRepository<PurchaseOrderDetail>
    {
        IList<PurchaseOrderDetail> GetObjectsByPurchaseOrderId(int purchaseOrderId);
        PurchaseOrderDetail GetObjectById(int Id);
        PurchaseOrderDetail CreateObject(PurchaseOrderDetail purchaseOrderDetail);
        PurchaseOrderDetail UpdateObject(PurchaseOrderDetail purchaseOrderDetail);
        PurchaseOrderDetail SoftDeleteObject(PurchaseOrderDetail purchaseOrderDetail);
        bool DeleteObject(int Id);
        PurchaseOrderDetail FinishObject(PurchaseOrderDetail purchaseOrderDetail);
        PurchaseOrderDetail UnfinishObject(PurchaseOrderDetail purchaseOrderDetail);
        string SetObjectCode(string ParentCode);
    }
}