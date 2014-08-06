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
        PurchaseInvoice VContact(PurchaseInvoice purchaseInvoice, ICustomerService _customerService);
        PurchaseInvoice VHasPurchaseReceival(PurchaseInvoice purchaseInvoice, IPurchaseReceivalService _purchaseReceivalService);
        PurchaseInvoice VHasPurchaseInvoiceDetails(PurchaseInvoice purchaseInvoice, IPurchaseInvoiceDetailService _purchaseInvoiceDetailService);
        PurchaseInvoice VHasPayment(PurchaseInvoice purchaseInvoice, IPayableService _payableService, IPaymentVoucherDetailService _paymentVoucherDetailService);
        PurchaseInvoice VHasNotBeenConfirmed(PurchaseInvoice purchaseInvoice);
        PurchaseInvoice VHasBeenConfirmed(PurchaseInvoice purchaseInvoice);
        PurchaseInvoice VUpdateContactWithPurchaseInvoiceDetails(PurchaseInvoice purchaseInvoice, IPurchaseInvoiceService _purchaseInvoiceService, IPurchaseInvoiceDetailService _purchaseInvoiceDetailService);
        PurchaseInvoice VCreateObject(PurchaseInvoice purchaseInvoice, ICustomerService _customerService);
        PurchaseInvoice VUpdateObject(PurchaseInvoice purchaseInvoice, IPurchaseInvoiceService _purchaseInvoiceService, IPurchaseInvoiceDetailService _purchaseInvoiceDetailService, ICustomerService _customerService);
        PurchaseInvoice VDeleteObject(PurchaseInvoice purchaseInvoice);
        PurchaseInvoice VConfirmObject(PurchaseInvoice purchaseInvoice, IPurchaseInvoiceDetailService _purchaseInvoiceDetailService, IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        PurchaseInvoice VUnconfirmObject(PurchaseInvoice purchaseInvoice, IPurchaseInvoiceDetailService _purchaseInvoiceDetailService, IPaymentVoucherDetailService _paymentVoucherDetailService, IPayableService _payableService);
        bool ValidCreateObject(PurchaseInvoice purchaseInvoice, ICustomerService _customerService);
        bool ValidUpdateObject(PurchaseInvoice purchaseInvoice, IPurchaseInvoiceService _purchaseInvoiceService, IPurchaseInvoiceDetailService _purchaseInvoiceDetailService, ICustomerService _customerService);
        bool ValidDeleteObject(PurchaseInvoice purchaseInvoice);
        bool ValidConfirmObject(PurchaseInvoice purchaseInvoice, IPurchaseInvoiceDetailService _purchaseInvoiceDetailService, IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        bool ValidUnconfirmObject(PurchaseInvoice purchaseInvoice, IPurchaseInvoiceDetailService _purchaseInvoiceDetailService, IPaymentVoucherDetailService _paymentVoucherDetailService, IPayableService _payableService);
        bool isValid(PurchaseInvoice purchaseInvoice);
        string PrintError(PurchaseInvoice purchaseInvoice);
    }
}
