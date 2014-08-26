using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IPurchaseReceivalRepository : IRepository<PurchaseReceival>
    {
        IQueryable<PurchaseReceival> GetQueryable();
        IList<PurchaseReceival> GetAll();
        IList<PurchaseReceival> GetAllByMonthCreated();
        IList<PurchaseReceival> GetConfirmedObjects();
        PurchaseReceival GetObjectById(int Id);
        IList<PurchaseReceival> GetObjectsByPurchaseOrderId(int purchaseOrderId);
        PurchaseReceival CreateObject(PurchaseReceival purchaseReceival);
        PurchaseReceival UpdateObject(PurchaseReceival purchaseReceival);
        PurchaseReceival SoftDeleteObject(PurchaseReceival purchaseReceival);
        bool DeleteObject(int Id);
        PurchaseReceival ConfirmObject(PurchaseReceival purchaseReceival);
        PurchaseReceival UnconfirmObject(PurchaseReceival purchaseReceival);
        PurchaseReceival SetInvoiceComplete(PurchaseReceival purchaseReceival);
        PurchaseReceival UnsetInvoiceComplete(PurchaseReceival purchaseReceival);
        string SetObjectCode();
    }
}