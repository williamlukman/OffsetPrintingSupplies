using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface ICashBankMutationService
    {
        ICashBankMutationValidator GetValidator();
        IList<CashBankMutation> GetAll();
        CashBankMutation GetObjectById(int Id);
        CashBank GetSourceCashBank(CashBankMutation cashBankMutation);
        CashBank GetTargetCashBank(CashBankMutation cashBankMutation);
        CashBankMutation CreateObject(CashBankMutation cashBankMutation, ICashBankService _cashBankService);
        CashBankMutation UpdateObject(CashBankMutation cashBankMutation, ICashBankService _cashBankService);
        CashBankMutation SoftDeleteObject(CashBankMutation cashBankMutation);
        bool DeleteObject(int Id);
        CashBankMutation ConfirmObject(CashBankMutation cashBankMutation, ICashBankService _cashBankService);
        CashBankMutation UnconfirmObject(CashBankMutation cashBankMutation, ICashBankService _cashBankService);
    }
}