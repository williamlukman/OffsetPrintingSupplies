using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IPurchaseAllowanceValidator
    {
        PurchaseAllowance VHasContact(PurchaseAllowance purchaseAllowance, IContactService _contactService);
        PurchaseAllowance VHasCashBank(PurchaseAllowance purchaseAllowance, ICashBankService _cashBankService);
        PurchaseAllowance VHasPaymentDate(PurchaseAllowance purchaseAllowance);
        PurchaseAllowance VNotIsGBCH(PurchaseAllowance purchaseAllowance);
        PurchaseAllowance VIfGBCHThenIsBank(PurchaseAllowance purchaseAllowance, ICashBankService _cashBankService);
        PurchaseAllowance VIfGBCHThenHasDueDate(PurchaseAllowance purchaseAllowance);
        PurchaseAllowance VHasNoPurchaseAllowanceDetail(PurchaseAllowance purchaseAllowance, IPurchaseAllowanceDetailService _purchaseAllowanceDetailService);
        PurchaseAllowance VHasPurchaseAllowanceDetails(PurchaseAllowance purchaseAllowance, IPurchaseAllowanceDetailService _purchaseAllowanceDetailService);
        PurchaseAllowance VTotalAmountIsNotZero(PurchaseAllowance purchaseAllowance, IPurchaseAllowanceDetailService _purchaseAllowanceDetailService);
        PurchaseAllowance VHasNotBeenDeleted(PurchaseAllowance purchaseAllowance);
        PurchaseAllowance VHasBeenConfirmed(PurchaseAllowance purchaseAllowance);
        PurchaseAllowance VHasNotBeenConfirmed(PurchaseAllowance purchaseAllowance);
        PurchaseAllowance VTotalAmountEqualDetailsAmount(PurchaseAllowance purchaseAllowance, IPurchaseAllowanceDetailService _purchaseAllowanceDetailService);
        PurchaseAllowance VAllPurchaseAllowanceDetailsAreConfirmable(PurchaseAllowance purchaseAllowance, IPurchaseAllowanceService _paymetnVoucherService,
                                                               IPurchaseAllowanceDetailService purchaseAllowanceDetailService, ICashBankService _cashBankService,
                                                               IPayableService _payableService);
        PurchaseAllowance VCashBankIsGreaterThanOrEqualPurchaseAllowanceDetails(PurchaseAllowance purchaseAllowance, IPurchaseAllowanceDetailService _purchaseAllowanceDetailService,
                                                                   ICashBankService _cashBankService, bool CasePayment);
        PurchaseAllowance VHasBeenReconciled(PurchaseAllowance purchaseAllowance);
        PurchaseAllowance VHasNotBeenReconciled(PurchaseAllowance purchaseAllowance);
        PurchaseAllowance VHasReconciliationDate(PurchaseAllowance purchaseAllowance);
        PurchaseAllowance VGeneralLedgerPostingHasNotBeenClosed(PurchaseAllowance purchaseAllowance, IClosingService _closingService, int CaseConfirmUnconfirm);
        PurchaseAllowance VCreateObject(PurchaseAllowance purchaseAllowance, IPurchaseAllowanceService _purchaseAllowanceService, IPurchaseAllowanceDetailService _purchaseAllowanceDetailService,
                                     IPayableService _payableService, IContactService _contactService, ICashBankService _cashBankService);
        PurchaseAllowance VUpdateObject(PurchaseAllowance purchaseAllowance, IPurchaseAllowanceService _purchaseAllowanceService, IPurchaseAllowanceDetailService _purchaseAllowanceDetailService,
                                     IPayableService _payableService, IContactService _contactService, ICashBankService _cashBankService);
        PurchaseAllowance VDeleteObject(PurchaseAllowance purchaseAllowance, IPurchaseAllowanceDetailService _purchaseAllowanceDetailService);
        PurchaseAllowance VHasConfirmationDate(PurchaseAllowance purchaseAllowance);
        PurchaseAllowance VConfirmObject(PurchaseAllowance purchaseAllowance, IPurchaseAllowanceService _purchaseAllowanceService,
                                       IPurchaseAllowanceDetailService _purchaseAllowanceDetailService, ICashBankService _cashBankService,
                                       IPayableService _payableService, IClosingService _closingService);
        PurchaseAllowance VUnconfirmObject(PurchaseAllowance purchaseAllowance, IClosingService _closingService);
        PurchaseAllowance VReconcileObject(PurchaseAllowance purchaseAllowance, IPurchaseAllowanceDetailService _purchaseAllowanceDetailService, ICashBankService _cashBankService, IClosingService _closingService);
        PurchaseAllowance VUnreconcileObject(PurchaseAllowance purchaseAllowance, IClosingService _closingService);
        bool ValidCreateObject(PurchaseAllowance purchaseAllowance, IPurchaseAllowanceService _purchaseAllowanceService, IPurchaseAllowanceDetailService _purchaseAllowanceDetailService,
                               IPayableService _payableService, IContactService _contactService, ICashBankService _cashBankService);
        bool ValidUpdateObject(PurchaseAllowance purchaseAllowance, IPurchaseAllowanceService _purchaseAllowanceService, IPurchaseAllowanceDetailService _purchaseAllowanceDetailService,
                               IPayableService _payableService, IContactService _contactService, ICashBankService _cashBankService);
        bool ValidDeleteObject(PurchaseAllowance purchaseAllowance, IPurchaseAllowanceDetailService _purchaseAllowanceDetailService);
        bool ValidConfirmObject(PurchaseAllowance purchaseAllowance, IPurchaseAllowanceService _purchaseAllowanceService,
                                IPurchaseAllowanceDetailService _purchaseAllowanceDetailService, ICashBankService _cashBankService,
                                IPayableService _payableService, IClosingService _closingService);
        bool ValidUnconfirmObject(PurchaseAllowance purchaseAllowance, IClosingService _closingService);
        bool ValidReconcileObject(PurchaseAllowance purchaseAllowance, IPurchaseAllowanceDetailService _purchaseAllowanceDetailService, ICashBankService _cashBankService, IClosingService _closingService);
        bool ValidUnreconcileObject(PurchaseAllowance purchaseAllowance, IClosingService _closingService);
        bool isValid(PurchaseAllowance purchaseAllowance);
        string PrintError(PurchaseAllowance purchaseAllowance);
    }
}
