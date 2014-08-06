using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface ISalesInvoiceDetailValidator
    {
        SalesInvoiceDetail VHasDeliveryOrderDetail(SalesInvoiceDetail sid, IDeliveryOrderDetailService _dods);
        SalesInvoiceDetail VQuantity(SalesInvoiceDetail sid, IDeliveryOrderDetailService _dods);
        SalesInvoiceDetail VPrice(SalesInvoiceDetail sid);
        SalesInvoiceDetail VIsUniqueDeliveryOrderDetail(SalesInvoiceDetail sid, ISalesInvoiceDetailService _sids, IDeliveryOrderDetailService _dodds);
        SalesInvoiceDetail VHasReceipt(SalesInvoiceDetail sid, IReceiptVoucherDetailService _rvds, IReceivableService _receivableService);
        SalesInvoiceDetail VIsConfirmed(SalesInvoiceDetail sid);
        SalesInvoiceDetail VCreateObject(SalesInvoiceDetail sid, ISalesInvoiceDetailService _sids, IDeliveryOrderDetailService _dods);
        SalesInvoiceDetail VUpdateObject(SalesInvoiceDetail sid, ISalesInvoiceDetailService _sids, IDeliveryOrderDetailService _dods);
        SalesInvoiceDetail VDeleteObject(SalesInvoiceDetail sid);
        SalesInvoiceDetail VConfirmObject(SalesInvoiceDetail sid, ISalesInvoiceDetailService _sids, IDeliveryOrderDetailService _dods);
        SalesInvoiceDetail VUnconfirmObject(SalesInvoiceDetail sid, IReceiptVoucherDetailService _rvds, IReceivableService _receivableService);
        bool ValidCreateObject(SalesInvoiceDetail sid, ISalesInvoiceDetailService _sids, IDeliveryOrderDetailService _dods);
        bool ValidUpdateObject(SalesInvoiceDetail sid, ISalesInvoiceDetailService _sids, IDeliveryOrderDetailService _dods);
        bool ValidDeleteObject(SalesInvoiceDetail sid);
        bool ValidConfirmObject(SalesInvoiceDetail sid, ISalesInvoiceDetailService _sids, IDeliveryOrderDetailService _dods);
        bool ValidUnconfirmObject(SalesInvoiceDetail sid, IReceiptVoucherDetailService _rvds, IReceivableService _receivableService);
        bool isValid(SalesInvoiceDetail sid);
        string PrintError(SalesInvoiceDetail sid);
    }
}
