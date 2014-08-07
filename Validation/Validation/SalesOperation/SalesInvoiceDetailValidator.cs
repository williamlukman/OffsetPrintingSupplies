using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Interface.Validation;
using Core.DomainModel;
using Core.Interface.Service;
using Core.Constants;

namespace Validation.Validation
{
    public class SalesInvoiceDetailValidator : ISalesInvoiceDetailValidator
    {

        public SalesInvoiceDetail VHasSalesInvoice(SalesInvoiceDetail salesInvoiceDetail, ISalesInvoiceService _salesInvoiceService)
        {
            SalesInvoice salesInvoice = _salesInvoiceService.GetObjectById(salesInvoiceDetail.SalesInvoiceId);
            if (salesInvoice == null)
            {
                salesInvoiceDetail.Errors.Add("SalesInvoiceId", "Tidak terasosiasi dengan sales invoice");
            }
            return salesInvoiceDetail;
        }

        public SalesInvoiceDetail VHasDeliveryOrderDetail(SalesInvoiceDetail salesInvoiceDetail, IDeliveryOrderDetailService _deliveryOrderDetailService)
        {
            DeliveryOrderDetail deliveryOrderDetail = _deliveryOrderDetailService.GetObjectById(salesInvoiceDetail.DeliveryOrderDetailId);
            if (deliveryOrderDetail == null)
            {
                salesInvoiceDetail.Errors.Add("DeliveryOrderDetail", "Tidak boleh tidak ada");
            }
            return salesInvoiceDetail;
        }

        public SalesInvoiceDetail VDeliveryOrderDetailAndSalesInvoiceMustHaveTheSameDeliveryOrder(SalesInvoiceDetail salesInvoiceDetail,
                                     IDeliveryOrderDetailService _deliveryOrderDetailService, ISalesInvoiceService _salesInvoiceService)
        {
            DeliveryOrderDetail deliveryOrderDetail = _deliveryOrderDetailService.GetObjectById(salesInvoiceDetail.DeliveryOrderDetailId);
            SalesInvoice salesInvoice = _salesInvoiceService.GetObjectById(salesInvoiceDetail.SalesInvoiceId);
            if (deliveryOrderDetail.DeliveryOrderId != salesInvoice.DeliveryOrderId)
            {
                deliveryOrderDetail.Errors.Add("Generic", "Delivery order detail dan sales invoice memiliki delivery order berbeda");
            }
            return salesInvoiceDetail;
        }

        public SalesInvoiceDetail VIsUniqueDeliveryOrderDetail(SalesInvoiceDetail salesInvoiceDetail, ISalesInvoiceDetailService _salesInvoiceDetail, IDeliveryOrderDetailService _deliveryOrderDetailService)
        {
            IList<SalesInvoiceDetail> details = _salesInvoiceDetail.GetObjectsBySalesInvoiceId(salesInvoiceDetail.SalesInvoiceId);
            foreach (var detail in details)
            {
                if (detail.DeliveryOrderDetailId == salesInvoiceDetail.DeliveryOrderDetailId && detail.Id != salesInvoiceDetail.Id)
                {
                    salesInvoiceDetail.Errors.Add("SalesInvoiceDetail", "Tidak boleh memiliki lebih dari 2 Sales Receival Detail");
                }
            }
            return salesInvoiceDetail;
        }

        public SalesInvoiceDetail VNonNegativeNorZeroQuantity(SalesInvoiceDetail salesInvoiceDetail)
        {
            if (salesInvoiceDetail.Quantity <= 0)
            {
                salesInvoiceDetail.Errors.Add("Quantity", "Harus lebih besar dari 0");
            }
            return salesInvoiceDetail;
        }

        public SalesInvoiceDetail VQuantityIsLessThanOrEqualPendingInvoiceQuantity(SalesInvoiceDetail salesInvoiceDetail, IDeliveryOrderDetailService _deliveryOrderDetailService)
        {
            DeliveryOrderDetail deliveryOrderDetail = _deliveryOrderDetailService.GetObjectById(salesInvoiceDetail.DeliveryOrderDetailId);
            if (salesInvoiceDetail.Quantity > deliveryOrderDetail.PendingInvoicedQuantity)
            {
                salesInvoiceDetail.Errors.Add("Generic", "Quantity harus kurang dari atau sama dengan pending invoice quantity");
            }
            return salesInvoiceDetail;
        }

        public SalesInvoiceDetail VHasBeenConfirmed(SalesInvoiceDetail salesInvoiceDetail)
        {
            if (!salesInvoiceDetail.IsConfirmed)
            {
                salesInvoiceDetail.Errors.Add("IsConfirmed", "Belum dikonfirmasi");
            }
            return salesInvoiceDetail;
        }

        public SalesInvoiceDetail VHasNotBeenConfirmed(SalesInvoiceDetail salesInvoiceDetail)
        {
            if (salesInvoiceDetail.IsConfirmed)
            {
                salesInvoiceDetail.Errors.Add("IsConfirmed", "Sudah dikonfirmasi");
            }
            return salesInvoiceDetail;
        }

