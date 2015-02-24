using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IInterestIncomeValidator
    {
        //InterestIncome VHasCashBank(InterestIncome interestIncome, ICashBankService _cashBankService);
        //InterestIncome VIncomeDate(InterestIncome interestIncome);
        //InterestIncome VHasBeenConfirmed(InterestIncome interestIncome);
        //InterestIncome VHasNotBeenConfirmed(InterestIncome interestIncome);
        //InterestIncome VNonZeroAmount(InterestIncome interestIncome);
        //InterestIncome VNonNegativeNorZeroCashBankAmount(InterestIncome interestIncome, ICashBankService _cashBankService, bool CaseConfirm);
        //InterestIncome VGeneralLedgerPostingHasNotBeenClosed(InterestIncome interestIncome, IClosingService _closingService);
        //InterestIncome VCreateObject(InterestIncome interestIncome, ICashBankService _cashBankService);
        //InterestIncome VUpdateObject(InterestIncome interestIncome, ICashBankService _cashBankService);
        //InterestIncome VDeleteObject(InterestIncome interestIncome);
        //InterestIncome VHasConfirmationDate(InterestIncome cashBankAdjusment);
        //InterestIncome VConfirmObject(InterestIncome interestIncome, ICashBankService _cashBankService, IClosingService _closingService);
        //InterestIncome VUnconfirmObject(InterestIncome interestIncome, ICashBankService _cashBankService, IClosingService _closingService);
        bool ValidCreateObject(InterestIncome interestIncome, ICashBankService _cashBankService);
        bool ValidUpdateObject(InterestIncome interestIncome, ICashBankService _cashBankService);
        bool ValidDeleteObject(InterestIncome interestIncome);
        bool ValidConfirmObject(InterestIncome interestIncome, ICashBankService _cashBankService, IClosingService _closingService);
        bool ValidUnconfirmObject(InterestIncome interestIncome, ICashBankService _cashBankService, IClosingService _closingService);
        bool isValid(InterestIncome interestIncome);
        string PrintError(InterestIncome interestIncome);
    }
}
