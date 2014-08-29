using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IValidCombService
    {
        IQueryable<ValidComb> GetQueryable();
        IValidCombValidator GetValidator();
        IList<ValidComb> GetAll();
        ValidComb GetObjectById(int Id);
        ValidComb CreateObject(ValidComb validComb, IAccountService _accountService, IClosingService _closingService);
        //ValidComb SoftDeleteObject(ValidComb validComb);
        bool DeleteObject(int Id);
        //ValidComb CreateValidCombForPaymentVoucher(PaymentVoucher paymentVoucher, CashBank cashBank);
        //ValidComb CreateValidCombForReceiptVoucher(ReceiptVoucher receiptVoucher, CashBank cashBank);
        //ValidComb CreateValidCombForCashBankAdjustment(CashBankAdjustment cashBankAdjustment, CashBank cashBank);
        //IList<ValidComb> CreateValidCombForCashBankMutation(CashBankMutation cashBankMutation, CashBank sourceCashBank, CashBank targetCashBank);
        //IList<ValidComb> SoftDeleteValidCombForPaymentVoucher(PaymentVoucher paymentVoucher, CashBank cashBank);
        //IList<ValidComb> SoftDeleteValidCombForReceiptVoucher(ReceiptVoucher receiptVoucher, CashBank cashBank);
        //IList<ValidComb> SoftDeleteValidCombForCashBankAdjustment(CashBankAdjustment cashBankAdjustment, CashBank cashBank);
        //IList<ValidComb> SoftDeleteValidCombForCashBankMutation(CashBankMutation cashBankMutation, CashBank sourceCashBank, CashBank targetCashBank);
    }
}