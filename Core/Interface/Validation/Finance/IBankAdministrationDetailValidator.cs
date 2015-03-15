using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IBankAdministrationDetailValidator
    {
        //BankAdministrationDetail VHasBankAdministration(BankAdministrationDetail bankAdministrationDetail, IBankAdministrationService _bankAdministrationService);
        //BankAdministrationDetail VHasAccount(BankAdministrationDetail bankAdministrationDetail, IAccountService _accountService);
        //BankAdministrationDetail VHasBeenConfirmed(BankAdministrationDetail bankAdministrationDetail, IBankAdministrationService _bankAdministrationService);
        //BankAdministrationDetail VHasNotBeenConfirmed(BankAdministrationDetail bankAdministrationDetail, IBankAdministrationService _bankAdministrationService);
        //BankAdministrationDetail VHasNotBeenDeleted(BankAdministrationDetail bankAdministrationDetail);
        //BankAdministrationDetail VAmountIsTheSameWithBankAdministrationAmount(BankAdministrationDetail bankAdministrationDetail, IBankAdministrationService _bankAdministrationService);
        //BankAdministrationDetail VNonNegativeAmount(BankAdministrationDetail bankAdministrationDetail);
        //BankAdministrationDetail VStatusIsCredit(BankAdministrationDetail bankAdministrationDetail);
        //BankAdministrationDetail VStatusIsDebit(BankAdministrationDetail bankAdministrationDetail);
        //BankAdministrationDetail VNotLegacyObject(BankAdministrationDetail bankAdministrationDetail);
        //BankAdministrationDetail VHasConfirmationDate(BankAdministrationDetail bankAdministrationDetail);
        //BankAdministrationDetail VCreateObject(BankAdministrationDetail bankAdministrationDetail, IBankAdministrationService _bankAdministrationService, IBankAdministrationDetailService _bankAdministrationDetailService, IAccountService _accountService);
        //BankAdministrationDetail VUpdateObject(BankAdministrationDetail bankAdministrationDetail, IBankAdministrationService _bankAdministrationService, IBankAdministrationDetailService _bankAdministrationDetailService, IAccountService _accountService);
        //BankAdministrationDetail VDeleteObject(BankAdministrationDetail bankAdministrationDetail, IBankAdministrationService _bankAdministrationService);
        //BankAdministrationDetail VCreateLegacyObject(BankAdministrationDetail bankAdministrationDetail, IBankAdministrationService _bankAdministrationService, IBankAdministrationDetailService _bankAdministrationDetailService, IAccountService _accountService);
        //BankAdministrationDetail VUpdateLegacyObject(BankAdministrationDetail bankAdministrationDetail, IBankAdministrationService _bankAdministrationService, IBankAdministrationDetailService _bankAdministrationDetailService, IAccountService _accountService);
        //BankAdministrationDetail VConfirmObject(BankAdministrationDetail bankAdministrationDetail, IBankAdministrationService _bankAdministrationService);
        //BankAdministrationDetail VUnconfirmObject(BankAdministrationDetail bankAdministrationDetail, IBankAdministrationService _bankAdministrationService);
        bool ValidCreateObject(BankAdministrationDetail bankAdministrationDetail, IBankAdministrationService _bankAdministrationService, IBankAdministrationDetailService _bankAdministrationDetailService, IAccountService _accountService);
        bool ValidUpdateObject(BankAdministrationDetail bankAdministrationDetail, IBankAdministrationService _bankAdministrationService, IBankAdministrationDetailService _bankAdministrationDetailService, IAccountService _accountService);
        bool ValidDeleteObject(BankAdministrationDetail bankAdministrationDetail, IBankAdministrationService _bankAdministrationService);
        bool ValidCreateLegacyObject(BankAdministrationDetail bankAdministrationDetail, IBankAdministrationService _bankAdministrationService, IBankAdministrationDetailService _bankAdministrationDetailService, IAccountService _accountService);
        bool ValidUpdateLegacyObject(BankAdministrationDetail bankAdministrationDetail, IBankAdministrationService _bankAdministrationService, IBankAdministrationDetailService _bankAdministrationDetailService, IAccountService _accountService);
        bool ValidConfirmObject(BankAdministrationDetail bankAdministrationDetail, IBankAdministrationService _bankAdministrationService);
        bool ValidUnconfirmObject(BankAdministrationDetail bankAdministrationDetail, IBankAdministrationService _bankAdministrationService);
        bool isValid(BankAdministrationDetail bankAdministrationDetail);
        string PrintError(BankAdministrationDetail bankAdministrationDetail);
    }
}