        public SalesInvoiceDetail VCreateObject(SalesInvoiceDetail salesInvoiceDetail, ISalesInvoiceService _salesInvoiceService,
                                                   ISalesInvoiceDetailService _salesInvoiceDetailService, IDeliveryOrderDetailService _deliveryOrderDetailService)
        {
            VHasSalesInvoice(salesInvoiceDetail, _salesInvoiceService);
            if (!isValid(salesInvoiceDetail)) { return salesInvoiceDetail; }
            VHasDeliveryOrderDetail(salesInvoiceDetail, _deliveryOrderDetailService);
            if (!isValid(salesInvoiceDetail)) { return salesInvoiceDetail; }
            VDeliveryOrderDetailAndSalesInvoiceMustHaveTheSameDeliveryOrder(salesInvoiceDetail, _deliveryOrderDetailService, _salesInvoiceService);
            if (!isValid(salesInvoiceDetail)) { return salesInvoiceDetail; }
            VIsUniqueDeliveryOrderDetail(salesInvoiceDetail, _salesInvoiceDetailService, _deliveryOrderDetailService);
            if (!isValid(salesInvoiceDetail)) { return salesInvoiceDetail; }
            VNonNegativeNorZeroQuantity(salesInvoiceDetail);
            if (!isValid(salesInvoiceDetail)) { return salesInvoiceDetail; }
            VQuantityIsLessThanOrEqualPendingInvoiceQuantity(salesInvoiceDetail, _deliveryOrderDetailService);
            return salesInvoiceDetail;
        }

        public SalesInvoiceDetail VUpdateObject(SalesInvoiceDetail salesInvoiceDetail, ISalesInvoiceService _salesInvoiceService,
                                                   ISalesInvoiceDetailService _salesInvoiceDetailService, IDeliveryOrderDetailService _deliveryOrderDetailService)
        {
            VHasNotBeenConfirmed(salesInvoiceDetail);
            if (!isValid(salesInvoiceDetail)) { return salesInvoiceDetail; }
            VCreateObject(salesInvoiceDetail, _salesInvoiceService, _salesInvoiceDetailService, _deliveryOrderDetailService);
            return salesInvoiceDetail;
        }

        public SalesInvoiceDetail VDeleteObject(SalesInvoiceDetail salesInvoiceDetail)
        {
            VHasNotBeenConfirmed(salesInvoiceDetail);
            return salesInvoiceDetail;
        }

        public SalesInvoiceDetail VConfirmObject(SalesInvoiceDetail salesInvoiceDetail, ISalesInvoiceDetailService _salesInvoiceDetailService, IDeliveryOrderDetailService _deliveryOrderDetailService)
        {
            VHasNotBeenConfirmed(salesInvoiceDetail);
            if (!isValid(salesInvoiceDetail)) { return salesInvoiceDetail; }
            VQuantityIsLessThanOrEqualPendingInvoiceQuantity(salesInvoiceDetail, _deliveryOrderDetailService);
            return salesInvoiceDetail;
        }

        public SalesInvoiceDetail VUnconfirmObject(SalesInvoiceDetail salesInvoiceDetail)
        {
            VHasBeenConfirmed(salesInvoiceDetail);
            return salesInvoiceDetail;
        }

        public bool ValidCreateObject(SalesInvoiceDetail salesInvoiceDetail, ISalesInvoiceService _salesInvoiceService,
                                      ISalesInvoiceDetailService _salesInvoiceDetailService, IDeliveryOrderDetailService _deliveryOrderDetailService)
        {
            VCreateObject(salesInvoiceDetail, _salesInvoiceService, _salesInvoiceDetailService, _deliveryOrderDetailService);
            return isValid(salesInvoiceDetail);
        }

        public bool ValidUpdateObject(SalesInvoiceDetail salesInvoiceDetail, ISalesInvoiceService _salesInvoiceService, ISalesInvoiceDetailService _salesInvoiceDetailService, IDeliveryOrderDetailService _deliveryOrderDetailService)
        {
            salesInvoiceDetail.Errors.Clear();
            VUpdateObject(salesInvoiceDetail, _salesInvoiceService, _salesInvoiceDetailService, _deliveryOrderDetailService);
            return isValid(salesInvoiceDetail);
        }

        public bool ValidDeleteObject(SalesInvoiceDetail salesInvoiceDetail)
        {
            salesInvoiceDetail.Errors.Clear();
            VDeleteObject(salesInvoiceDetail);
            return isValid(salesInvoiceDetail);
        }

        public bool ValidConfirmObject(SalesInvoiceDetail salesInvoiceDetail, ISalesInvoiceDetailService _salesInvoiceDetailService, IDeliveryOrderDetailService _deliveryOrderDetailService)
        {
            salesInvoiceDetail.Errors.Clear();
            VConfirmObject(salesInvoiceDetail, _salesInvoiceDetailService, _deliveryOrderDetailService);
            return isValid(salesInvoiceDetail);
        }

        public bool ValidUnconfirmObject(SalesInvoiceDetail salesInvoiceDetail)
        {
            salesInvoiceDetail.Errors.Clear();
            VUnconfirmObject(salesInvoiceDetail);
            return isValid(salesInvoiceDetail);
        }

        public bool isValid(SalesInvoiceDetail obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(SalesInvoiceDetail obj)
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