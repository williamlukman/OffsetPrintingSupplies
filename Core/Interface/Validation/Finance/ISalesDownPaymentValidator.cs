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
        SalesDownPayment VHasReceiptDate(SalesDownPayment salesDownPayment);
        SalesDownPayment VNotIsGBCH(SalesDownPayment salesDownPayment);
        SalesDownPayment VIfGBCHThenIsBank(SalesDownPayment salesDownPayment, ICashBankService _cashBankService);
        SalesDownPayment VIfGBCHThenHasDueDate(SalesDownPayment salesDownPayment);
        SalesDownPayment VHasNoSalesDownPaymentDetail(SalesDownPayment salesDownPayment, ISalesDownPaymentDetailService _salesDownPaymentDetailService);
        SalesDownPayment VHasSalesDownPaymentDetails(SalesDownPayment salesDownPayment, ISalesDownPaymentDetailService _salesDownPaymentDetailService);
        SalesDownPayment VTotalAmountIsNotZero(SalesDownPayment salesDownPayment, ISalesDownPaymentDetailService _salesDownPaymentDetailService);
        SalesDownPayment VHasNotBeenDeleted(SalesDownPayment salesDownPayment);
        SalesDownPayment VHasBeenConfirmed(SalesDownPayment salesDownPayment);
        SalesDownPayment VHasNotBeenConfirmed(SalesDownPayment salesDownPayment);
        SalesDownPayment VTotalAmountEqualDetailsAmount(SalesDownPayment salesDownPayment, ISalesDownPaymentDetailService _salesDownPaymentDetailService);
        SalesDownPayment VAllSalesDownPaymentDetailsAreConfirmable(SalesDownPayment salesDownPayment, ISalesDownPaymentService _paymetnVoucherService,
                                                               ISalesDownPaymentDetailService salesDownPaymentDetailService, ICashBankService _cashBankService,
                                                               IReceivableService _receivableService);
        SalesDownPayment VCashBankIsGreaterThanOrEqualSalesDownPaymentDetails(SalesDownPayment salesDownPayment, ISalesDownPaymentDetailService _salesDownPaymentDetailService,
                                                                          ICashBankService _cashBankService, bool CaseReceipt);
        SalesDownPayment VHasBeenReconciled(SalesDownPayment salesDownPayment);
        SalesDownPayment VHasNotBeenReconciled(SalesDownPayment salesDownPayment);
        SalesDownPayment VHasReconciliationDate(SalesDownPayment salesDownPayment);
        SalesDownPayment VGeneralLedgerPostingHasNotBeenClosed(SalesDownPayment salesDownPayment, IClosingService _closingService, int CaseConfirmUnconfirm);
        SalesDownPayment VCreateObject(SalesDownPayment salesDownPayment, ISalesDownPaymentService _salesDownPaymentService, ISalesDownPaymentDetailService _salesDownPaymentDetailService,
                                     IReceivableService _receivableService, IContactService _contactService, ICashBankService _cashBankService);
        SalesDownPayment VUpdateObject(SalesDownPayment salesDownPayment, ISalesDownPaymentService _salesDownPaymentService, ISalesDownPaymentDetailService _salesDownPaymentDetailService,
                                     IReceivableService _receivableService, IContactService _contactService, ICashBankService _cashBankService);
        SalesDownPayment VDeleteObject(SalesDownPayment salesDownPayment, ISalesDownPaymentDetailService _salesDownPaymentDetailService);
        SalesDownPayment VHasConfirmationDate(SalesDownPayment salesDownPayment);
        SalesDownPayment VConfirmObject(SalesDownPayment salesDownPayment, ISalesDownPaymentService _salesDownPaymentService,
                                       ISalesDownPaymentDetailService _salesDownPaymentDetailService, ICashBankService _cashBankService,
                                       IReceivableService _receivableService, IClosingService _closingService);
        SalesDownPayment VUnconfirmObject(SalesDownPayment salesDownPayment, ISalesDownPaymentDetailService _salesDownPaymentDetailService,
                                        ICashBankService _cashBankService, IClosingService _closingService);
        SalesDownPayment VReconcileObject(SalesDownPayment salesDownPayment, IClosingService _closingService);
        SalesDownPayment VUnreconcileObject(SalesDownPayment salesDownPayment, ISalesDownPaymentDetailService _salesDownPaymentDetailService, ICashBankService _cashBankService, IClosingService _closingService);
        bool ValidCreateObject(SalesDownPayment salesDownPayment, ISalesDownPaymentService _salesDownPaymentService, ISalesDownPaymentDetailService _salesDownPaymentDetailService,
                               IReceivableService _receivableService, IContactService _contactService, ICashBankService _cashBankService);
        bool ValidUpdateObject(SalesDownPayment salesDownPayment, ISalesDownPaymentService _salesDownPaymentService, ISalesDownPaymentDetailService _salesDownPaymentDetailService,
                               IReceivableService _receivableService, IContactService _contactService, ICashBankService _cashBankService);
        bool ValidDeleteObject(SalesDownPayment salesDownPayment, ISalesDownPaymentDetailService _salesDownPaymentDetailService);
        bool ValidConfirmObject(SalesDownPayment salesDownPayment, ISalesDownPaymentService _salesDownPaymentService,
                                ISalesDownPaymentDetailService _salesDownPaymentDetailService, ICashBankService _cashBankService,
                                IReceivableService _receivableService, IClosingService _closingService);
        bool ValidUnconfirmObject(SalesDownPayment salesDownPayment, ISalesDownPaymentDetailService _salesDownPaymentDetailService, ICashBankService _cashBankService, IClosingService _closingService);
        bool ValidReconcileObject(SalesDownPayment salesDownPayment, IClosingService _closingService);
        bool ValidUnreconcileObject(SalesDownPayment salesDownPayment, ISalesDownPaymentDetailService _salesDownPaymentDetailService, ICashBankService _cashBankService, IClosingService _closingService);
        bool isValid(SalesDownPayment salesDownPayment);
        string PrintError(SalesDownPayment salesDownPayment);
    }
}
