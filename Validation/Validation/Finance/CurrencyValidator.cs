using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Interface.Validation;
using Core.DomainModel;
using Core.Interface.Service;

namespace Validation.Validation
{
    public class CurrencyValidator : ICurrencyValidator
    {

        public Currency VName(Currency currency, ICurrencyService _currencyService)
        {
            if (String.IsNullOrEmpty(currency.Name) || currency.Name.Trim() == "")
            {
                currency.Errors.Add("Name", "Tidak boleh kosong");
            }
            if (_currencyService.IsNameDuplicated(currency))
            {
                currency.Errors.Add("Name", "Tidak boleh ada duplikasi");
            }
            return currency;
        }
         
        public Currency VIsBase(Currency currency, ICurrencyService _currencyService)
        {
            if (_currencyService.GetObjectById(currency.Id).IsBase == true)
            {
                currency.Errors.Add("Generic", "Cannot Edit Base Currency");
            }
            return currency;
        }

        public Currency VAlreadyUsed(Currency currency, ICashBankService _cashBankService,IPaymentRequestService _paymentRequestService
            ,IPurchaseOrderService _purchaseOrderService,ISalesOrderService _salesOrderService,ISalesInvoiceService _salesInvoiceService
            ,IPurchaseInvoiceService _purchaseInvoiceService,IPayableService _payableService,IReceivableService _receivableService
            ,IPaymentVoucherService _paymentVoucherService,IReceiptVoucherService _receiptVoucherService)
        {
            if (_cashBankService.GetQueryable().Where(x => x.CurrencyId == currency.Id).Any())
            {
                currency.Errors.Add("Generic", "Currency telah terpakai di CashBank");
            }
            if (_paymentRequestService.GetQueryable().Where(x => x.CurrencyId == currency.Id).Any())
            {
                currency.Errors.Add("Generic", "Currency telah terpakai di PaymentRequest");
            }
            if (_purchaseOrderService.GetQueryable().Where(x => x.CurrencyId == currency.Id).Any())
            {
                currency.Errors.Add("Generic", "Currency telah terpakai di PurchaseOrder");
            }
            if (_salesOrderService.GetQueryable().Where(x => x.CurrencyId == currency.Id).Any())
            {
                currency.Errors.Add("Generic", "Currency telah terpakai di SalesOrder");
            }
            if (_salesInvoiceService.GetQueryable().Where(x => x.CurrencyId == currency.Id).Any())
            {
                currency.Errors.Add("Generic", "Currency telah terpakai di SalesInvoice");
            }
            if (_purchaseInvoiceService.GetQueryable().Where(x => x.CurrencyId == currency.Id).Any())
            {
                currency.Errors.Add("Generic", "Currency telah terpakai di PurchaseInvoice");
            }
            if (_payableService.GetQueryable().Where(x => x.CurrencyId == currency.Id).Any())
            {
                currency.Errors.Add("Generic", "Currency telah terpakai di Payable");
            } 
            if (_receivableService.GetQueryable().Where(x => x.CurrencyId == currency.Id).Any())
            {
                currency.Errors.Add("Generic", "Currency telah terpakai di Receivable");
            }
            if (_paymentVoucherService.GetQueryable().Where(x => x.CurrencyId == currency.Id).Any())
            {
                currency.Errors.Add("Generic", "Currency telah terpakai di PaymentVoucher");
            }
            if (_receiptVoucherService.GetQueryable().Where(x => x.CurrencyId == currency.Id).Any())
            {
                currency.Errors.Add("Generic", "Currency telah terpakai di ReceiptVoucher");
            }
            return currency;
        }


        public Currency VCreateObject(Currency currency, ICurrencyService _currencyService)
        {
            VName(currency, _currencyService);
            if (!isValid(currency)) { return currency; }
            return currency;
        }

