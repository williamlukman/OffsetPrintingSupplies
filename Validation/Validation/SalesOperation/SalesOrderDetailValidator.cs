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
    public class SalesOrderDetailValidator : ISalesOrderDetailValidator
    {
        public SalesOrderDetail VHasSalesOrder(SalesOrderDetail salesOrderDetail, ISalesOrderService _salesOrderService)
        {
            SalesOrder salesOrder = _salesOrderService.GetObjectById(salesOrderDetail.SalesOrderId);
            if (salesOrder == null)
            {
                salesOrderDetail.Errors.Add("SalesOrderId", "Tidak terasosiasi dengan sales order");
            }
            return salesOrderDetail;
        }

        public SalesOrderDetail VSalesOrderHasNotBeenConfirmed(SalesOrderDetail salesOrderDetail, ISalesOrderService _salesOrderService)
        {
            SalesOrder salesOrder = _salesOrderService.GetObjectById(salesOrderDetail.SalesOrderId);
            if (salesOrder.IsConfirmed)
            {
                salesOrderDetail.Errors.Add("Generic", "Sudah dikonfirmasi");
            }
            return salesOrderDetail;
        }

        public SalesOrderDetail VHasItem(SalesOrderDetail salesOrderDetail, IItemService _itemService)
        {
            Item item = _itemService.GetObjectById(salesOrderDetail.ItemId);
            if (item == null)
            {
                salesOrderDetail.Errors.Add("ItemId", "Tidak terasosiasi dengan item");
            }
            return salesOrderDetail;
        }

        public SalesOrderDetail VNonZeroNorNegativeQuantity(SalesOrderDetail salesOrderDetail)
        {
            if (salesOrderDetail.Quantity < 0)
            {
                salesOrderDetail.Errors.Add("Quantity", "Tidak boleh kurang dari 0");
            }
            return salesOrderDetail;
        }

        public SalesOrderDetail VNonNegativePrice(SalesOrderDetail salesOrderDetail)
        {
            if (salesOrderDetail.Price <= 0)
            {
                salesOrderDetail.Errors.Add("Price", "Tidak boleh kurang dari atau sama dengan 0");
            }
            return salesOrderDetail;
        }

        public SalesOrderDetail VUniqueSalesOrderDetail(SalesOrderDetail salesOrderDetail, ISalesOrderDetailService _salesOrderDetailService, IItemService _itemService)
        {
            IList<SalesOrderDetail> details = _salesOrderDetailService.GetObjectsBySalesOrderId(salesOrderDetail.SalesOrderId);
            foreach (var detail in details)
            {
                if (detail.ItemId == salesOrderDetail.ItemId && detail.Id != salesOrderDetail.Id)
                {
                    salesOrderDetail.Errors.Add("Generic", "Tidak boleh memiliki Sku yang sama dalam 1 Sales Order");
                    return salesOrderDetail;
                }
            }
            return salesOrderDetail;
        }

        public SalesOrderDetail VHasNotBeenConfirmed(SalesOrderDetail salesOrderDetail)
        {
            if (salesOrderDetail.IsConfirmed)
            {
                salesOrderDetail.Errors.Add("Generic", "Tidak boleh sudah selesai");
            }
            return salesOrderDetail;
        }

        public SalesOrderDetail VHasBeenConfirmed(SalesOrderDetail salesOrderDetail)
        {
            if (!salesOrderDetail.IsConfirmed)
            {
                salesOrderDetail.Errors.Add("Generic", "Belum selesai");
            }
            return salesOrderDetail;
        }

        public SalesOrderDetail VHasItemPendingDelivery(SalesOrderDetail salesOrderDetail, IItemService _itemService)
        {
            Item item = _itemService.GetObjectById(salesOrderDetail.ItemId);
            if (item.PendingDelivery < salesOrderDetail.Quantity)
            {
                salesOrderDetail.Errors.Add("Generic", "Tidak boleh kurang dari quantity dari Sales Order Detail");
            }
            return salesOrderDetail;
        }

        public SalesOrderDetail VHasNoDeliveryOrderDetail(SalesOrderDetail salesOrderDetail, IDeliveryOrderDetailService _deliveryOrderDetailService)
        {
            IList<DeliveryOrderDetail> details = _deliveryOrderDetailService.GetObjectsBySalesOrderDetailId(salesOrderDetail.Id);
            if (details.Any())
            {
                salesOrderDetail.Errors.Add("Generic", "Tidak boleh boleh terasosiasi dengan Delivery Order Detail");
            }
            return salesOrderDetail;
        }

        public SalesOrderDetail VHasConfirmationDate(SalesOrderDetail salesOrderDetail)
        {
            if (salesOrderDetail.ConfirmationDate == null)
            {
                salesOrderDetail.Errors.Add("ConfirmationDate", "Tidak boleh kosong");
            }
            return salesOrderDetail;
        }

        public SalesOrderDetail VCreateObject(SalesOrderDetail salesOrderDetail, ISalesOrderDetailService _salesOrderDetailService, ISalesOrderService _salesOrderService, IItemService _itemService)
        {
            VHasSalesOrder(salesOrderDetail, _salesOrderService);
            if (!isValid(salesOrderDetail)) { return salesOrderDetail; }
            VSalesOrderHasNotBeenConfirmed(salesOrderDetail, _salesOrderService);
            if (!isValid(salesOrderDetail)) { return salesOrderDetail; }
            VHasItem(salesOrderDetail, _itemService);
            if (!isValid(salesOrderDetail)) { return salesOrderDetail; }
            VNonZeroNorNegativeQuantity(salesOrderDetail);
            if (!isValid(salesOrderDetail)) { return salesOrderDetail; }
            VNonNegativePrice(salesOrderDetail);
            if (!isValid(salesOrderDetail)) { return salesOrderDetail; }
            VUniqueSalesOrderDetail(salesOrderDetail, _salesOrderDetailService, _itemService);
            //VQuantityIsLessThanItemQuantity
            return salesOrderDetail;
        }

        public SalesOrderDetail VUpdateObject(SalesOrderDetail salesOrderDetail, ISalesOrderDetailService _salesOrderDetailService, ISalesOrderService _salesOrderService, IItemService _itemService)
        {
            VCreateObject(salesOrderDetail, _salesOrderDetailService, _salesOrderService, _itemService);
            if (!isValid(salesOrderDetail)) { return salesOrderDetail; }
            VHasNotBeenConfirmed(salesOrderDetail);
            return salesOrderDetail;
        }

        public SalesOrderDetail VDeleteObject(SalesOrderDetail salesOrderDetail)
        {
            VHasNotBeenConfirmed(salesOrderDetail);
            return salesOrderDetail;
        }

        public SalesOrderDetail VConfirmObject(SalesOrderDetail salesOrderDetail)
        {
            VHasConfirmationDate(salesOrderDetail);
            if (!isValid(salesOrderDetail)) { return salesOrderDetail; }
            VHasNotBeenConfirmed(salesOrderDetail);
            return salesOrderDetail;
        }

        public SalesOrderDetail VUnconfirmObject(SalesOrderDetail salesOrderDetail, IDeliveryOrderDetailService _deliveryOrderDetailService, IItemService _itemService)
        {
            VHasBeenConfirmed(salesOrderDetail);
            if (!isValid(salesOrderDetail)) { return salesOrderDetail; }
            VHasItemPendingDelivery(salesOrderDetail, _itemService);
            if (!isValid(salesOrderDetail)) { return salesOrderDetail; }
            VHasNoDeliveryOrderDetail(salesOrderDetail, _deliveryOrderDetailService);
            return salesOrderDetail;
        }

        public bool ValidCreateObject(SalesOrderDetail salesOrderDetail,  ISalesOrderDetailService _salesOrderDetailService, ISalesOrderService _salesOrderService, IItemService _itemService)
        {
            VCreateObject(salesOrderDetail, _salesOrderDetailService, _salesOrderService, _itemService);
            return isValid(salesOrderDetail);
        }

        public bool ValidUpdateObject(SalesOrderDetail salesOrderDetail,  ISalesOrderDetailService _salesOrderDetailService, ISalesOrderService _salesOrderService, IItemService _itemService)
        {
            salesOrderDetail.Errors.Clear();
            VUpdateObject(salesOrderDetail, _salesOrderDetailService, _salesOrderService, _itemService);
            return isValid(salesOrderDetail);
        }

        public bool ValidDeleteObject(SalesOrderDetail salesOrderDetail)
        {
            salesOrderDetail.Errors.Clear();
            VDeleteObject(salesOrderDetail);
            return isValid(salesOrderDetail);
        }

        public bool ValidConfirmObject(SalesOrderDetail salesOrderDetail)
        {
            salesOrderDetail.Errors.Clear();
            VConfirmObject(salesOrderDetail);
            return isValid(salesOrderDetail);
        }

        public bool ValidUnconfirmObject(SalesOrderDetail salesOrderDetail, IDeliveryOrderDetailService _deliveryOrderDetailService, IItemService _itemService)
        {
            salesOrderDetail.Errors.Clear();
            VUnconfirmObject(salesOrderDetail, _deliveryOrderDetailService, _itemService);
            return isValid(salesOrderDetail);
        }

        public bool isValid(SalesOrderDetail obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(SalesOrderDetail obj)
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