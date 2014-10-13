using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IPurchaseAllowanceDetailRepository : IRepository<PurchaseAllowanceDetail>
    {
        IQueryable<PurchaseAllowanceDetail> GetQueryable();
        IList<PurchaseAllowanceDetail> GetAll();
        IList<PurchaseAllowanceDetail> GetAllByMonthCreated();
        IList<PurchaseAllowanceDetail> GetObjectsByPurchaseAllowanceId(int purchaseAllowanceId);
        IList<PurchaseAllowanceDetail> GetObjectsByPayableId(int payableId);
        PurchaseAllowanceDetail GetObjectById(int Id);
        PurchaseAllowanceDetail CreateObject(PurchaseAllowanceDetail purchaseAllowanceDetail);
        PurchaseAllowanceDetail UpdateObject(PurchaseAllowanceDetail purchaseAllowanceDetail);
        PurchaseAllowanceDetail SoftDeleteObject(PurchaseAllowanceDetail purchaseAllowanceDetail);
        bool DeleteObject(int Id);
        PurchaseAllowanceDetail ConfirmObject(PurchaseAllowanceDetail purchaseAllowanceDetail);
        PurchaseAllowanceDetail UnconfirmObject(PurchaseAllowanceDetail purchaseAllowanceDetail);
        string SetObjectCode(string ParentCode);
    }
}