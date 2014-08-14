using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;

namespace Core.Interface.Repository
{
    public interface IRetailPurchaseInvoiceRepository : IRepository<RetailPurchaseInvoice>
    {
        IList<RetailPurchaseInvoice> GetAll();
        RetailPurchaseInvoice GetObjectById(int Id);
        RetailPurchaseInvoice ConfirmObject(RetailPurchaseInvoice retailPurchaseInvoice);
        RetailPurchaseInvoice UnconfirmObject(RetailPurchaseInvoice retailPurchaseInvoice);
        RetailPurchaseInvoice PaidObject(RetailPurchaseInvoice retailPurchaseInvoice);
        RetailPurchaseInvoice UnpaidObject(RetailPurchaseInvoice retailPurchaseInvoice);
        RetailPurchaseInvoice CreateObject(RetailPurchaseInvoice retailPurchaseInvoice);
        RetailPurchaseInvoice UpdateObject(RetailPurchaseInvoice retailPurchaseInvoice);
        RetailPurchaseInvoice SoftDeleteObject(RetailPurchaseInvoice retailPurchaseInvoice);
        string SetObjectCode();
        bool DeleteObject(int Id);
    }
}
