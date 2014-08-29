using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IClosingService
    {
        IQueryable<Closing> GetQueryable();
        IClosingValidator GetValidator();
        IList<Closing> GetAll();
        Closing GetObjectById(int Id);
        Closing CreateObject(Closing closing, ICashBankService _cashBankService);
        Closing SoftDeleteObject(Closing closing);
        bool DeleteObject(int Id);
        //Closing CreateClosingForPaymentVoucher(PaymentVoucher paymentVoucher, CashBank cashBank);
        //Closing CreateClosingForReceiptVoucher(ReceiptVoucher receiptVoucher, CashBank cashBank);
        //Closing CreateClosingForCashBankAdjustment(CashBankAdjustment cashBankAdjustment, CashBank cashBank);
        //IList<Closing> CreateClosingForCashBankMutation(CashBankMutation cashBankMutation, CashBank sourceCashBank, CashBank targetCashBank);
        //IList<Closing> SoftDeleteClosingForPaymentVoucher(PaymentVoucher paymentVoucher, CashBank cashBank);
        //IList<Closing> SoftDeleteClosingForReceiptVoucher(ReceiptVoucher receiptVoucher, CashBank cashBank);
        //IList<Closing> SoftDeleteClosingForCashBankAdjustment(CashBankAdjustment cashBankAdjustment, CashBank cashBank);
        //IList<Closing> SoftDeleteClosingForCashBankMutation(CashBankMutation cashBankMutation, CashBank sourceCashBank, CashBank targetCashBank);
    }
}