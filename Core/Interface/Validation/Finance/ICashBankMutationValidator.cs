using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface ICashBankMutationValidator
    {
        CashBankMutation VHasDifferentCashBank(CashBankMutation cashBankMutation);
        CashBankMutation VHasSourceCashBank(CashBankMutation cashBankMutation, ICashBankMutationService _cashBankMutationService);
        CashBankMutation VHasTargetCashBank(CashBankMutation cashBankMutation, ICashBankMutationService _cashBankMutationService);
        CashBankMutation VHasNotBeenConfirmed(CashBankMutation cashBankMutation);
        CashBankMutation VHasBeenConfirmed(CashBankMutation cashBankMutation);
        CashBankMutation VNonNegativeNorZeroSourceCashBank(CashBankMutation cashBankMutation, ICashBankMutationService _cashBankMutationService);
        CashBankMutation VNonNegativeNorZeroTargetCashBank(CashBankMutation cashBankMutation, ICashBankMutationService _cashBankMutationService);
        CashBankMutation VCreateObject(CashBankMutation cashBankMutation, ICashBankMutationService _cashBankMutationService);
        CashBankMutation VUpdateObject(CashBankMutation cashBankMutation, ICashBankMutationService _cashBankMutationService);
        CashBankMutation VDeleteObject(CashBankMutation cashBankMutation);
        CashBankMutation VConfirmObject(CashBankMutation cashBankMutation, ICashBankMutationService _cashBankMutationService);
        CashBankMutation VUnconfirmObject(CashBankMutation cashBankMutation, ICashBankMutationService _cashBankMutationService);
        bool ValidCreateObject(CashBankMutation cashBankMutation, ICashBankService _cashBankService);
        bool ValidUpdateObject(CashBankMutation cashBankMutation, ICashBankService _cashBankService);
        bool ValidDeleteObject(CashBankMutation cashBankMutation);
        bool ValidConfirmObject(CashBankMutation cashBankMutation, ICashBankMutationService _cashBankMutationService);
        bool ValidUnconfirmObject(CashBankMutation cashBankMutation, ICashBankMutationService _cashBankMutationService);
        bool isValid(CashBankMutation cashBankMutation);
        string PrintError(CashBankMutation cashBankMutation);
    }
}