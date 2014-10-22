using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IPurchaseDownPaymentAllocationRepository : IRepository<PurchaseDownPaymentAllocation>
    {
        IQueryable<PurchaseDownPaymentAllocation> GetQueryable();
        IList<PurchaseDownPaymentAllocation> GetAll();
        IList<PurchaseDownPaymentAllocation> GetAllByMonthCreated();
        PurchaseDownPaymentAllocation GetObjectById(int Id);
        PurchaseDownPaymentAllocation GetObjectByPurchaseDownPaymentId(int purchaseDownPaymentId);
        IList<PurchaseDownPaymentAllocation> GetObjectsByContactId(int contactId);
        PurchaseDownPaymentAllocation CreateObject(PurchaseDownPaymentAllocation purchaseDownPayment);
        PurchaseDownPaymentAllocation UpdateObject(PurchaseDownPaymentAllocation purchaseDownPayment);
        PurchaseDownPaymentAllocation SoftDeleteObject(PurchaseDownPaymentAllocation purchaseDownPayment);
        bool DeleteObject(int Id);
        PurchaseDownPaymentAllocation ConfirmObject(PurchaseDownPaymentAllocation purchaseDownPayment);
        PurchaseDownPaymentAllocation UnconfirmObject(PurchaseDownPaymentAllocation purchaseDownPayment);
        string SetObjectCode();
    }
}