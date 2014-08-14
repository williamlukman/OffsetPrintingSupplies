using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IPurchaseOrderRepository : IRepository<PurchaseOrder>
    {
        IList<PurchaseOrder> GetAll();
        IList<PurchaseOrder> GetAllByMonthCreated();
        PurchaseOrder GetObjectById(int Id);
        IList<PurchaseOrder> GetObjectsByContactId(int contactId);
        IList<PurchaseOrder> GetConfirmedObjects();
        PurchaseOrder CreateObject(PurchaseOrder purchaseOrder);
        PurchaseOrder UpdateObject(PurchaseOrder purchaseOrder);
        PurchaseOrder SoftDeleteObject(PurchaseOrder purchaseOrder);
        bool DeleteObject(int Id);
        PurchaseOrder ConfirmObject(PurchaseOrder purchaseOrder);
        PurchaseOrder UnconfirmObject(PurchaseOrder purchaseOrder);
        PurchaseOrder SetReceivalComplete(PurchaseOrder purchaseOrder);
        PurchaseOrder UnsetReceivalComplete(PurchaseOrder purchaseOrder);
        string SetObjectCode();
    }
}