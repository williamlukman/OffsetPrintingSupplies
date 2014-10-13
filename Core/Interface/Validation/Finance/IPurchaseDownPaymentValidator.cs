using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IPurchaseDownPaymentValidator
    {
        PurchaseDownPayment VHasContact(PurchaseDownPayment purchaseDownPayment, IContactService _contactService);
        PurchaseDownPayment VHasCashBank(PurchaseDownPayment purchaseDownPayment, ICashBankService _cashBankService);
        PurchaseDownPayment VHasPaymentDate(PurchaseDownPayment purchaseDownPayment);
        PurchaseDownPayment VNotIsGBCH(PurchaseDownPayment purchaseDownPayment);
        PurchaseDownPayment VIfGBCHThenIsBank(PurchaseDownPayment purchaseDownPayment, ICashBankService _cashBankService);
        PurchaseDownPayment VIfGBCHThenHasDueDate(PurchaseDownPayment purchaseDownPayment);
        PurchaseDownPayment VHasNoPurchaseDownPaymentDetail(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentDetailService _purchaseDownPaymentDetailService);
        PurchaseDownPayment VHasPurchaseDownPaymentDetails(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentDetailService _purchaseDownPaymentDetailService);
        PurchaseDownPayment VTotalAmountIsNotZero(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentDetailService _purchaseDownPaymentDetailService);
        PurchaseDownPayment VHasNotBeenDeleted(PurchaseDownPayment purchaseDownPayment);
        PurchaseDownPayment VHasBeenConfirmed(PurchaseDownPayment purchaseDownPayment);
        PurchaseDownPayment VHasNotBeenConfirmed(PurchaseDownPayment purchaseDownPayment);
        PurchaseDownPayment VTotalAmountEqualDetailsAmount(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentDetailService _purchaseDownPaymentDetailService);
        PurchaseDownPayment VAllPurchaseDownPaymentDetailsAreConfirmable(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentService _paymetnVoucherService,
                                                               IPurchaseDownPaymentDetailService purchaseDownPaymentDetailService, ICashBankService _cashBankService,
                                                               IPayableService _payableService);
        PurchaseDownPayment VCashBankIsGreaterThanOrEqualPurchaseDownPaymentDetails(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentDetailService _purchaseDownPaymentDetailService,
                                                                   ICashBankService _cashBankService, bool CasePayment);
        PurchaseDownPayment VHasBeenReconciled(PurchaseDownPayment purchaseDownPayment);
        PurchaseDownPayment VHasNotBeenReconciled(PurchaseDownPayment purchaseDownPayment);
        PurchaseDownPayment VHasReconciliationDate(PurchaseDownPayment purchaseDownPayment);
        PurchaseDownPayment VGeneralLedgerPostingHasNotBeenClosed(PurchaseDownPayment purchaseDownPayment, IClosingService _closingService, int CaseConfirmUnconfirm);
        PurchaseDownPayment VCreateObject(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentService _purchaseDownPaymentService, IPurchaseDownPaymentDetailService _purchaseDownPaymentDetailService,
                                     IPayableService _payableService, IContactService _contactService, ICashBankService _cashBankService);
        PurchaseDownPayment VUpdateObject(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentService _purchaseDownPaymentService, IPurchaseDownPaymentDetailService _purchaseDownPaymentDetailService,
                                     IPayableService _payableService, IContactService _contactService, ICashBankService _cashBankService);
        PurchaseDownPayment VDeleteObject(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentDetailService _purchaseDownPaymentDetailService);
        PurchaseDownPayment VHasConfirmationDate(PurchaseDownPayment purchaseDownPayment);
        PurchaseDownPayment VConfirmObject(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentService _purchaseDownPaymentService,
                                       IPurchaseDownPaymentDetailService _purchaseDownPaymentDetailService, ICashBankService _cashBankService,
                                       IPayableService _payableService, IClosingService _closingService);
        PurchaseDownPayment VUnconfirmObject(PurchaseDownPayment purchaseDownPayment, IClosingService _closingService);
        PurchaseDownPayment VReconcileObject(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentDetailService _purchaseDownPaymentDetailService, ICashBankService _cashBankService, IClosingService _closingService);
        PurchaseDownPayment VUnreconcileObject(PurchaseDownPayment purchaseDownPayment, IClosingService _closingService);
        bool ValidCreateObject(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentService _purchaseDownPaymentService, IPurchaseDownPaymentDetailService _purchaseDownPaymentDetailService,
                               IPayableService _payableService, IContactService _contactService, ICashBankService _cashBankService);
        bool ValidUpdateObject(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentService _purchaseDownPaymentService, IPurchaseDownPaymentDetailService _purchaseDownPaymentDetailService,
                               IPayableService _payableService, IContactService _contactService, ICashBankService _cashBankService);
        bool ValidDeleteObject(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentDetailService _purchaseDownPaymentDetailService);
        bool ValidConfirmObject(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentService _purchaseDownPaymentService,
                                IPurchaseDownPaymentDetailService _purchaseDownPaymentDetailService, ICashBankService _cashBankService,
                                IPayableService _payableService, IClosingService _closingService);
        bool ValidUnconfirmObject(PurchaseDownPayment purchaseDownPayment, IClosingService _closingService);
        bool ValidReconcileObject(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentDetailService _purchaseDownPaymentDetailService, ICashBankService _cashBankService, IClosingService _closingService);
        bool ValidUnreconcileObject(PurchaseDownPayment purchaseDownPayment, IClosingService _closingService);
        bool isValid(PurchaseDownPayment purchaseDownPayment);
        string PrintError(PurchaseDownPayment purchaseDownPayment);
    }
}
