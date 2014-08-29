using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IAccountService
    {
        IQueryable<Account> GetQueryable();
        IAccountValidator GetValidator();
        IList<Account> GetAll();
        Account GetObjectById(int Id);
        Account GetObjectByIsLegacy(bool IsLegacy);
        Account CreateObject(Account account, ICashBankService _cashBankService);
        Account CreateLegacyObject(Account account, ICashBankService _cashBankService);
        Account SoftDeleteObject(Account account);
        bool DeleteObject(int Id);
        //Account CreateAccountForPaymentVoucher(PaymentVoucher paymentVoucher, CashBank cashBank);
        //Account CreateAccountForReceiptVoucher(ReceiptVoucher receiptVoucher, CashBank cashBank);
        //Account CreateAccountForCashBankAdjustment(CashBankAdjustment cashBankAdjustment, CashBank cashBank);
        //IList<Account> CreateAccountForCashBankMutation(CashBankMutation cashBankMutation, CashBank sourceCashBank, CashBank targetCashBank);
        //IList<Account> SoftDeleteAccountForPaymentVoucher(PaymentVoucher paymentVoucher, CashBank cashBank);
        //IList<Account> SoftDeleteAccountForReceiptVoucher(ReceiptVoucher receiptVoucher, CashBank cashBank);
        //IList<Account> SoftDeleteAccountForCashBankAdjustment(CashBankAdjustment cashBankAdjustment, CashBank cashBank);
        //IList<Account> SoftDeleteAccountForCashBankMutation(CashBankMutation cashBankMutation, CashBank sourceCashBank, CashBank targetCashBank);
    }
}