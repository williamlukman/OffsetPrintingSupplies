using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IPurchaseDownPaymentDetailRepository : IRepository<PurchaseDownPaymentDetail>
    {
        IQueryable<PurchaseDownPaymentDetail> GetQueryable();
        IList<PurchaseDownPaymentDetail> GetAll();
        IList<PurchaseDownPaymentDetail> GetAllByMonthCreated();
        IList<PurchaseDownPaymentDetail> GetObjectsByPurchaseDownPaymentId(int purchaseDownPaymentId);
        IList<PurchaseDownPaymentDetail> GetObjectsByPayableId(int payableId);
        PurchaseDownPaymentDetail GetObjectById(int Id);
        PurchaseDownPaymentDetail CreateObject(PurchaseDownPaymentDetail purchaseDownPaymentDetail);
        PurchaseDownPaymentDetail UpdateObject(PurchaseDownPaymentDetail purchaseDownPaymentDetail);
        PurchaseDownPaymentDetail SoftDeleteObject(PurchaseDownPaymentDetail purchaseDownPaymentDetail);
        bool DeleteObject(int Id);
        PurchaseDownPaymentDetail ConfirmObject(PurchaseDownPaymentDetail purchaseDownPaymentDetail);
        PurchaseDownPaymentDetail UnconfirmObject(PurchaseDownPaymentDetail purchaseDownPaymentDetail);
        string SetObjectCode(string ParentCode);
    }
}