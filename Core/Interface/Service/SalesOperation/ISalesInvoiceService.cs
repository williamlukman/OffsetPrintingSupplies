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
        IList<SalesInvoice> GetObjectsByDeliveryOrderId(int deliveryOrderId);
        SalesInvoice CreateObject(SalesInvoice salesInvoice, IDeliveryOrderService _deliveryOrderService);
        SalesInvoice CreateObject(int deliveryOrderId, string description, int discount, bool IsTaxable, DateTime InvoiceDate, DateTime DueDate, IDeliveryOrderService _deliveryOrderService);
        SalesInvoice UpdateObject(SalesInvoice salesInvoice, IDeliveryOrderService _deliveryOrderService, ISalesInvoiceDetailService _salesInvoiceDetailService);
        SalesInvoice SoftDeleteObject(SalesInvoice salesInvoice, ISalesInvoiceDetailService _salesInvoiceDetailService);
        bool DeleteObject(int Id);
        SalesInvoice ConfirmObject(SalesInvoice salesInvoice, DateTime ConfirmationDate, ISalesInvoiceDetailService _salesInvoiceDetailService, ISalesOrderService _salesOrderService,
                                      IDeliveryOrderService _deliveryOrderService, IDeliveryOrderDetailService _deliveryOrderDetailService, IReceivableService _receivableService);
        SalesInvoice UnconfirmObject(SalesInvoice salesInvoice, ISalesInvoiceDetailService _salesInvoiceDetailService,
                                        IDeliveryOrderService _deliveryOrderService, IDeliveryOrderDetailService _deliveryOrderDetailService,
                                        IReceiptVoucherDetailService _receiptVoucherDetailService, IReceivableService _receivableService);
        SalesInvoice CalculateAmountReceivable(SalesInvoice salesInvoice, ISalesInvoiceDetailService _salesInvoiceDetailService);
    }
}