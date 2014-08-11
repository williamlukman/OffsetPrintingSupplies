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
        IList<SalesInvoiceDetail> GetObjectsByDeliveryOrderDetailId(int deliveryOrderDetailId);
        SalesInvoiceDetail GetObjectById(int Id);
        SalesInvoiceDetail CreateObject(SalesInvoiceDetail salesInvoiceDetail, ISalesInvoiceService _salesInvoiceService,
                                           ISalesOrderDetailService _salesOrderDetailService, IDeliveryOrderDetailService _deliveryOrderDetailService);
        SalesInvoiceDetail CreateObject(int salesInvoiceId, int deliveryOrderDetailId, int quantity, decimal amount,
                                           ISalesInvoiceService _salesInvoiceService, ISalesOrderDetailService _salesOrderDetailService,
                                           IDeliveryOrderDetailService _deliveryOrderDetailService);
        SalesInvoiceDetail UpdateObject(SalesInvoiceDetail salesInvoiceDetail, ISalesInvoiceService _salesInvoiceService,
                                           ISalesOrderDetailService _salesOrderDetailService, IDeliveryOrderDetailService _deliveryOrderDetailService);
        SalesInvoiceDetail SoftDeleteObject(SalesInvoiceDetail salesInvoiceDetail, ISalesInvoiceService _salesInvoiceService);
        bool DeleteObject(int Id);
        SalesInvoiceDetail ConfirmObject(SalesInvoiceDetail salesInvoiceDetail, DateTime ConfirmationDate, IDeliveryOrderDetailService _deliveryOrderDetailService);
        SalesInvoiceDetail UnconfirmObject(SalesInvoiceDetail salesInvoiceDetail, IDeliveryOrderService _deliveryOrderService, IDeliveryOrderDetailService _deliveryOrderDetailService);
    }
}