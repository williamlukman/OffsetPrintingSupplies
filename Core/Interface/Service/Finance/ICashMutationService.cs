using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface ICashMutationService
    {
        IQueryable<CashMutation> GetQueryable();
        ICashMutationValidator GetValidator();
        IList<CashMutation> GetAll();
        CashMutation GetObjectById(int Id);
        IList<CashMutation> GetObjectsByCashBankId(int cashBankId);
        IList<CashMutation> GetObjectsBySourceDocument(int cashBankId, string SourceDocumentType, int SourceDocumentId);
        CashMutation CreateObject(CashMutation cashMutation, ICashBankService _cashBankService);
        CashMutation SoftDeleteObject(CashMutation cashMutation);
        bool DeleteObject(int Id);
        CashMutation CreateCashMutationForPaymentVoucher(PaymentVoucher paymentVoucher, CashBank cashBank);
        IList<CashMutation> SoftDeleteCashMutationForPaymentVoucher(PaymentVoucher paymentVoucher, CashBank cashBank);
        CashMutation CreateCashMutationForPurchaseAllowance(PurchaseAllowance purchaseAllowance, CashBank cashBank);
        IList<CashMutation> SoftDeleteCashMutationForPurchaseAllowance(PurchaseAllowance purchaseAllowance, CashBank cashBank);
        CashMutation CreateCashMutationForReceiptVoucher(ReceiptVoucher receiptVoucher, CashBank cashBank);
        IList<CashMutation> SoftDeleteCashMutationForReceiptVoucher(ReceiptVoucher receiptVoucher, CashBank cashBank);
        CashMutation CreateCashMutationForSalesAllowance(SalesAllowance salesAllowance, CashBank cashBank);
        IList<CashMutation> SoftDeleteCashMutationForSalesAllowance(SalesAllowance salesAllowance, CashBank cashBank);
        CashMutation CreateCashMutationForCashBankAdjustment(CashBankAdjustment cashBankAdjustment, CashBank cashBank);
        IList<CashMutation> SoftDeleteCashMutationForCashBankAdjustment(CashBankAdjustment cashBankAdjustment, CashBank cashBank);
        CashMutation CreateCashMutationForInterestAdjustment(InterestAdjustment interestAdjustment, CashBank cashBank);
        IList<CashMutation> SoftDeleteCashMutationForInterestAdjustment(InterestAdjustment interestAdjustment, CashBank cashBank);
        IList<CashMutation> CreateCashMutationForCashBankMutation(CashBankMutation cashBankMutation, CashBank sourceCashBank, CashBank targetCashBank);
        IList<CashMutation> SoftDeleteCashMutationForCashBankMutation(CashBankMutation cashBankMutation, CashBank sourceCashBank, CashBank targetCashBank);
        void CashMutateObject(CashMutation cashMutation, ICashBankService _cashBankService, ICurrencyService _currencyService);
        void ReverseCashMutateObject(CashMutation cashMutation, ICashBankService _cashBankService, ICurrencyService _currencyService);
    }
}