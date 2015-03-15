using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IBankAdministrationDetailService
    {
        IBankAdministrationDetailValidator GetValidator();
        IQueryable<BankAdministrationDetail> GetQueryable();
        IList<BankAdministrationDetail> GetAll();
        IList<BankAdministrationDetail> GetObjectsByBankAdministrationId(int bankAdministrationId);
        IList<BankAdministrationDetail> GetNonLegacyObjectsByBankAdministrationId(int bankAdministrationId);
        BankAdministrationDetail GetLegacyObjectByBankAdministrationId(int bankAdministrationId);
        BankAdministrationDetail GetObjectById(int Id);
        BankAdministrationDetail CreateObject(BankAdministrationDetail bankAdministrationDetail, IBankAdministrationService _bankAdministrationService, IAccountService _accountService);
        BankAdministrationDetail UpdateObject(BankAdministrationDetail bankAdministrationDetail, IBankAdministrationService _bankAdministrationService, IAccountService _accountService);
        BankAdministrationDetail SoftDeleteObject(BankAdministrationDetail bankAdministrationDetail, IBankAdministrationService _bankAdministrationService);
        bool DeleteObject(int Id);
        BankAdministrationDetail CreateLegacyObject(BankAdministrationDetail bankAdministrationDetail, IBankAdministrationService _bankAdministrationService, IAccountService _accountService);
        BankAdministrationDetail UpdateLegacyObject(BankAdministrationDetail bankAdministrationDetail, IBankAdministrationService _bankAdministrationService, IAccountService _accountService);
        BankAdministrationDetail ConfirmObject(BankAdministrationDetail bankAdministrationDetail, DateTime ConfirmationDate, IBankAdministrationService _bankAdministrationService);
        BankAdministrationDetail UnconfirmObject(BankAdministrationDetail bankAdministrationDetail, IBankAdministrationService _bankAdministrationService);
    }
}