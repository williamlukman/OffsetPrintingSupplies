using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface ISalesAllowanceValidator
    {
        SalesAllowance VHasContact(SalesAllowance salesAllowance, IContactService _contactService);
        SalesAllowance VHasCashBank(SalesAllowance salesAllowance, ICashBankService _cashBankService);
        SalesAllowance VHasReceiptDate(SalesAllowance salesAllowance);
        SalesAllowance VNotIsGBCH(SalesAllowance salesAllowance);
        SalesAllowance VIfGBCHThenIsBank(SalesAllowance salesAllowance, ICashBankService _cashBankService);
        SalesAllowance VIfGBCHThenHasDueDate(SalesAllowance salesAllowance);
        SalesAllowance VHasNoSalesAllowanceDetail(SalesAllowance salesAllowance, ISalesAllowanceDetailService _salesAllowanceDetailService);
        SalesAllowance VHasSalesAllowanceDetails(SalesAllowance salesAllowance, ISalesAllowanceDetailService _salesAllowanceDetailService);
        SalesAllowance VTotalAmountIsNotZero(SalesAllowance salesAllowance, ISalesAllowanceDetailService _salesAllowanceDetailService);
        SalesAllowance VHasNotBeenDeleted(SalesAllowance salesAllowance);
        SalesAllowance VHasBeenConfirmed(SalesAllowance salesAllowance);
        SalesAllowance VHasNotBeenConfirmed(SalesAllowance salesAllowance);
        SalesAllowance VTotalAmountEqualDetailsAmount(SalesAllowance salesAllowance, ISalesAllowanceDetailService _salesAllowanceDetailService);
        SalesAllowance VAllSalesAllowanceDetailsAreConfirmable(SalesAllowance salesAllowance, ISalesAllowanceService _paymetnVoucherService,
                                                               ISalesAllowanceDetailService salesAllowanceDetailService, ICashBankService _cashBankService,
                                                               IReceivableService _receivableService);
        SalesAllowance VCashBankIsGreaterThanOrEqualSalesAllowanceDetails(SalesAllowance salesAllowance, ISalesAllowanceDetailService _salesAllowanceDetailService,
                                                                          ICashBankService _cashBankService, bool CaseReceipt);
        SalesAllowance VHasBeenReconciled(SalesAllowance salesAllowance);
        SalesAllowance VHasNotBeenReconciled(SalesAllowance salesAllowance);
        SalesAllowance VHasReconciliationDate(SalesAllowance salesAllowance);
        SalesAllowance VGeneralLedgerPostingHasNotBeenClosed(SalesAllowance salesAllowance, IClosingService _closingService, int CaseConfirmUnconfirm);
        SalesAllowance VCreateObject(SalesAllowance salesAllowance, ISalesAllowanceService _salesAllowanceService, ISalesAllowanceDetailService _salesAllowanceDetailService,
                                     IReceivableService _receivableService, IContactService _contactService, ICashBankService _cashBankService);
        SalesAllowance VUpdateObject(SalesAllowance salesAllowance, ISalesAllowanceService _salesAllowanceService, ISalesAllowanceDetailService _salesAllowanceDetailService,
                                     IReceivableService _receivableService, IContactService _contactService, ICashBankService _cashBankService);
        SalesAllowance VDeleteObject(SalesAllowance salesAllowance, ISalesAllowanceDetailService _salesAllowanceDetailService);
        SalesAllowance VHasConfirmationDate(SalesAllowance salesAllowance);
        SalesAllowance VConfirmObject(SalesAllowance salesAllowance, ISalesAllowanceService _salesAllowanceService,
                                       ISalesAllowanceDetailService _salesAllowanceDetailService, ICashBankService _cashBankService,
                                       IReceivableService _receivableService, IClosingService _closingService);
        SalesAllowance VUnconfirmObject(SalesAllowance salesAllowance, ISalesAllowanceDetailService _salesAllowanceDetailService,
                                        ICashBankService _cashBankService, IClosingService _closingService);
        SalesAllowance VReconcileObject(SalesAllowance salesAllowance, IClosingService _closingService);
        SalesAllowance VUnreconcileObject(SalesAllowance salesAllowance, ISalesAllowanceDetailService _salesAllowanceDetailService, ICashBankService _cashBankService, IClosingService _closingService);
        bool ValidCreateObject(SalesAllowance salesAllowance, ISalesAllowanceService _salesAllowanceService, ISalesAllowanceDetailService _salesAllowanceDetailService,
                               IReceivableService _receivableService, IContactService _contactService, ICashBankService _cashBankService);
        bool ValidUpdateObject(SalesAllowance salesAllowance, ISalesAllowanceService _salesAllowanceService, ISalesAllowanceDetailService _salesAllowanceDetailService,
                               IReceivableService _receivableService, IContactService _contactService, ICashBankService _cashBankService);
        bool ValidDeleteObject(SalesAllowance salesAllowance, ISalesAllowanceDetailService _salesAllowanceDetailService);
        bool ValidConfirmObject(SalesAllowance salesAllowance, ISalesAllowanceService _salesAllowanceService,
                                ISalesAllowanceDetailService _salesAllowanceDetailService, ICashBankService _cashBankService,
                                IReceivableService _receivableService, IClosingService _closingService);
        bool ValidUnconfirmObject(SalesAllowance salesAllowance, ISalesAllowanceDetailService _salesAllowanceDetailService, ICashBankService _cashBankService, IClosingService _closingService);
        bool ValidReconcileObject(SalesAllowance salesAllowance, IClosingService _closingService);
        bool ValidUnreconcileObject(SalesAllowance salesAllowance, ISalesAllowanceDetailService _salesAllowanceDetailService, ICashBankService _cashBankService, IClosingService _closingService);
        bool isValid(SalesAllowance salesAllowance);
        string PrintError(SalesAllowance salesAllowance);
    }
}
