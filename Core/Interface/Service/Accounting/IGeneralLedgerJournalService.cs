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
        IList<GeneralLedgerJournal> CreateConfirmationJournalForMemorial(Memorial memorial, IMemorialDetailService _memorialDetailService, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateUnconfirmationJournalForMemorial(Memorial memorial, IMemorialDetailService _memorialDetailService, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateConfirmationJournalForPaymentVoucher(PaymentVoucher paymentVoucher, CashBank cashBank, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateUnconfirmationJournalForPaymentVoucher(PaymentVoucher paymentVoucher, CashBank cashBank, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateConfirmationJournalForPaymentRequest(PaymentRequest paymentRequest, IPaymentRequestDetailService _paymentRequestDetailService, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateUnconfirmationJournalForPaymentRequest(PaymentRequest paymentRequest, IPaymentRequestDetailService _paymentRequestDetailService, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateConfirmationJournalForPurchaseDownPayment(PurchaseDownPayment purchaseDownPayment, CashBank cashBank, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateUnconfirmationJournalForPurchaseDownPayment(PurchaseDownPayment urchaseDownPayment, CashBank cashBank, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateConfirmationJournalForPurchaseAllowance(PurchaseAllowance purchaseAllowance, CashBank cashBank, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateUnconfirmationJournalForPurchaseAllowance(PurchaseAllowance purchaseAllowance, CashBank cashBank, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateConfirmationJournalForReceiptVoucher(ReceiptVoucher receiptVoucher, CashBank cashBank, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateUnconfirmationJournalForReceiptVoucher(ReceiptVoucher receiptVoucher, CashBank cashBank, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateConfirmationJournalForSalesDownPayment(SalesDownPayment salesDownPayment, CashBank cashBank, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateUnconfirmationJournalForSalesDownPayment(SalesDownPayment salesDownPayment, CashBank cashBank, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateConfirmationJournalForSalesAllowance(SalesAllowance salesAllowance, CashBank cashBank, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateUnconfirmationJournalForSalesAllowance(SalesAllowance salesAllowance, CashBank cashBank, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateConfirmationJournalForCashBankAdjustment(CashBankAdjustment cashBankAdjustment, CashBank cashBank, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateUnconfirmationJournalForCashBankAdjustment(CashBankAdjustment cashBankAdjustment, CashBank cashBank, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateConfirmationJournalForCashBankMutation(CashBankMutation cashBankMutation, CashBank sourceCashBank, CashBank targetCashBank, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateUnconfirmationJournalForCashBankMutation(CashBankMutation cashBankMutation, CashBank sourceCashBank, CashBank targetCashBank, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateConfirmationJournalForStockAdjustment(StockAdjustment stockAdjustment, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateUnconfirmationJournalForStockAdjustment(StockAdjustment stockAdjustment, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateConfirmationJournalForPurchaseReceival(PurchaseReceival purchaseReceival, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateUnconfirmationJournalForPurchaseReceival(PurchaseReceival purchaseReceival, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateConfirmationJournalForPurchaseInvoice(PurchaseInvoice purchaseInvoice, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateUnconfirmationJournalForPurchaseInvoice(PurchaseInvoice purchaseInvoice, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateConfirmationJournalForDeliveryOrder(DeliveryOrder deliveryOrder, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateUnconfirmationJournalForDeliveryOrder(DeliveryOrder deliveryOrder, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateReconciliationJournalForTemporaryDeliveryOrderWaste(TemporaryDeliveryOrder temporaryDeliveryOrder, DateTime PushDate, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateUnreconciliationJournalForTemporaryDeliveryOrderWaste(TemporaryDeliveryOrder temporaryDeliveryOrder, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateConfirmationJournalForSalesInvoice(SalesInvoice salesInvoice, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateUnconfirmationJournalForSalesInvoice(SalesInvoice salesInvoice, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateFinishedJournalForRecoveryOrderDetail(RecoveryOrderDetail recoveryOrderDetail, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateUnfinishedJournalForRecoveryOrderDetail(RecoveryOrderDetail recoveryOrderDetail, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateRejectedJournalForRecoveryOrderDetail(RecoveryOrderDetail recoveryOrderDetail, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateUndoRejectedJournalForRecoveryOrderDetail(RecoveryOrderDetail recoveryOrderDetail, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateFinishedJournalForBlanketOrderDetail(BlanketOrderDetail blanketOrderDetail, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateUnfinishedJournalForBlanketOrderDetail(BlanketOrderDetail blanketOrderDetail, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateRejectedJournalForBlanketOrderDetail(BlanketOrderDetail blanketOrderDetail, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateUndoRejectedJournalForBlanketOrderDetail(BlanketOrderDetail blanketOrderDetail, IAccountService _accountService);
    }
}