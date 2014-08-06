using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface ISalesInvoiceService
    {
        ISalesInvoiceValidator GetValidator();
        IList<SalesInvoice> GetAll();
        SalesInvoice GetObjectById(int Id);
        DeliveryOrder GetDeliveryOrderByDeliveryOrderId(int deliveryOrderId);
        IList<SalesInvoice> GetObjectsByContactId(int customerId);
        SalesInvoice CreateObject(SalesInvoice salesInvoice, ICustomerService _customerService);
        SalesInvoice CreateObject(int customerId, string description, decimal totalAmount, ICustomerService _customerService);
        SalesInvoice UpdateObject(SalesInvoice salesInvoice, ISalesInvoiceDetailService _salesInvoiceDetailService, ICustomerService _customerService);
        SalesInvoice SoftDeleteObject(SalesInvoice salesInvoice);
        bool DeleteObject(int Id);
        SalesInvoice ConfirmObject(SalesInvoice salesInvoice, ISalesInvoiceDetailService _salesInvoiceDetailService, IDeliveryOrderDetailService _deliveryOrderDetailService, IReceivableService _receivableService);
        SalesInvoice UnconfirmObject(SalesInvoice salesInvoice, ISalesInvoiceDetailService _salesInvoiceDetailService,
                                        IReceiptVoucherDetailService _receiptVoucherDetailService, IReceivableService _receivableService);
    }
}