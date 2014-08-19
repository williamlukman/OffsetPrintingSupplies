using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;

namespace Core.Interface.Repository
{
    public interface ICashSalesReturnDetailRepository : IRepository<CashSalesReturnDetail>
    {
        IList<CashSalesReturnDetail> GetAll();
        IList<CashSalesReturnDetail> GetObjectsByCashSalesReturnId(int CashSalesReturnId);
        CashSalesReturnDetail GetObjectById(int Id);
        CashSalesReturnDetail GetObjectByCashSalesInvoiceDetailId(int CashSalesInvoiceDetailId);
        CashSalesReturnDetail ConfirmObject(CashSalesReturnDetail cashSalesReturnDetail);
        CashSalesReturnDetail UnconfirmObject(CashSalesReturnDetail cashSalesReturnDetail);
        CashSalesReturnDetail CreateObject(CashSalesReturnDetail cashSalesReturnDetail);
        CashSalesReturnDetail UpdateObject(CashSalesReturnDetail cashSalesReturnDetail);
        CashSalesReturnDetail SoftDeleteObject(CashSalesReturnDetail cashSalesReturnDetail);
        bool DeleteObject(int Id);
        string SetObjectCode(string ParentCode);
    }
}
