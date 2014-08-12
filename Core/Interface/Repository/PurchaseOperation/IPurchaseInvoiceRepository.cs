using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IPurchaseInvoiceRepository : IRepository<PurchaseInvoice>
    {
        IList<PurchaseInvoice> GetAll();
        IList<PurchaseInvoice> GetAllByMonthCreated();
        PurchaseInvoice GetObjectById(int Id);
        IList<PurchaseInvoice> GetObjectsByPurchaseReceivalId(int purchaseReceivalId);
        PurchaseInvoice CreateObject(PurchaseInvoice purchaseInvoice);
        PurchaseInvoice UpdateObject(PurchaseInvoice purchaseInvoice);
        PurchaseInvoice SoftDeleteObject(PurchaseInvoice purchaseInvoice);
        bool DeleteObject(int Id);
        PurchaseInvoice ConfirmObject(PurchaseInvoice purchaseInvoice);
        PurchaseInvoice UnconfirmObject(PurchaseInvoice purchaseInvoice);
        string SetObjectCode();
    }
}