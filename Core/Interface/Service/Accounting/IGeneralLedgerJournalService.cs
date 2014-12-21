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
        IList<GeneralLedgerJournal> CreateConfirmationJournalForPaymentVoucher(PaymentVoucher paymentVoucher, CashBank cashBank, IAccountService _accountService,
                                    IPaymentVoucherDetailService _paymentVoucherDetailService, IPayableService _payableService, IGLNonBaseCurrencyService _gLNonBaseCurrencyService, ICurrencyService _currencyService);
        IList<GeneralLedgerJournal> CreateUnconfirmationJournalForPaymentVoucher(PaymentVoucher paymentVoucher, CashBank cashBank, IAccountService _accountService,
                                    IPaymentVoucherDetailService _paymentVoucherDetailService, IPayableService _payableService, IGLNonBaseCurrencyService _gLNonBaseCurrencyService, ICurrencyService _currencyService);
        IList<GeneralLedgerJournal> CreateReconcileJournalForPaymentVoucher(PaymentVoucher paymenttVoucher, CashBank cashBank, IAccountService _accountService, IGLNonBaseCurrencyService _gLNonBaseCurrencyService, ICurrencyService _currencyService);
        IList<GeneralLedgerJournal> CreateUnReconcileJournalForPaymentVoucher(PaymentVoucher paymenttVoucher, CashBank cashBank, IAccountService _accountService, IGLNonBaseCurrencyService _gLNonBaseCurrencyService, ICurrencyService _currencyService);
        IList<GeneralLedgerJournal> CreateConfirmationJournalForPaymentRequest(PaymentRequest paymentRequest, IPaymentRequestDetailService _paymentRequestDetailService, IAccountService _accountService,
                                    IGLNonBaseCurrencyService _gLNonBaseCurrencyService, ICurrencyService _currencyService);
        IList<GeneralLedgerJournal> CreateUnconfirmationJournalForPaymentRequest(PaymentRequest paymentRequest, IPaymentRequestDetailService _paymentRequestDetailService, IAccountService _accountService,
                                    IGLNonBaseCurrencyService _gLNonBaseCurrencyService, ICurrencyService _currencyService);
        IList<GeneralLedgerJournal> CreateConfirmationJournalForPurchaseDownPayment(PurchaseDownPayment purchaseDownPayment, IAccountService _accountService,
                                    ICurrencyService _currencyService, IGLNonBaseCurrencyService _glNonBaseCurrencyService);
        IList<GeneralLedgerJournal> CreateUnconfirmationJournalForPurchaseDownPayment(PurchaseDownPayment purchaseDownPayment, IAccountService _accountService,
                                    ICurrencyService _currencyService, IGLNonBaseCurrencyService _glNonBaseCurrencyService);
        IList<GeneralLedgerJournal> CreateConfirmationJournalForPurchaseDownPaymentAllocation(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation, IAccountService _accountService,
                                    IPurchaseDownPaymentService _purchaseDownPaymentService, IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService,
                                    IPayableService _payableService, IReceivableService _receivableService);
        IList<GeneralLedgerJournal> CreateUnconfirmationJournalForPurchaseDownPaymentAllocation(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation, IAccountService _accountService,
                                    IPurchaseDownPaymentService _purchaseDownPaymentService, IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService,
                                    IPayableService _payableService, IReceivableService _receivableService);
        IList<GeneralLedgerJournal> CreateConfirmationJournalForPurchaseAllowance(PurchaseAllowance purchaseAllowance, CashBank cashBank, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateUnconfirmationJournalForPurchaseAllowance(PurchaseAllowance purchaseAllowance, CashBank cashBank, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateConfirmationJournalForReceiptVoucher(ReceiptVoucher receiptVoucher, CashBank cashBank, IAccountService _accountService,
                                    IReceiptVoucherDetailService _receiptVoucherDetailService, IReceivableService _receivableService, IGLNonBaseCurrencyService _gLNonBaseCurrencyService,
                                    ICurrencyService _currencyService);
        IList<GeneralLedgerJournal> CreateUnconfirmationJournalForReceiptVoucher(ReceiptVoucher receiptVoucher, CashBank cashBank, IAccountService _accountService,
                                    IReceiptVoucherDetailService _receiptVoucherDetailService, IReceivableService _receivableService, IGLNonBaseCurrencyService _gLNonBaseCurrencyService,
                                    ICurrencyService _currencyService);
        IList<GeneralLedgerJournal> CreateReconcileJournalForReceiptVoucher(ReceiptVoucher receiptVoucher, CashBank cashBank, IAccountService _accountService,
                                    IGLNonBaseCurrencyService _gLNonBaseCurrencyService, ICurrencyService _currencyService);
        IList<GeneralLedgerJournal> CreateUnReconcileJournalForReceiptVoucher(ReceiptVoucher receiptVoucher, CashBank cashBank, IAccountService _accountService,
                                    IGLNonBaseCurrencyService _gLNonBaseCurrencyService, ICurrencyService _currencyService);
        IList<GeneralLedgerJournal> CreateConfirmationJournalForSalesDownPayment(SalesDownPayment salesDownPayment, IAccountService _accountService,
                                    ICurrencyService _currencyService, IGLNonBaseCurrencyService _glNonBaseCurrencyService);
        IList<GeneralLedgerJournal> CreateUnconfirmationJournalForSalesDownPayment(SalesDownPayment salesDownPayment, IAccountService _accountService,
                                    ICurrencyService _currencyService, IGLNonBaseCurrencyService _glNonBaseCurrencyService);
        IList<GeneralLedgerJournal> CreateConfirmationJournalForSalesDownPaymentAllocation(SalesDownPaymentAllocation salesDownPaymentAllocation, IAccountService _accountService,
                                    ISalesDownPaymentService _salesDownPaymentService, ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService,
                                    IPayableService _payableService, IReceivableService _receivableService);
        IList<GeneralLedgerJournal> CreateUnconfirmationJournalForSalesDownPaymentAllocation(SalesDownPaymentAllocation salesDownPaymentAllocation, IAccountService _accountService,
                                    ISalesDownPaymentService _salesDownPaymentService, ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService,
                                    IPayableService _payableService, IReceivableService _receivableService);
        IList<GeneralLedgerJournal> CreateConfirmationJournalForSalesAllowance(SalesAllowance salesAllowance, CashBank cashBank, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateUnconfirmationJournalForSalesAllowance(SalesAllowance salesAllowance, CashBank cashBank, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateConfirmationJournalForCashBankAdjustment(CashBankAdjustment cashBankAdjustment, CashBank cashBank, IAccountService _accountService, ICurrencyService _currencyService, IGLNonBaseCurrencyService _gLNonBaseCurrencyService);
        IList<GeneralLedgerJournal> CreateUnconfirmationJournalForCashBankAdjustment(CashBankAdjustment cashBankAdjustment, CashBank cashBank, IAccountService _accountService, ICurrencyService _currencyService, IGLNonBaseCurrencyService _gLNonBaseCurrencyService);
        IList<GeneralLedgerJournal> CreateConfirmationJournalForCashBankMutation(CashBankMutation cashBankMutation, CashBank sourceCashBank, CashBank targetCashBank, IAccountService _accountService,
                                    ICurrencyService _currencyService, IGLNonBaseCurrencyService _glNonBaseCurrencyService);
        IList<GeneralLedgerJournal> CreateUnconfirmationJournalForCashBankMutation(CashBankMutation cashBankMutation, CashBank sourceCashBank, CashBank targetCashBank, IAccountService _accountService,
                                    ICurrencyService _currencyService, IGLNonBaseCurrencyService _glNonBaseCurrencyService);
        IList<GeneralLedgerJournal> CreateConfirmationJournalForStockAdjustment(StockAdjustment stockAdjustment, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateUnconfirmationJournalForStockAdjustment(StockAdjustment stockAdjustment, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateConfirmationJournalForCustomerStockAdjustment(CustomerStockAdjustment customerStockAdjustment, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateUnconfirmationJournalForCustomerStockAdjustment(CustomerStockAdjustment customerStockAdjustment, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateConfirmationJournalForPurchaseReceival(PurchaseReceival purchaseReceival, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateUnconfirmationJournalForPurchaseReceival(PurchaseReceival purchaseReceival, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateJournalForPurchaseInvoiceMigration(PurchaseInvoiceMigration purchaseInvoiceMigration, IAccountService _accountService,  ICurrencyService _currencyService, IGLNonBaseCurrencyService _gLNonBaseCurrencyService);
        IList<GeneralLedgerJournal> CreateConfirmationJournalForPurchaseInvoice(PurchaseInvoice purchaseInvoice, PurchaseReceival purchaseReceival, IAccountService _accountService,
                                    IGLNonBaseCurrencyService _gLNonBaseCurrencyService, ICurrencyService _currencyService);
        IList<GeneralLedgerJournal> CreateUnconfirmationJournalForPurchaseInvoice(PurchaseInvoice purchaseInvoice, PurchaseReceival purchaseReceival, IAccountService _accountService,
                                    IGLNonBaseCurrencyService _gLNonBaseCurrencyService, ICurrencyService _currencyService);
        IList<GeneralLedgerJournal> CreateConfirmationJournalForDeliveryOrder(DeliveryOrder deliveryOrder, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateUnconfirmationJournalForDeliveryOrder(DeliveryOrder deliveryOrder, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateReconciliationJournalForTemporaryDeliveryOrderWaste(TemporaryDeliveryOrder temporaryDeliveryOrder, DateTime PushDate, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateUnreconciliationJournalForTemporaryDeliveryOrderWaste(TemporaryDeliveryOrder temporaryDeliveryOrder, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateConfirmationJournalForTemporaryDeliveryOrderClearanceWaste(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateUnconfirmationJournalForTemporaryDeliveryOrderClearanceWaste(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateJournalForSalesInvoiceMigration(SalesInvoiceMigration salesInvoiceMigration, IAccountService _accountService, ICurrencyService _currencyService, IGLNonBaseCurrencyService _gLNonBaseCurrencyService);
        IList<GeneralLedgerJournal> CreateConfirmationJournalForSalesInvoice(SalesInvoice salesInvoice, Contact contact, IAccountService _accountService, IExchangeRateService _exchangeRateService, ICurrencyService _currencyService, IGLNonBaseCurrencyService _gLNonBaseCurrencyService);
        IList<GeneralLedgerJournal> CreateUnconfirmationJournalForSalesInvoice(SalesInvoice salesInvoice, Contact contact, IAccountService _accountService, IExchangeRateService _exchangeRateService, ICurrencyService _currencyService, IGLNonBaseCurrencyService _gLNonBaseCurrencyService);
        IList<GeneralLedgerJournal> CreateFinishedJournalForRecoveryOrderDetail(RecoveryOrderDetail recoveryOrderDetail, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateUnfinishedJournalForRecoveryOrderDetail(RecoveryOrderDetail recoveryOrderDetail, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateRejectedJournalForRecoveryOrderDetail(RecoveryOrderDetail recoveryOrderDetail, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateUndoRejectedJournalForRecoveryOrderDetail(RecoveryOrderDetail recoveryOrderDetail, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateFinishedJournalForBlanketOrderDetail(BlanketOrderDetail blanketOrderDetail, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateUnfinishedJournalForBlanketOrderDetail(BlanketOrderDetail blanketOrderDetail, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateRejectedJournalForBlanketOrderDetail(BlanketOrderDetail blanketOrderDetail, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateUndoRejectedJournalForBlanketOrderDetail(BlanketOrderDetail blanketOrderDetail, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateConfirmationJournalForBlendingWorkOrder(BlendingWorkOrder blendingWorkOrder, IAccountService _accountService, decimal TotalCost);
        IList<GeneralLedgerJournal> CreateUnconfirmationJournalForBlendingWorkOrder(BlendingWorkOrder blendingWorkOrder, IAccountService _accountService, decimal TotalCost);
    }
}