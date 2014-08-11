using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface ICashMutationValidator
    {
        CashMutation VHasCashBank(CashMutation cashMutation, ICashBankService _cashBankService);
        CashMutation VStatus(CashMutation cashMutation);
        CashMutation VSourceDocumentType(CashMutation cashMutation);
        CashMutation VNonNegativeNorZeroAmount(CashMutation cashMutation);
        CashMutation VCreateObject(CashMutation cashMutation, ICashBankService _cashBankService);
        CashMutation VDeleteObject(CashMutation cashMutation);
        bool ValidCreateObject(CashMutation cashMutation, ICashBankService _cashBankService);
        bool ValidDeleteObject(CashMutation cashMutation);
        bool isValid(CashMutation cashMutation);
        string PrintError(CashMutation cashMutation);
    }
}
