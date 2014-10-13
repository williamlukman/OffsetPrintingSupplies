using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IPurchaseAllowanceRepository : IRepository<PurchaseAllowance>
    {
        IQueryable<PurchaseAllowance> GetQueryable();
        IList<PurchaseAllowance> GetAll();
        IList<PurchaseAllowance> GetAllByMonthCreated();
        PurchaseAllowance GetObjectById(int Id);
        IList<PurchaseAllowance> GetObjectsByCashBankId(int cashBankId);
        IList<PurchaseAllowance> GetObjectsByContactId(int contactId);
        PurchaseAllowance CreateObject(PurchaseAllowance purchaseAllowance);
        PurchaseAllowance UpdateObject(PurchaseAllowance purchaseAllowance);
        PurchaseAllowance SoftDeleteObject(PurchaseAllowance purchaseAllowance);
        bool DeleteObject(int Id);
        PurchaseAllowance ConfirmObject(PurchaseAllowance purchaseAllowance);
        PurchaseAllowance UnconfirmObject(PurchaseAllowance purchaseAllowance);
        PurchaseAllowance ReconcileObject(PurchaseAllowance purchaseAllowance);
        PurchaseAllowance UnreconcileObject(PurchaseAllowance purchaseAllowance);
        string SetObjectCode();
    }
}