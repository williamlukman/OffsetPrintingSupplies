using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IReceiptVoucherService
    {
        IReceiptVoucherValidator GetValidator();
        IQueryable<ReceiptVoucher> GetQueryable();
        IList<ReceiptVoucher> GetAll();
        ReceiptVoucher GetObjectById(int Id);
        IList<ReceiptVoucher> GetObjectsByCashBankId(int cashBankId);
        IList<ReceiptVoucher> GetObjectsByContactId(int contactId);
        ReceiptVoucher CreateObject(ReceiptVoucher receiptVoucher, IReceiptVoucherDetailService _receiptVoucherDetailService, IReceivableService _receivableService,
                                    IContactService _contactService, ICashBankService _cashBankService, ICurrencyService _currencyService);
        ReceiptVoucher CreateObject(int cashBankId, int contactId, DateTime receiptDate, decimal totalAmount, bool IsGBCH, DateTime DueDate, bool IsBank,
                                    IReceiptVoucherDetailService _receiptVoucherDetailService, IReceivableService _receivableService,
                                    IContactService _contactService, ICashBankService _cashBankService, ICurrencyService _currencyService);
        ReceiptVoucher UpdateAmount(ReceiptVoucher receiptVoucher);
        ReceiptVoucher UpdateObject(ReceiptVoucher receiptVoucher, IReceiptVoucherDetailService _receiptVoucherDetailService, IReceivableService _receivableService,
                                    IContactService _contactService, ICashBankService _cashBankService);
        ReceiptVoucher SoftDeleteObject(ReceiptVoucher receiptVoucher, IReceiptVoucherDetailService _receiptVoucherDetailService);
        bool DeleteObject(int Id);
        ReceiptVoucher ConfirmObject(ReceiptVoucher receiptVoucher, DateTime ConfirmationDate, IReceiptVoucherDetailService _receiptVoucherDetailService,
                                     ICashBankService _cashBankService, IReceivableService _receivableService, ICashMutationService _cashMutationService,
                                     IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService,
                                     ICurrencyService _currencyService, IExchangeRateService _exchangeRateService, ISalesInvoiceService _salesInvoiceService, IGLNonBaseCurrencyService _gLNonBaseCurrencyService);
        ReceiptVoucher UnconfirmObject(ReceiptVoucher receiptVoucher, IReceiptVoucherDetailService _receiptVoucherDetailService,
                                       ICashBankService _cashBankService, IReceivableService _receivableService, ICashMutationService _cashMutationService,
                                       IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService,
                                       ICurrencyService _currencyService, IExchangeRateService _exchangeRateService, IGLNonBaseCurrencyService _gLNonBaseCurrencyService);
        ReceiptVoucher ReconcileObject(ReceiptVoucher receiptVoucher, DateTime ReconciliationDate, IReceiptVoucherDetailService _receiptVoucherDetailService,
                                       ICashMutationService _cashMutationService, ICashBankService _cashBankService, IReceivableService _receivableService,
                                       IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService,
                                       ICurrencyService _currencyService, IExchangeRateService _exchangeRateService, ISalesInvoiceService _salesInvoiceService, IGLNonBaseCurrencyService _gLNonBaseCurrencyService);
        ReceiptVoucher UnreconcileObject(ReceiptVoucher receiptVoucher, IReceiptVoucherDetailService _receiptVoucherDetailService,
                                         ICashMutationService _cashMutationService, ICashBankService _cashBankService, IReceivableService _receivableService,
                                         IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService,
                                         ICurrencyService _currencyService, IExchangeRateService _exchangeRateService, IGLNonBaseCurrencyService _gLNonBaseCurrencyService);
        ReceiptVoucher CalculateTotalAmount(ReceiptVoucher receiptVoucher, IReceiptVoucherDetailService _receiptVoucherDetailService);
    }
}