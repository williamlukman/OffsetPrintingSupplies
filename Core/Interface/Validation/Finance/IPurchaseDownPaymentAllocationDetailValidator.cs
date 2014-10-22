using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IPurchaseDownPaymentAllocationDetailValidator
    {
        PurchaseDownPaymentAllocationDetail VHasPurchaseDownPaymentAllocation(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail,
                                            IPurchaseDownPaymentAllocationService _purchaseDownPaymentAllocationService);
        PurchaseDownPaymentAllocationDetail VHasPayable(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail, IPayableService _payableService);
        PurchaseDownPaymentAllocationDetail VHasBeenConfirmed(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail);
        PurchaseDownPaymentAllocationDetail VHasNotBeenConfirmed(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail);
        PurchaseDownPaymentAllocationDetail VHasNotBeenDeleted(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail);
        PurchaseDownPaymentAllocationDetail VPayableHasNotBeenCompleted(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail, IPayableService _payableService);
        PurchaseDownPaymentAllocationDetail VNonNegativeAmount(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail);
        PurchaseDownPaymentAllocationDetail VAmountLessOrEqualPayable(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail, IPayableService _payableService);
        PurchaseDownPaymentAllocationDetail VUniquePayableId(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail,
                                            IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService, IPayableService _payableService);
        PurchaseDownPaymentAllocationDetail VDetailsAmountLessOrEqualPurchaseDownPaymentTotal(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail,
                                            IPurchaseDownPaymentAllocationService _purchaseDownPaymentAllocationService,
                                            IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService);
        PurchaseDownPaymentAllocationDetail VCreateObject(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail, IPurchaseDownPaymentAllocationService _purchaseDownPaymentAllocationService,
                                                          IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService, IPurchaseDownPaymentService _purchaseDownPaymentService,
                                                          IPaymentVoucherDetailService _paymentVoucherDetailService, IPayableService _payableService);
        PurchaseDownPaymentAllocationDetail VUpdateObject(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail, IPurchaseDownPaymentAllocationService _purchaseDownPaymentAllocationService,
                                                          IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService, IPurchaseDownPaymentService _purchaseDownPaymentService,
                                                          IPaymentVoucherDetailService _paymentVoucherDetailService, IPayableService _payableService);
        PurchaseDownPaymentAllocationDetail VDeleteObject(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail);
        PurchaseDownPaymentAllocationDetail VHasConfirmationDate(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail);
        PurchaseDownPaymentAllocationDetail VConfirmObject(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail,
                                                           IPaymentVoucherDetailService _paymentVoucherDetailService, IPayableService _payableService);
        PurchaseDownPaymentAllocationDetail VUnconfirmObject(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail,
                                                             IPaymentVoucherDetailService _paymentVoucherDetailService, IPayableService _payableService);
        bool ValidCreateObject(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail, IPurchaseDownPaymentAllocationService _purchaseDownPaymentAllocationService,
                               IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService, IPurchaseDownPaymentService _purchaseDownPaymentService,
                               IPaymentVoucherDetailService _paymentVoucherDetailService, IPayableService _payableService);
        bool ValidUpdateObject(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail, IPurchaseDownPaymentAllocationService _purchaseDownPaymentAllocationService,
                               IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService, IPurchaseDownPaymentService _purchaseDownPaymentService,
                               IPaymentVoucherDetailService _paymentVoucherDetailService, IPayableService _payableService);
        bool ValidDeleteObject(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail);
        bool ValidConfirmObject(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail,
                                IPaymentVoucherDetailService _paymentVoucherDetailService, IPayableService _payableService);
        bool ValidUnconfirmObject(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail,
                                  IPaymentVoucherDetailService _paymentVoucherDetailService, IPayableService _payableService);
        bool isValid(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail);
        string PrintError(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail);
    }
}
