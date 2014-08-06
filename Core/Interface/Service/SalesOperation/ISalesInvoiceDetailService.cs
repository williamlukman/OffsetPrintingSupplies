using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface ISalesInvoiceDetailService
    {
        ISalesInvoiceDetailValidator GetValidator();
        IList<SalesInvoiceDetail> GetObjectsBySalesInvoiceId(int salesInvoiceId);
        SalesInvoiceDetail GetObjectById(int Id);
        DeliveryOrderDetail GetDeliveryOrderDetailByDeliveryOrderDetailId(int deliveryOrderDetailId);
        SalesInvoiceDetail CreateObject(SalesInvoiceDetail salesInvoiceDetail, ISalesInvoiceService _salesInvoiceService, IDeliveryOrderDetailService _deliveryOrderDetailService);
        SalesInvoiceDetail CreateObject(int salesInvoiceId, int deliveryOrderDetailId, int quantity, decimal amount, ISalesInvoiceService _salesInvoiceService, IDeliveryOrderDetailService _deliveryOrderDetailService);
        SalesInvoiceDetail UpdateObject(SalesInvoiceDetail salesInvoiceDetail, IDeliveryOrderDetailService _deliveryOrderDetailService);
        SalesInvoiceDetail SoftDeleteObject(SalesInvoiceDetail salesInvoiceDetail);
        bool DeleteObject(int Id);
        SalesInvoiceDetail ConfirmObject(SalesInvoiceDetail salesInvoiceDetail, ISalesInvoiceDetailService _salesInvoiceDetailService, IDeliveryOrderDetailService _deliveryOrderDetailService);
        SalesInvoiceDetail UnconfirmObject(SalesInvoiceDetail salesInvoiceDetail, IReceiptVoucherDetailService _receiptVoucherDetailService, IReceivableService _receivableService);
    }
}