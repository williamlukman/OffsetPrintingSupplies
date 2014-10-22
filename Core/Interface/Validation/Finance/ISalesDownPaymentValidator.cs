using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface ISalesDownPaymentValidator
    {
        SalesDownPayment VHasContact(SalesDownPayment salesDownPayment, IContactService _contactService);
        SalesDownPayment VHasCashBank(SalesDownPayment salesDownPayment, ICashBankService _cashBankService);
        SalesDownPayment VHasReceiptVoucher(SalesDownPayment salesDownPayment, IReceiptVoucherService _receiptVoucherService);
        SalesDownPayment VHasDownPaymentDate(SalesDownPayment salesDownPayment);
        SalesDownPayment VNotIsGBCH(SalesDownPayment salesDownPayment);
        SalesDownPayment VIfGBCHThenIsBank(SalesDownPayment salesDownPayment, ICashBankService _cashBankService);
        SalesDownPayment VIfGBCHThenHasDueDate(SalesDownPayment salesDownPayment);
        SalesDownPayment VSalesDownPaymentAllocationHasNoDetails(SalesDownPayment salesDownPayment, ISalesDownPaymentAllocationService _salesDownPaymentAllocationService,
                                                                       ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService);
        SalesDownPayment VTotalAmountNotNegativeNorZero(SalesDownPayment salesDownPayment);
        SalesDownPayment VHasNotBeenDeleted(SalesDownPayment salesDownPayment);
        SalesDownPayment VHasBeenConfirmed(SalesDownPayment salesDownPayment);
        SalesDownPayment VHasNotBeenConfirmed(SalesDownPayment salesDownPayment);
        SalesDownPayment VGeneralLedgerPostingHasNotBeenClosed(SalesDownPayment salesDownPayment, IClosingService _closingService, int CaseConfirmUnconfirm);
        SalesDownPayment VCreateObject(SalesDownPayment salesDownPayment, ISalesDownPaymentService _salesDownPaymentService, IContactService _contactService,
                                          ICashBankService _cashBankService, IReceiptVoucherService _receiptVoucherService);
        SalesDownPayment VUpdateObject(SalesDownPayment salesDownPayment, ISalesDownPaymentService _salesDownPaymentService, IContactService _contactService,
                                          ICashBankService _cashBankService, IReceiptVoucherService _receiptVoucherService);
        SalesDownPayment VDeleteObject(SalesDownPayment salesDownPayment, ISalesDownPaymentAllocationService _salesDownPaymentAllocationService, IReceiptVoucherDetailService _receiptVoucherDetailService);
        SalesDownPayment VHasConfirmationDate(SalesDownPayment salesDownPayment);
        SalesDownPayment VConfirmObject(SalesDownPayment salesDownPayment, ICashBankService _cashBankService, IReceiptVoucherService _receiptVoucherService,
                                           ISalesDownPaymentService _salesDownPaymentService, IContactService _contactService,
                                           IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService);
        SalesDownPayment VUnconfirmObject(SalesDownPayment salesDownPayment, ICashBankService _cashBankService, IReceiptVoucherService _receiptVoucherService,
                                            ISalesDownPaymentAllocationService _salesDownPaymentAllocationService, ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService,
                                            IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService);
        bool ValidCreateObject(SalesDownPayment salesDownPayment, ISalesDownPaymentService _salesDownPaymentService, IContactService _contactService,
                               ICashBankService _cashBankService, IReceiptVoucherService _receiptVoucherService);
        bool ValidUpdateObject(SalesDownPayment salesDownPayment, ISalesDownPaymentService _salesDownPaymentService, IContactService _contactService,
                               ICashBankService _cashBankService, IReceiptVoucherService _receiptVoucherService);
        bool ValidDeleteObject(SalesDownPayment salesDownPayment, ISalesDownPaymentAllocationService _salesDownPaymentAllocationService, IReceiptVoucherDetailService _receiptVoucherDetailService);
        bool ValidConfirmObject(SalesDownPayment salesDownPayment, ICashBankService _cashBankService, IReceiptVoucherService _receiptVoucherService,
                                ISalesDownPaymentService _salesDownPaymentService, IContactService _contactService, 
                                IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService);
        bool ValidUnconfirmObject(SalesDownPayment salesDownPayment, ICashBankService _cashBankService, IReceiptVoucherService _receiptVoucherService,
                                  ISalesDownPaymentAllocationService _salesDownPaymentAllocationService, ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService,
                                  IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService);
        bool isValid(SalesDownPayment salesDownPayment);
        string PrintError(SalesDownPayment salesDownPayment);
    }
}
