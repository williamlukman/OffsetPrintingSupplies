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
        PurchaseInvoice GetObjectById(int Id);
        IList<PurchaseInvoice> GetObjectsByContactId(int contactId);
        PurchaseInvoice CreateObject(PurchaseInvoice purchaseInvoice, ICustomerService _customerService);
        PurchaseInvoice CreateObject(int customerId, string description, decimal totalAmount, ICustomerService _customerService);
        PurchaseInvoice UpdateObject(PurchaseInvoice purchaseInvoice, IPurchaseInvoiceDetailService _purchaseInvoiceDetailService, ICustomerService _customerService);
        PurchaseInvoice SoftDeleteObject(PurchaseInvoice purchaseInvoice);
        bool DeleteObject(int Id);
        PurchaseInvoice ConfirmObject(PurchaseInvoice purchaseInvoice, IPurchaseInvoiceDetailService _purchaseInvoiceDetailService,
                                      IPurchaseReceivalDetailService _purchaseReceivalDetailService, IPayableService _payableService);
        PurchaseInvoice UnconfirmObject(PurchaseInvoice purchaseInvoice, IPurchaseInvoiceDetailService _purchaseInvoiceDetailService,
                                        IPaymentVoucherDetailService _paymentVoucherDetailService, IPayableService _payableService);
    }
}