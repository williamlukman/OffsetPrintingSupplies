using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;

namespace Core.Interface.Repository
{
    public interface ICashSalesReturnRepository : IRepository<CashSalesReturn>
    {
        IQueryable<CashSalesReturn> GetQueryable();
        IList<CashSalesReturn> GetAll();
        IList<CashSalesReturn> GetObjectsByCashSalesInvoiceId(int CashSalesInvoiceId);
        CashSalesReturn GetObjectById(int Id);
        CashSalesReturn GetObjectByCashSalesInvoiceId(int CashSalesInvoiceId);
        CashSalesReturn ConfirmObject(CashSalesReturn cashSalesReturn);
        CashSalesReturn UnconfirmObject(CashSalesReturn cashSalesReturn);
        CashSalesReturn PaidObject(CashSalesReturn cashSalesReturn);
        CashSalesReturn UnpaidObject(CashSalesReturn cashSalesReturn);
        CashSalesReturn CreateObject(CashSalesReturn cashSalesReturn);
        CashSalesReturn UpdateObject(CashSalesReturn cashSalesReturn);
        CashSalesReturn SoftDeleteObject(CashSalesReturn cashSalesReturn);
        bool DeleteObject(int Id);
        string SetObjectCode();
    }
}
