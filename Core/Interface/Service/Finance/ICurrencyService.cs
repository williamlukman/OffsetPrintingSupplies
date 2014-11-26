using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface ICurrencyService
    {
        IQueryable<Currency> GetQueryable();
        ICurrencyValidator GetValidator();
        IList<Currency> GetAll();
        Currency GetObjectById(int Id);
        Currency GetObjectByName(string Name);
        Currency CreateObject(Currency currency, IAccountService _accountService);
        Currency UpdateObject(Currency currency, ICashBankService _cashBankService, IPaymentRequestService _paymentRequestService
            , IPurchaseOrderService _purchaseOrderService, ISalesOrderService _salesOrderService, ISalesInvoiceService _salesInvoiceService
            , IPurchaseInvoiceService _purchaseInvoiceService, IPayableService _payableService, IReceivableService _receivableService
            , IPaymentVoucherService _paymentVoucherService, IReceiptVoucherService _receiptVoucherService);
        Currency SoftDeleteObject(Currency currency, ICashBankService _cashBankService, IPaymentRequestService _paymentRequestService
            , IPurchaseOrderService _purchaseOrderService, ISalesOrderService _salesOrderService, ISalesInvoiceService _salesInvoiceService
            , IPurchaseInvoiceService _purchaseInvoiceService, IPayableService _payableService, IReceivableService _receivableService
            , IPaymentVoucherService _paymentVoucherService, IReceiptVoucherService _receiptVoucherService);
        bool DeleteObject(int Id);
        bool IsNameDuplicated(Currency currency);
        string GenerateAccountCode(IAccountService _accountService);
    }
}