using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface ICurrencyValidator
    {
        Currency VCreateObject(Currency currency, ICurrencyService _currencyService);
        Currency VUpdateObject(Currency currency, ICurrencyService _currencyService, ICashBankService _cashBankService, IPaymentRequestService _paymentRequestService
            , IPurchaseOrderService _purchaseOrderService, ISalesOrderService _salesOrderService, ISalesInvoiceService _salesInvoiceService
            , IPurchaseInvoiceService _purchaseInvoiceService, IPayableService _payableService, IReceivableService _receivableService
            , IPaymentVoucherService _paymentVoucherService, IReceiptVoucherService _receiptVoucherService);
        Currency VDeleteObject(Currency currency, ICurrencyService _currencyService, ICashBankService _cashBankService, IPaymentRequestService _paymentRequestService
            , IPurchaseOrderService _purchaseOrderService, ISalesOrderService _salesOrderService, ISalesInvoiceService _salesInvoiceService
            , IPurchaseInvoiceService _purchaseInvoiceService, IPayableService _payableService, IReceivableService _receivableService
            , IPaymentVoucherService _paymentVoucherService, IReceiptVoucherService _receiptVoucherService);
        bool ValidCreateObject(Currency currency, ICurrencyService _currencyService);
        bool ValidUpdateObject(Currency currency, ICurrencyService _currencyService, ICashBankService _cashBankService, IPaymentRequestService _paymentRequestService
            , IPurchaseOrderService _purchaseOrderService, ISalesOrderService _salesOrderService, ISalesInvoiceService _salesInvoiceService
            , IPurchaseInvoiceService _purchaseInvoiceService, IPayableService _payableService, IReceivableService _receivableService
            , IPaymentVoucherService _paymentVoucherService, IReceiptVoucherService _receiptVoucherService);
        bool ValidDeleteObject(Currency currency, ICurrencyService _currencyService, ICashBankService _cashBankService, IPaymentRequestService _paymentRequestService
            , IPurchaseOrderService _purchaseOrderService, ISalesOrderService _salesOrderService, ISalesInvoiceService _salesInvoiceService
            , IPurchaseInvoiceService _purchaseInvoiceService, IPayableService _payableService, IReceivableService _receivableService
            , IPaymentVoucherService _paymentVoucherService, IReceiptVoucherService _receiptVoucherService);
        bool isValid(Currency currency);
        string PrintError(Currency currency);
    }
}
