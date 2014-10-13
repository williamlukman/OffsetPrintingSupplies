using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IPurchaseDownPaymentDetailValidator
    {
        PurchaseDownPaymentDetail VHasPurchaseDownPayment(PurchaseDownPaymentDetail purchaseDownPaymentDetail, IPurchaseDownPaymentService _purchaseDownPaymentService);
        PurchaseDownPaymentDetail VHasPayable(PurchaseDownPaymentDetail purchaseDownPaymentDetail, IPayableService _payableService);
        PurchaseDownPaymentDetail VHasBeenConfirmed(PurchaseDownPaymentDetail purchaseDownPaymentDetail);
        PurchaseDownPaymentDetail VHasNotBeenConfirmed(PurchaseDownPaymentDetail purchaseDownPaymentDetail);
        PurchaseDownPaymentDetail VHasNotBeenDeleted(PurchaseDownPaymentDetail purchaseDownPaymentDetail);
        PurchaseDownPaymentDetail VPayableHasNotBeenCompleted(PurchaseDownPaymentDetail purchaseDownPaymentDetail, IPayableService _payableService);
        PurchaseDownPaymentDetail VNonNegativeAmount(PurchaseDownPaymentDetail purchaseDownPaymentDetail);
        PurchaseDownPaymentDetail VAmountLessOrEqualPayable(PurchaseDownPaymentDetail purchaseDownPaymentDetail, IPayableService _payableService);
        PurchaseDownPaymentDetail VUniquePayableId(PurchaseDownPaymentDetail purchaseDownPaymentDetail, IPurchaseDownPaymentDetailService _purchaseDownPaymentDetailService, IPayableService _payableService);
        PurchaseDownPaymentDetail VDetailsAmountLessOrEqualPurchaseDownPaymentTotal(PurchaseDownPaymentDetail purchaseDownPaymentDetail, IPurchaseDownPaymentService _purchaseDownPaymentService,
                                                                          IPurchaseDownPaymentDetailService _purchaseDownPaymentDetailService);
        PurchaseDownPaymentDetail VCreateObject(PurchaseDownPaymentDetail purchaseDownPaymentDetail, IPurchaseDownPaymentService _purchaseDownPaymentService, IPurchaseDownPaymentDetailService _purchaseDownPaymentDetailService, ICashBankService _cashBankService, IPayableService _payableService);
        PurchaseDownPaymentDetail VUpdateObject(PurchaseDownPaymentDetail purchaseDownPaymentDetail, IPurchaseDownPaymentService _purchaseDownPaymentService, IPurchaseDownPaymentDetailService _purchaseDownPaymentDetailService, ICashBankService _cashBankService, IPayableService _payableService);
        PurchaseDownPaymentDetail VDeleteObject(PurchaseDownPaymentDetail purchaseDownPaymentDetail);
        PurchaseDownPaymentDetail VHasConfirmationDate(PurchaseDownPaymentDetail purchaseDownPaymentDetail);
        PurchaseDownPaymentDetail VConfirmObject(PurchaseDownPaymentDetail purchaseDownPaymentDetail, IPayableService _payableService);
        PurchaseDownPaymentDetail VUnconfirmObject(PurchaseDownPaymentDetail purchaseDownPaymentDetail);
        bool ValidCreateObject(PurchaseDownPaymentDetail purchaseDownPaymentDetail, IPurchaseDownPaymentService _purchaseDownPaymentService, IPurchaseDownPaymentDetailService _purchaseDownPaymentDetailService, ICashBankService _cashBankService, IPayableService _payableService);
        bool ValidUpdateObject(PurchaseDownPaymentDetail purchaseDownPaymentDetail, IPurchaseDownPaymentService _purchaseDownPaymentService, IPurchaseDownPaymentDetailService _purchaseDownPaymentDetailService, ICashBankService _cashBankService, IPayableService _payableService);
        bool ValidDeleteObject(PurchaseDownPaymentDetail purchaseDownPaymentDetail);
        bool ValidConfirmObject(PurchaseDownPaymentDetail purchaseDownPaymentDetail, IPayableService _payableService);
        bool ValidUnconfirmObject(PurchaseDownPaymentDetail purchaseDownPaymentDetail);
        bool isValid(PurchaseDownPaymentDetail purchaseDownPaymentDetail);
        string PrintError(PurchaseDownPaymentDetail purchaseDownPaymentDetail);
    }
}