        public Currency VUpdateObject(Currency currency, ICurrencyService _currencyService, ICashBankService _cashBankService, IPaymentRequestService _paymentRequestService
            , IPurchaseOrderService _purchaseOrderService, ISalesOrderService _salesOrderService, ISalesInvoiceService _salesInvoiceService
            , IPurchaseInvoiceService _purchaseInvoiceService, IPayableService _payableService, IReceivableService _receivableService
            , IPaymentVoucherService _paymentVoucherService, IReceiptVoucherService _receiptVoucherService)
        {
            VIsBase(currency, _currencyService);
            if (!isValid(currency)) { return currency; }
            VAlreadyUsed(currency,_cashBankService,_paymentRequestService
            , _purchaseOrderService, _salesOrderService, _salesInvoiceService
            , _purchaseInvoiceService, _payableService, _receivableService
            , _paymentVoucherService, _receiptVoucherService);
            if (!isValid(currency)) { return currency; }
            VName(currency, _currencyService);
            if (!isValid(currency)) { return currency; }
            return currency;
        }

        public Currency VDeleteObject(Currency currency, ICurrencyService _currencyService ,ICashBankService _cashBankService, IPaymentRequestService _paymentRequestService
            , IPurchaseOrderService _purchaseOrderService, ISalesOrderService _salesOrderService, ISalesInvoiceService _salesInvoiceService
            , IPurchaseInvoiceService _purchaseInvoiceService, IPayableService _payableService, IReceivableService _receivableService
            , IPaymentVoucherService _paymentVoucherService, IReceiptVoucherService _receiptVoucherService)
        {
            VIsBase(currency, _currencyService);
            if (!isValid(currency)) { return currency; }
            if (!isValid(currency)) { return currency; }
            VAlreadyUsed(currency, _cashBankService, _paymentRequestService
            , _purchaseOrderService, _salesOrderService, _salesInvoiceService
            , _purchaseInvoiceService, _payableService, _receivableService
            , _paymentVoucherService, _receiptVoucherService);
            if (!isValid(currency)) { return currency; }
            return currency;
        }

        public Currency VAdjustAmount(Currency currency)
        {
            return currency;
        }

        public bool ValidCreateObject(Currency currency, ICurrencyService _currencyService)
        {
            VCreateObject(currency, _currencyService);
            return isValid(currency);
        }

        public bool ValidUpdateObject(Currency currency, ICurrencyService _currencyService, ICashBankService _cashBankService, IPaymentRequestService _paymentRequestService
            , IPurchaseOrderService _purchaseOrderService, ISalesOrderService _salesOrderService, ISalesInvoiceService _salesInvoiceService
            , IPurchaseInvoiceService _purchaseInvoiceService, IPayableService _payableService, IReceivableService _receivableService
            , IPaymentVoucherService _paymentVoucherService, IReceiptVoucherService _receiptVoucherService)
        {
            currency.Errors.Clear();
            VUpdateObject(currency, _currencyService,_cashBankService, _paymentRequestService
            , _purchaseOrderService, _salesOrderService, _salesInvoiceService
            , _purchaseInvoiceService, _payableService, _receivableService
            , _paymentVoucherService, _receiptVoucherService);
            return isValid(currency);
        }

        public bool ValidDeleteObject(Currency currency, ICurrencyService _currencyService, ICashBankService _cashBankService, IPaymentRequestService _paymentRequestService
            , IPurchaseOrderService _purchaseOrderService, ISalesOrderService _salesOrderService, ISalesInvoiceService _salesInvoiceService
            , IPurchaseInvoiceService _purchaseInvoiceService, IPayableService _payableService, IReceivableService _receivableService
            , IPaymentVoucherService _paymentVoucherService, IReceiptVoucherService _receiptVoucherService)
        {
            currency.Errors.Clear();
            VDeleteObject(currency, _currencyService,_cashBankService, _paymentRequestService
            , _purchaseOrderService, _salesOrderService, _salesInvoiceService
            , _purchaseInvoiceService, _payableService, _receivableService
            , _paymentVoucherService, _receiptVoucherService);
            return isValid(currency);
        }

        public bool isValid(Currency obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(Currency obj)
        {
            string erroroutput = "";
            KeyValuePair<string, string> first = obj.Errors.ElementAt(0);
            erroroutput += first.Key + "," + first.Value;
            foreach (KeyValuePair<string, string> pair in obj.Errors.Skip(1))
            {
                erroroutput += Environment.NewLine;
                erroroutput += pair.Key + "," + pair.Value;
            }
            return erroroutput;
        }

    }
}
