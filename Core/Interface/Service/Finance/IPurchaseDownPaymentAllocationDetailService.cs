using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IPurchaseDownPaymentAllocationDetailService
    {
        IPurchaseDownPaymentAllocationDetailValidator GetValidator();
        IQueryable<PurchaseDownPaymentAllocationDetail> GetQueryable();
        IList<PurchaseDownPaymentAllocationDetail> GetAll();
        IList<PurchaseDownPaymentAllocationDetail> GetObjectsByPurchaseDownPaymentAllocationId(int purchaseDownPaymentAllocationId);
        IList<PurchaseDownPaymentAllocationDetail> GetObjectsByPayableId(int payableId);
        PurchaseDownPaymentAllocationDetail GetObjectByPaymentVoucherDetailId(int paymentVoucherDetailId);
        PurchaseDownPaymentAllocationDetail GetObjectById(int Id);
        PurchaseDownPaymentAllocationDetail CreateObject(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail, IPurchaseDownPaymentAllocationService _purchaseDownPaymentAllocationService,
                                                         IPurchaseDownPaymentService _purchaseDownPaymentService, IPaymentVoucherDetailService _paymentVoucherDetailService, IPayableService _payableService,
                                                         IPaymentVoucherService _paymentVoucherService, ICashBankService _cashBankService);
        PurchaseDownPaymentAllocationDetail UpdateObject(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail, IPurchaseDownPaymentAllocationService _purchaseDownPaymentAllocationService,
                                                         IPurchaseDownPaymentService _purchaseDownPaymentService, IPaymentVoucherDetailService _paymentVoucherDetailService, IPayableService _payableService,
                                                         IPaymentVoucherService _paymentVoucherService, ICashBankService _cashBankService);
        PurchaseDownPaymentAllocationDetail SoftDeleteObject(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail,
                                                             IPaymentVoucherDetailService _paymentVoucherDetailService);
        bool DeleteObject(int Id);
        PurchaseDownPaymentAllocationDetail ConfirmObject(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail, DateTime ConfirmationDate,
                                                          IPurchaseDownPaymentAllocationService _purchaseDownPaymentAllocationService, IPurchaseDownPaymentService _purchaseDownPaymentService,
                                                          IPaymentVoucherDetailService _paymentVoucherDetailService, IPayableService _payableService);
        PurchaseDownPaymentAllocationDetail UnconfirmObject(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail,
                                                            IPurchaseDownPaymentAllocationService _purchaseDownPaymentAllocationService, IPurchaseDownPaymentService _purchaseDownPaymentService,
                                                            IPaymentVoucherDetailService _paymentVoucherDetailService, IPayableService _payableService);
    }
}