using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IPurchaseDownPaymentRepository : IRepository<PurchaseDownPayment>
    {
        IQueryable<PurchaseDownPayment> GetQueryable();
        IList<PurchaseDownPayment> GetAll();
        IList<PurchaseDownPayment> GetAllByMonthCreated();
        PurchaseDownPayment GetObjectById(int Id);
        IList<PurchaseDownPayment> GetObjectsByContactId(int contactId);
        PurchaseDownPayment CreateObject(PurchaseDownPayment purchaseDownPayment);
        PurchaseDownPayment UpdateObject(PurchaseDownPayment purchaseDownPayment);
        PurchaseDownPayment SoftDeleteObject(PurchaseDownPayment purchaseDownPayment);
        bool DeleteObject(int Id);
        PurchaseDownPayment ConfirmObject(PurchaseDownPayment purchaseDownPayment);
        PurchaseDownPayment UnconfirmObject(PurchaseDownPayment purchaseDownPayment);
        string SetObjectCode();
    }
}