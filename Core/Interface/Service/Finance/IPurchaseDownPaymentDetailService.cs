using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IPurchaseDownPaymentDetailService
    {
        IPurchaseDownPaymentDetailValidator GetValidator();
        IQueryable<PurchaseDownPaymentDetail> GetQueryable();
        IList<PurchaseDownPaymentDetail> GetAll();
        IList<PurchaseDownPaymentDetail> GetObjectsByPurchaseDownPaymentId(int purchaseDownPaymentId);
        IList<PurchaseDownPaymentDetail> GetObjectsByPayableId(int payableId);
        PurchaseDownPaymentDetail GetObjectById(int Id);        
        PurchaseDownPaymentDetail CreateObject(PurchaseDownPaymentDetail purchaseDownPaymentDetail, IPurchaseDownPaymentService _purchaseDownPaymentService,
                                          ICashBankService _cashBankService, IPayableService _payableService);
        PurchaseDownPaymentDetail UpdateObject(PurchaseDownPaymentDetail purchaseDownPaymentDetail, IPurchaseDownPaymentService _purchaseDownPaymentService,
                                          ICashBankService _cashBankService, IPayableService _payableService);
        PurchaseDownPaymentDetail SoftDeleteObject(PurchaseDownPaymentDetail purchaseDownPaymentDetail);
        bool DeleteObject(int Id);
        PurchaseDownPaymentDetail ConfirmObject(PurchaseDownPaymentDetail purchaseDownPaymentDetail, DateTime ConfirmationDate, IPurchaseDownPaymentService _purchaseDownPaymentService, IPayableService _payableService);
        PurchaseDownPaymentDetail UnconfirmObject(PurchaseDownPaymentDetail purchaseDownPaymentDetail, IPurchaseDownPaymentService _purchaseDownPaymentService, IPayableService _payableService);
    }
}