using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;

namespace Core.Interface.Repository
{
    public interface ICustomPurchaseInvoiceDetailRepository : IRepository<CustomPurchaseInvoiceDetail>
    {
        IList<CustomPurchaseInvoiceDetail> GetAll();
        IList<CustomPurchaseInvoiceDetail> GetObjectsByCustomPurchaseInvoiceId(int CustomPurchaseInvoiceId);
        CustomPurchaseInvoiceDetail GetObjectById(int Id);
        CustomPurchaseInvoiceDetail ConfirmObject(CustomPurchaseInvoiceDetail customPurchaseInvoiceDetail);
        CustomPurchaseInvoiceDetail UnconfirmObject(CustomPurchaseInvoiceDetail customPurchaseInvoiceDetail);
        CustomPurchaseInvoiceDetail CreateObject(CustomPurchaseInvoiceDetail customPurchaseInvoiceDetail);
        CustomPurchaseInvoiceDetail UpdateObject(CustomPurchaseInvoiceDetail customPurchaseInvoiceDetail);
        CustomPurchaseInvoiceDetail SoftDeleteObject(CustomPurchaseInvoiceDetail customPurchaseInvoiceDetail);
        string SetObjectCode(string ParentCode);
        bool DeleteObject(int Id);
    }
}
