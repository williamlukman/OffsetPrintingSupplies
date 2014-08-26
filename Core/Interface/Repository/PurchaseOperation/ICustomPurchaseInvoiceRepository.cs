using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;

namespace Core.Interface.Repository
{
    public interface ICustomPurchaseInvoiceRepository : IRepository<CustomPurchaseInvoice>
    {
        IQueryable<CustomPurchaseInvoice> GetQueryable();
        IList<CustomPurchaseInvoice> GetAll();
        CustomPurchaseInvoice GetObjectById(int Id);
        CustomPurchaseInvoice ConfirmObject(CustomPurchaseInvoice customPurchaseInvoice);
        CustomPurchaseInvoice UnconfirmObject(CustomPurchaseInvoice customPurchaseInvoice);
        CustomPurchaseInvoice PaidObject(CustomPurchaseInvoice customPurchaseInvoice);
        CustomPurchaseInvoice UnpaidObject(CustomPurchaseInvoice customPurchaseInvoice);
        CustomPurchaseInvoice CreateObject(CustomPurchaseInvoice customPurchaseInvoice);
        CustomPurchaseInvoice UpdateObject(CustomPurchaseInvoice customPurchaseInvoice);
        CustomPurchaseInvoice SoftDeleteObject(CustomPurchaseInvoice customPurchaseInvoice);
        string SetObjectCode();
        bool DeleteObject(int Id);
    }
}
