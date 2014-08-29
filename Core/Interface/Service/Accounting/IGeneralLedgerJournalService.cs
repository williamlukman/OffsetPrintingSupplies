using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IGeneralLedgerJournalService
    {
        IQueryable<GeneralLedgerJournal> GetQueryable();
        IGeneralLedgerJournalValidator GetValidator();
        IList<GeneralLedgerJournal> GetAll();
        GeneralLedgerJournal GetObjectById(int Id);
        IList<GeneralLedgerJournal> GetObjectsByAccountId(int accountId);
        IList<GeneralLedgerJournal> GetObjectsBySourceDocument(int accountId, string SourceDocumentType, int SourceDocumentId);
        GeneralLedgerJournal CreateObject(GeneralLedgerJournal generalLedgerJournal, IAccountService _accountService);
        GeneralLedgerJournal SoftDeleteObject(GeneralLedgerJournal generalLedgerJournal);
        bool DeleteObject(int Id);
        //GeneralLedgerJournal CreateGeneralLedgerJournalForPaymentVoucher(PaymentVoucher paymentVoucher, CashBank cashBank);
        //GeneralLedgerJournal CreateGeneralLedgerJournalForReceiptVoucher(ReceiptVoucher receiptVoucher, CashBank cashBank);
        //GeneralLedgerJournal CreateGeneralLedgerJournalForCashBankAdjustment(CashBankAdjustment cashBankAdjustment, CashBank cashBank);
        //IList<GeneralLedgerJournal> CreateGeneralLedgerJournalForCashBankMutation(CashBankMutation cashBankMutation, CashBank sourceCashBank, CashBank targetCashBank);
        //IList<GeneralLedgerJournal> SoftDeleteGeneralLedgerJournalForPaymentVoucher(PaymentVoucher paymentVoucher, CashBank cashBank);
        //IList<GeneralLedgerJournal> SoftDeleteGeneralLedgerJournalForReceiptVoucher(ReceiptVoucher receiptVoucher, CashBank cashBank);
        //IList<GeneralLedgerJournal> SoftDeleteGeneralLedgerJournalForCashBankAdjustment(CashBankAdjustment cashBankAdjustment, CashBank cashBank);
        //IList<GeneralLedgerJournal> SoftDeleteGeneralLedgerJournalForCashBankMutation(CashBankMutation cashBankMutation, CashBank sourceCashBank, CashBank targetCashBank);
    }
}