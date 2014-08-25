using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;

namespace Core.Interface.Repository
{
    public interface IRetailSalesInvoiceRepository : IRepository<RetailSalesInvoice>
    {
        IQueryable<RetailSalesInvoice> GetQueryable();
        IList<RetailSalesInvoice> GetAll();
        RetailSalesInvoice GetObjectById(int Id);
        RetailSalesInvoice ConfirmObject(RetailSalesInvoice retailSalesInvoice);
        RetailSalesInvoice UnconfirmObject(RetailSalesInvoice retailSalesInvoice);
        RetailSalesInvoice PaidObject(RetailSalesInvoice retailSalesInvoice);
        RetailSalesInvoice UnpaidObject(RetailSalesInvoice retailSalesInvoice);
        RetailSalesInvoice CreateObject(RetailSalesInvoice retailSalesInvoice);
        RetailSalesInvoice UpdateObject(RetailSalesInvoice retailSalesInvoice);
        RetailSalesInvoice SoftDeleteObject(RetailSalesInvoice retailSalesInvoice);
        bool DeleteObject(int Id);
        string SetObjectCode();
    }
}
