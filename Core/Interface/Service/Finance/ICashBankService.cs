using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface ICashBankService
    {
        ICashBankValidator GetValidator();
        IList<CashBank> GetAll();
        CashBank GetObjectById(int Id);
        CashBank GetObjectByName(string Name);
        CashBank CreateObject(CashBank cashBank);
        CashBank CreateObject(string name, string description);
        CashBank UpdateObject(CashBank cashBank);
        CashBank SoftDeleteObject(CashBank cashBank, ICashMutationService _cashMutationService);
        CashBank AdjustAmount(CashBank cashBank, decimal amount);
        bool DeleteObject(int Id);
        bool IsNameDuplicated(CashBank cashBank);
    }
}