using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IInterestAdjustmentValidator
    {
        //InterestAdjustment VHasCashBank(InterestAdjustment interestAdjustment, ICashBankService _cashBankService);
        //InterestAdjustment VIncomeDate(InterestAdjustment interestAdjustment);
        //InterestAdjustment VHasBeenConfirmed(InterestAdjustment interestAdjustment);
        //InterestAdjustment VHasNotBeenConfirmed(InterestAdjustment interestAdjustment);
        //InterestAdjustment VNonZeroAmount(InterestAdjustment interestAdjustment);
        //InterestAdjustment VNonNegativeNorZeroCashBankAmount(InterestAdjustment interestAdjustment, ICashBankService _cashBankService, bool CaseConfirm);
        //InterestAdjustment VGeneralLedgerPostingHasNotBeenClosed(InterestAdjustment interestAdjustment, IClosingService _closingService);
        //InterestAdjustment VCreateObject(InterestAdjustment interestAdjustment, ICashBankService _cashBankService);
        //InterestAdjustment VUpdateObject(InterestAdjustment interestAdjustment, ICashBankService _cashBankService);
        //InterestAdjustment VDeleteObject(InterestAdjustment interestAdjustment);
        //InterestAdjustment VHasConfirmationDate(InterestAdjustment cashBankAdjusment);
        //InterestAdjustment VConfirmObject(InterestAdjustment interestAdjustment, ICashBankService _cashBankService, IClosingService _closingService);
        //InterestAdjustment VUnconfirmObject(InterestAdjustment interestAdjustment, ICashBankService _cashBankService, IClosingService _closingService);
        bool ValidCreateObject(InterestAdjustment interestAdjustment, ICashBankService _cashBankService);
        bool ValidUpdateObject(InterestAdjustment interestAdjustment, ICashBankService _cashBankService);
        bool ValidDeleteObject(InterestAdjustment interestAdjustment);
        bool ValidConfirmObject(InterestAdjustment interestAdjustment, ICashBankService _cashBankService, IClosingService _closingService);
        bool ValidUnconfirmObject(InterestAdjustment interestAdjustment, ICashBankService _cashBankService, IClosingService _closingService);
        bool isValid(InterestAdjustment interestAdjustment);
        string PrintError(InterestAdjustment interestAdjustment);
    }
}
