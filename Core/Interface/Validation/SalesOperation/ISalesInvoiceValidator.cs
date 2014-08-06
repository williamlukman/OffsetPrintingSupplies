using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface ISalesInvoiceValidator
    {
        SalesInvoice VContact(SalesInvoice salesInvoice, ICustomerService _customerService);
        SalesInvoice VHasSalesInvoiceDetails(SalesInvoice salesInvoice, ISalesInvoiceDetailService _salesInvoiceDetailService);
        SalesInvoice VHasReceipt(SalesInvoice salesInvoice, IReceivableService _receivableService, IReceiptVoucherDetailService _receiptVoucherDetailService);
        SalesInvoice VIsConfirmed(SalesInvoice salesInvoice);
        SalesInvoice VCreateObject(SalesInvoice salesInvoice, ICustomerService _customerService);
        SalesInvoice VUpdateObject(SalesInvoice salesInvoice, ISalesInvoiceDetailService _salesInvoiceDetailService, ICustomerService _customerService);
        SalesInvoice VDeleteObject(SalesInvoice salesInvoice);
        SalesInvoice VConfirmObject(SalesInvoice salesInvoice, ISalesInvoiceDetailService _salesInvoiceDetailServices, IDeliveryOrderDetailService _deliveryOrderDetailService);
        SalesInvoice VUnconfirmObject(SalesInvoice salesInvoice, ISalesInvoiceDetailService _salesInvoiceDetailServices, IReceiptVoucherDetailService _rvds, IReceivableService _receivableService);
        bool ValidCreateObject(SalesInvoice salesInvoice, ICustomerService _customerService);
        bool ValidUpdateObject(SalesInvoice salesInvoice, ISalesInvoiceDetailService _salesInvoiceDetailService, ICustomerService  _customerService);
        bool ValidDeleteObject(SalesInvoice salesInvoice);
        bool ValidConfirmObject(SalesInvoice salesInvoice, ISalesInvoiceDetailService _salesInvoiceDetailServices, IDeliveryOrderDetailService _deliveryOrderDetailService);
        bool ValidUnconfirmObject(SalesInvoice salesInvoice, ISalesInvoiceDetailService _salesInvoiceDetailServices, IReceiptVoucherDetailService _rvds, IReceivableService _receivableService);
        bool isValid(SalesInvoice salesInvoice);
        string PrintError(SalesInvoice salesInvoice);
    }
}
