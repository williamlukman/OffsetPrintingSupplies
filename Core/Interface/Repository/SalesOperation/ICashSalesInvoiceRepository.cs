using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;

namespace Core.Interface.Repository
{
    public interface ICashSalesInvoiceRepository : IRepository<CashSalesInvoice>
    {
        IList<CashSalesInvoice> GetAll();
        CashSalesInvoice GetObjectById(int Id);
        CashSalesInvoice ConfirmObject(CashSalesInvoice cashSalesInvoice);
        CashSalesInvoice UnconfirmObject(CashSalesInvoice cashSalesInvoice);
        CashSalesInvoice PaidObject(CashSalesInvoice cashSalesInvoice);
        CashSalesInvoice UnpaidObject(CashSalesInvoice cashSalesInvoice);
        CashSalesInvoice CreateObject(CashSalesInvoice cashSalesInvoice);
        CashSalesInvoice UpdateObject(CashSalesInvoice cashSalesInvoice);
        CashSalesInvoice SoftDeleteObject(CashSalesInvoice cashSalesInvoice);
        bool DeleteObject(int Id);
        string SetObjectCode();
    }
}
