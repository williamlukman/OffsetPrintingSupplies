using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IPurchaseAllowanceDetailValidator
    {
        PurchaseAllowanceDetail VHasPurchaseAllowance(PurchaseAllowanceDetail purchaseAllowanceDetail, IPurchaseAllowanceService _purchaseAllowanceService);
        PurchaseAllowanceDetail VHasPayable(PurchaseAllowanceDetail purchaseAllowanceDetail, IPayableService _payableService);
        PurchaseAllowanceDetail VHasBeenConfirmed(PurchaseAllowanceDetail purchaseAllowanceDetail);
        PurchaseAllowanceDetail VHasNotBeenConfirmed(PurchaseAllowanceDetail purchaseAllowanceDetail);
        PurchaseAllowanceDetail VHasNotBeenDeleted(PurchaseAllowanceDetail purchaseAllowanceDetail);
        PurchaseAllowanceDetail VPayableHasNotBeenCompleted(PurchaseAllowanceDetail purchaseAllowanceDetail, IPayableService _payableService);
        PurchaseAllowanceDetail VNonNegativeAmount(PurchaseAllowanceDetail purchaseAllowanceDetail);
        PurchaseAllowanceDetail VAmountLessOrEqualPayable(PurchaseAllowanceDetail purchaseAllowanceDetail, IPayableService _payableService);
        PurchaseAllowanceDetail VUniquePayableId(PurchaseAllowanceDetail purchaseAllowanceDetail, IPurchaseAllowanceDetailService _purchaseAllowanceDetailService, IPayableService _payableService);
        PurchaseAllowanceDetail VDetailsAmountLessOrEqualPurchaseAllowanceTotal(PurchaseAllowanceDetail purchaseAllowanceDetail, IPurchaseAllowanceService _purchaseAllowanceService,
                                                                          IPurchaseAllowanceDetailService _purchaseAllowanceDetailService);
        PurchaseAllowanceDetail VCreateObject(PurchaseAllowanceDetail purchaseAllowanceDetail, IPurchaseAllowanceService _purchaseAllowanceService, IPurchaseAllowanceDetailService _purchaseAllowanceDetailService, ICashBankService _cashBankService, IPayableService _payableService);
        PurchaseAllowanceDetail VUpdateObject(PurchaseAllowanceDetail purchaseAllowanceDetail, IPurchaseAllowanceService _purchaseAllowanceService, IPurchaseAllowanceDetailService _purchaseAllowanceDetailService, ICashBankService _cashBankService, IPayableService _payableService);
        PurchaseAllowanceDetail VDeleteObject(PurchaseAllowanceDetail purchaseAllowanceDetail);
        PurchaseAllowanceDetail VHasConfirmationDate(PurchaseAllowanceDetail purchaseAllowanceDetail);
        PurchaseAllowanceDetail VConfirmObject(PurchaseAllowanceDetail purchaseAllowanceDetail, IPayableService _payableService);
        PurchaseAllowanceDetail VUnconfirmObject(PurchaseAllowanceDetail purchaseAllowanceDetail);
        bool ValidCreateObject(PurchaseAllowanceDetail purchaseAllowanceDetail, IPurchaseAllowanceService _purchaseAllowanceService, IPurchaseAllowanceDetailService _purchaseAllowanceDetailService, ICashBankService _cashBankService, IPayableService _payableService);
        bool ValidUpdateObject(PurchaseAllowanceDetail purchaseAllowanceDetail, IPurchaseAllowanceService _purchaseAllowanceService, IPurchaseAllowanceDetailService _purchaseAllowanceDetailService, ICashBankService _cashBankService, IPayableService _payableService);
        bool ValidDeleteObject(PurchaseAllowanceDetail purchaseAllowanceDetail);
        bool ValidConfirmObject(PurchaseAllowanceDetail purchaseAllowanceDetail, IPayableService _payableService);
        bool ValidUnconfirmObject(PurchaseAllowanceDetail purchaseAllowanceDetail);
        bool isValid(PurchaseAllowanceDetail purchaseAllowanceDetail);
        string PrintError(PurchaseAllowanceDetail purchaseAllowanceDetail);
    }
}
