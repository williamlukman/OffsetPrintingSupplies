using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IPurchaseDownPaymentAllocationDetailRepository : IRepository<PurchaseDownPaymentAllocationDetail>
    {
        IQueryable<PurchaseDownPaymentAllocationDetail> GetQueryable();
        IList<PurchaseDownPaymentAllocationDetail> GetAll();
        IList<PurchaseDownPaymentAllocationDetail> GetAllByMonthCreated();
        IList<PurchaseDownPaymentAllocationDetail> GetObjectsByPurchaseDownPaymentAllocationId(int purchaseDownPaymentAllocationId);
        IList<PurchaseDownPaymentAllocationDetail> GetObjectsByPayableId(int payableId);
        PurchaseDownPaymentAllocationDetail GetObjectByPaymentVoucherDetailId(int paymentVoucherDetailId);
        PurchaseDownPaymentAllocationDetail GetObjectById(int Id);
        PurchaseDownPaymentAllocationDetail CreateObject(PurchaseDownPaymentAllocationDetail purchaseDownPaymentDetail);
        PurchaseDownPaymentAllocationDetail UpdateObject(PurchaseDownPaymentAllocationDetail purchaseDownPaymentDetail);
        PurchaseDownPaymentAllocationDetail SoftDeleteObject(PurchaseDownPaymentAllocationDetail purchaseDownPaymentDetail);
        bool DeleteObject(int Id);
        PurchaseDownPaymentAllocationDetail ConfirmObject(PurchaseDownPaymentAllocationDetail purchaseDownPaymentDetail);
        PurchaseDownPaymentAllocationDetail UnconfirmObject(PurchaseDownPaymentAllocationDetail purchaseDownPaymentDetail);
        string SetObjectCode(string ParentCode);
    }
}