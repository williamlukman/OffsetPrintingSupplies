using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface ISalesInvoiceRepository : IRepository<SalesInvoice>
    {
        IQueryable<SalesInvoice> GetQueryable();
        IList<SalesInvoice> GetAll();
        IList<SalesInvoice> GetAllByMonthCreated();
        SalesInvoice GetObjectById(int Id);
        IList<SalesInvoice> GetObjectsByDeliveryOrderId(int deliveryOrderId);
        SalesInvoice CreateObject(SalesInvoice salesInvoice);
        SalesInvoice UpdateObject(SalesInvoice salesInvoice);
        SalesInvoice SoftDeleteObject(SalesInvoice salesInvoice);
        bool DeleteObject(int Id);
        SalesInvoice ConfirmObject(SalesInvoice salesInvoice);
        SalesInvoice UnconfirmObject(SalesInvoice salesInvoice);
        string SetObjectCode();
    }
}