using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IBankAdministrationValidator
    {
        //BankAdministration VHasCashBank(BankAdministration bankAdministration, ICashBankService _cashBankService);
        //BankAdministration VIncomeDate(BankAdministration bankAdministration);
        //BankAdministration VHasBeenConfirmed(BankAdministration bankAdministration);
        //BankAdministration VHasNotBeenConfirmed(BankAdministration bankAdministration);
        //BankAdministration VNonZeroAmount(BankAdministration bankAdministration);
        //BankAdministration VNonNegativeNorZeroCashBankAmount(BankAdministration bankAdministration, ICashBankService _cashBankService, bool CaseConfirm);
        //BankAdministration VGeneralLedgerPostingHasNotBeenClosed(BankAdministration bankAdministration, IClosingService _closingService);
        //BankAdministration VCreateObject(BankAdministration bankAdministration, ICashBankService _cashBankService);
        //BankAdministration VUpdateObject(BankAdministration bankAdministration, ICashBankService _cashBankService);
        //BankAdministration VDeleteObject(BankAdministration bankAdministration);
        //BankAdministration VHasConfirmationDate(BankAdministration cashBankAdjusment);
        //BankAdministration VConfirmObject(BankAdministration bankAdministration, ICashBankService _cashBankService, IClosingService _closingService);
        //BankAdministration VUnconfirmObject(BankAdministration bankAdministration, ICashBankService _cashBankService, IClosingService _closingService);
        bool ValidCreateObject(BankAdministration bankAdministration, ICashBankService _cashBankService);
        bool ValidUpdateObject(BankAdministration bankAdministration, ICashBankService _cashBankService);
        bool ValidDeleteObject(BankAdministration bankAdministration);
        bool ValidConfirmObject(BankAdministration bankAdministration, ICashBankService _cashBankService, IClosingService _closingService);
        bool ValidUnconfirmObject(BankAdministration bankAdministration, ICashBankService _cashBankService, IClosingService _closingService);
        bool isValid(BankAdministration bankAdministration);
        string PrintError(BankAdministration bankAdministration);
    }
}
