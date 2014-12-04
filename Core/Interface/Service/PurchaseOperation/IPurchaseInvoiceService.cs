using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IPurchaseInvoiceService
    {
        IPurchaseInvoiceValidator GetValidator();
        IList<PurchaseInvoice> GetAll();
        IQueryable<PurchaseInvoice> GetQueryable();
        PurchaseInvoice GetObjectById(int Id);
        IList<PurchaseInvoice> GetObjectsByPurchaseReceivalId(int purchaseReceivalId);
        PurchaseInvoice CreateObject(PurchaseInvoice purchaseInvoice, IPurchaseReceivalService _purchaseReceivalService);
        PurchaseInvoice UpdateObject(PurchaseInvoice purchaseInvoice, IPurchaseReceivalService _purchaseReceivalService, IPurchaseInvoiceDetailService _purchaseInvoiceDetailService);
        PurchaseInvoice SoftDeleteObject(PurchaseInvoice purchaseInvoice, IPurchaseInvoiceDetailService _purchaseInvoiceDetailService);
        bool DeleteObject(int Id);
        PurchaseInvoice ConfirmObject(PurchaseInvoice purchaseInvoice, DateTime ConfirmationDate, IPurchaseInvoiceDetailService _purchaseInvoiceDetailService, IPurchaseOrderService _purchaseOrderService,
                                      IPurchaseReceivalService _purchaseReceivalService, IPurchaseReceivalDetailService _purchaseReceivalDetailService, IPayableService _payableService,
                                      IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService,
                                      ICurrencyService _currencyService, IExchangeRateService _exchangeRateService, IGLNonBaseCurrencyService _gLNonBaseCurrencyService);
        PurchaseInvoice UnconfirmObject(PurchaseInvoice purchaseInvoice, IPurchaseInvoiceDetailService _purchaseInvoiceDetailService,
                                        IPurchaseReceivalService _purchaseReceivalService, IPurchaseReceivalDetailService _purchaseReceivalDetailService,
                                        IPaymentVoucherDetailService _paymentVoucherDetailService, IPayableService _payableService,
                                        IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService, IGLNonBaseCurrencyService _gLNonBaseCurrencyService);
        PurchaseInvoice CalculateAmountPayable(PurchaseInvoice purchaseInvoice, IPurchaseInvoiceDetailService _purchaseInvoiceDetailService);
    }
}