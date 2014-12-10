using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IPurchaseInvoiceValidator
    {
        PurchaseInvoice VHasUniqueNomorSurat(PurchaseInvoice purchaseInvoice, IPurchaseInvoiceService _purchaseInvoiceService);
        PurchaseInvoice VHasPurchaseReceival(PurchaseInvoice purchaseInvoice, IPurchaseReceivalService _purchaseReceivalService);
        PurchaseInvoice VHasNoPurchaseInvoiceDetails(PurchaseInvoice purchaseInvoice, IPurchaseInvoiceDetailService _purchaseInvoiceDetailService);
        PurchaseInvoice VHasPurchaseInvoiceDetails(PurchaseInvoice purchaseInvoice, IPurchaseInvoiceDetailService _purchaseInvoiceDetailService);
        PurchaseInvoice VPurchaseReceivalHasNotBeenInvoiceCompleted(PurchaseInvoice purchaseInvoice, IPurchaseReceivalService _purchaseReceivalService);
        PurchaseInvoice VHasInvoiceDate(PurchaseInvoice purchaseInvoice);
        PurchaseInvoice VHasDueDate(PurchaseInvoice purchaseInvoice);
        PurchaseInvoice VHasDiscountBetweenZeroAndHundred(PurchaseInvoice purchaseInvoice);
        PurchaseInvoice VHasPaymentVoucherDetails(PurchaseInvoice purchaseInvoice, IPayableService _payableService, IPaymentVoucherDetailService _paymentVoucherDetailService);
        PurchaseInvoice VHasNotBeenConfirmed(PurchaseInvoice purchaseInvoice);
        PurchaseInvoice VHasBeenConfirmed(PurchaseInvoice purchaseInvoice);
        PurchaseInvoice VHasNotBeenDeleted(PurchaseInvoice purchaseInvoice);
        PurchaseInvoice VAllPurchaseInvoiceDetailsAreConfirmable(PurchaseInvoice purchaseInvoice, IPurchaseInvoiceDetailService _purchaseInvoiceDetailService, IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        PurchaseInvoice VAllPurchaseInvoiceDetailsAreUnconfirmable(PurchaseInvoice purchaseInvoice, IPurchaseInvoiceDetailService _purchaseInvoiceDetailService, IPaymentVoucherDetailService _paymentVoucherDetailService, IPayableService _payableService);
        PurchaseInvoice VPayableHasNoOtherAssociation(PurchaseInvoice purchaseInvoice, IPayableService _payableService, IPaymentVoucherDetailService _paymentVoucherDetailService);
        PurchaseInvoice VGeneralLedgerPostingHasNotBeenClosed(PurchaseInvoice purchaseInvoice, IClosingService _closingService);
        PurchaseInvoice VCreateObject(PurchaseInvoice purchaseInvoice, IPurchaseInvoiceService _purchaseInvoiceService, IPurchaseReceivalService _purchaseReceivalService);
        PurchaseInvoice VUpdateObject(PurchaseInvoice purchaseInvoice, IPurchaseInvoiceService _purchaseInvoiceService, IPurchaseReceivalService _purchaseReceivalService, IPurchaseInvoiceDetailService _purchaseInvoiceDetailService);
        PurchaseInvoice VDeleteObject(PurchaseInvoice purchaseInvoice, IPurchaseInvoiceDetailService _purchaseInvoiceDetailService);
        PurchaseInvoice VHasConfirmationDate(PurchaseInvoice purchaseInvoice);
        PurchaseInvoice VConfirmObject(PurchaseInvoice purchaseInvoice, IPurchaseInvoiceDetailService _purchaseInvoiceDetailService,
                                       IPurchaseReceivalService _purchaseReceivalService, IPurchaseReceivalDetailService _purchaseReceivalDetailService, IClosingService _closingService,
                                       IExchangeRateService _exchangeRateService, ICurrencyService _currencyService);
        PurchaseInvoice VUnconfirmObject(PurchaseInvoice purchaseInvoice, IPurchaseInvoiceDetailService _purchaseInvoiceDetailService, IPaymentVoucherDetailService _paymentVoucherDetailService,
                                         IPayableService _payableService, IClosingService _closingService);
        bool ValidCreateObject(PurchaseInvoice purchaseInvoice, IPurchaseInvoiceService _purchaseInvoiceService, IPurchaseReceivalService _purchaseReceivalService);
        bool ValidUpdateObject(PurchaseInvoice purchaseInvoice, IPurchaseInvoiceService _purchaseInvoiceService, IPurchaseReceivalService _purchaseReceivalService, IPurchaseInvoiceDetailService _purchaseInvoiceDetailService);
        bool ValidDeleteObject(PurchaseInvoice purchaseInvoice, IPurchaseInvoiceDetailService _purchaseInvoiceDetailService);
        bool ValidConfirmObject(PurchaseInvoice purchaseInvoice, IPurchaseInvoiceDetailService _purchaseInvoiceDetailService,
                                IPurchaseReceivalService _purchaseReceivalService, IPurchaseReceivalDetailService _purchaseReceivalDetailService,
                                IClosingService _closingService, IExchangeRateService _exchangeRateService, ICurrencyService _currencyService);
        bool ValidUnconfirmObject(PurchaseInvoice purchaseInvoice, IPurchaseInvoiceDetailService _purchaseInvoiceDetailService, IPaymentVoucherDetailService _paymentVoucherDetailService,
                                  IPayableService _payableService, IClosingService _closingService);
        bool isValid(PurchaseInvoice purchaseInvoice);
        string PrintError(PurchaseInvoice purchaseInvoice);
    }
}
