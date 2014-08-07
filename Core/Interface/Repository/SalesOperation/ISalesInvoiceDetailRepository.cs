using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface ISalesInvoiceDetailRepository : IRepository<SalesInvoiceDetail>
    {
        IList<SalesInvoiceDetail> GetObjectsBySalesInvoiceId(int salesInvoiceId);
        IList<SalesInvoiceDetail> GetObjectsByDeliveryOrderDetailId(int deliveryOrderDetailId);
        SalesInvoiceDetail GetObjectById(int Id);
        SalesInvoiceDetail CreateObject(SalesInvoiceDetail salesInvoiceDetail);
        SalesInvoiceDetail UpdateObject(SalesInvoiceDetail salesInvoiceDetail);
        SalesInvoiceDetail SoftDeleteObject(SalesInvoiceDetail salesInvoiceDetail);
        bool DeleteObject(int Id);
        SalesInvoiceDetail ConfirmObject(SalesInvoiceDetail salesInvoiceDetail);
        SalesInvoiceDetail UnconfirmObject(SalesInvoiceDetail salesInvoiceDetail);
        string SetObjectCode(string ParentCode);
    }
}