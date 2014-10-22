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
        PurchaseDownPayment VHasPaymentVoucher(PurchaseDownPayment purchaseDownPayment, IPaymentVoucherService _paymentVoucherService);
        PurchaseDownPayment VHasDownPaymentDate(PurchaseDownPayment purchaseDownPayment);
        PurchaseDownPayment VNotIsGBCH(PurchaseDownPayment purchaseDownPayment);
        PurchaseDownPayment VIfGBCHThenIsBank(PurchaseDownPayment purchaseDownPayment, ICashBankService _cashBankService);
        PurchaseDownPayment VIfGBCHThenHasDueDate(PurchaseDownPayment purchaseDownPayment);
        PurchaseDownPayment VPurchaseDownPaymentAllocationHasNoDetails(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentAllocationService _purchaseDownPaymentAllocationService,
                                                                       IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService);
        PurchaseDownPayment VTotalAmountNotNegativeNorZero(PurchaseDownPayment purchaseDownPayment);
        PurchaseDownPayment VHasNotBeenDeleted(PurchaseDownPayment purchaseDownPayment);
        PurchaseDownPayment VHasBeenConfirmed(PurchaseDownPayment purchaseDownPayment);
        PurchaseDownPayment VHasNotBeenConfirmed(PurchaseDownPayment purchaseDownPayment);
        PurchaseDownPayment VGeneralLedgerPostingHasNotBeenClosed(PurchaseDownPayment purchaseDownPayment, IClosingService _closingService, int CaseConfirmUnconfirm);
        PurchaseDownPayment VCreateObject(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentService _purchaseDownPaymentService, IContactService _contactService,
                                          ICashBankService _cashBankService, IPaymentVoucherService _paymentVoucherService);
        PurchaseDownPayment VUpdateObject(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentService _purchaseDownPaymentService, IContactService _contactService,
                                          ICashBankService _cashBankService, IPaymentVoucherService _paymentVoucherService);
        PurchaseDownPayment VDeleteObject(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentAllocationService _purchaseDownPaymentAllocationService, IPaymentVoucherDetailService _paymentVoucherDetailService);
        PurchaseDownPayment VHasConfirmationDate(PurchaseDownPayment purchaseDownPayment);
        PurchaseDownPayment VConfirmObject(PurchaseDownPayment purchaseDownPayment, ICashBankService _cashBankService, IPaymentVoucherService _paymentVoucherService,
                                           IPurchaseDownPaymentService _purchaseDownPaymentService, IContactService _contactService,
                                           IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService);
        PurchaseDownPayment VUnconfirmObject(PurchaseDownPayment purchaseDownPayment, ICashBankService _cashBankService, IPaymentVoucherService _paymentVoucherService,
                                            IPurchaseDownPaymentAllocationService _purchaseDownPaymentAllocationService, IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService,
                                            IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService);
        bool ValidCreateObject(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentService _purchaseDownPaymentService, IContactService _contactService,
                               ICashBankService _cashBankService, IPaymentVoucherService _paymentVoucherService);
        bool ValidUpdateObject(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentService _purchaseDownPaymentService, IContactService _contactService,
                               ICashBankService _cashBankService, IPaymentVoucherService _paymentVoucherService);
        bool ValidDeleteObject(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentAllocationService _purchaseDownPaymentAllocationService, IPaymentVoucherDetailService _paymentVoucherDetailService);
        bool ValidConfirmObject(PurchaseDownPayment purchaseDownPayment, ICashBankService _cashBankService, IPaymentVoucherService _paymentVoucherService,
                                IPurchaseDownPaymentService _purchaseDownPaymentService, IContactService _contactService, 
                                IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService);
        bool ValidUnconfirmObject(PurchaseDownPayment purchaseDownPayment, ICashBankService _cashBankService, IPaymentVoucherService _paymentVoucherService,
                                  IPurchaseDownPaymentAllocationService _purchaseDownPaymentAllocationService, IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService,
                                  IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService);
        bool isValid(PurchaseDownPayment purchaseDownPayment);
        string PrintError(PurchaseDownPayment purchaseDownPayment);
    }
}
