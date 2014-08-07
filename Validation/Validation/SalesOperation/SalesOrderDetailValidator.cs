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
            SalesOrder so = _salesOrderService.GetObjectById(salesOrderDetail.SalesOrderId);
            if (so == null)
            {
                salesOrderDetail.Errors.Add("SalesOrder", "Tidak boleh tidak ada");
            }
            return salesOrderDetail;
        }

        public SalesOrderDetail VHasItem(SalesOrderDetail salesOrderDetail, IItemService _itemService)
        {
            Item item = _itemService.GetObjectById(salesOrderDetail.ItemId);
            if (item == null)
            {
                salesOrderDetail.Errors.Add("Item", "Tidak boleh tidak ada");
            }
            return salesOrderDetail;
        }

        public SalesOrderDetail VQuantity(SalesOrderDetail salesOrderDetail)
        {
            if (salesOrderDetail.Quantity < 0)
            {
                salesOrderDetail.Errors.Add("Quantity", "Tidak boleh kurang dari 0");
            }
            return salesOrderDetail;
        }

        public SalesOrderDetail VPrice(SalesOrderDetail salesOrderDetail)
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
                    salesOrderDetail.Errors.Add("SalesOrderDetail", "Tidak boleh memiliki Sku yang sama dalam 1 Sales Order");
                    return salesOrderDetail;
                }
            }
            return salesOrderDetail;
        }

        public SalesOrderDetail VHasNotBeenFinished(SalesOrderDetail salesOrderDetail)
        {
            if (salesOrderDetail.IsFinished)
            {
                salesOrderDetail.Errors.Add("Generic", "Tidak boleh sudah selesai");
            }
            return salesOrderDetail;
        }

        public SalesOrderDetail VHasBeenFinished(SalesOrderDetail salesOrderDetail)
        {
            if (!salesOrderDetail.IsFinished)
            {
                salesOrderDetail.Errors.Add("Generic", "Belum selesai");
            }
            return salesOrderDetail;
        }

        public SalesOrderDetail VSalesOrderHasNotBeenCompleted(SalesOrderDetail salesOrderDetail, ISalesOrderService _salesOrderService)
        {
            SalesOrder salesOrder = _salesOrderService.GetObjectById(salesOrderDetail.SalesOrderId);
            if (salesOrder.IsCompleted)
            {
                salesOrderDetail.Errors.Add("Generic", "Sales order sudah complete");
            }
            return salesOrderDetail;
        }

        public SalesOrderDetail VHasItemPendingDelivery(SalesOrderDetail salesOrderDetail, IItemService _itemService)
        {
            Item item = _itemService.GetObjectById(salesOrderDetail.ItemId);
            if (item.PendingDelivery < salesOrderDetail.Quantity)
            {
                salesOrderDetail.Errors.Add("Item.PendingDelivery", "Tidak boleh kurang dari quantity dari Sales Order Detail");
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

        public SalesOrderDetail VDeliveryOrderDetailHasBeenFinished(SalesOrderDetail salesOrderDetail, IDeliveryOrderDetailService _deliveryOrderDetailService)
        {
            IList<DeliveryOrderDetail> deliveryOrderDetails = _deliveryOrderDetailService.GetObjectsBySalesOrderDetailId(salesOrderDetail.Id);
            foreach (var deliveryOrderDetail in deliveryOrderDetails)
            {
                if (deliveryOrderDetail.IsFinished)
                {
                    return salesOrderDetail;
                }
            }
            salesOrderDetail.Errors.Add("Generic", "Belum selesai");
            return salesOrderDetail;
        }

        public SalesOrderDetail VCreateObject(SalesOrderDetail salesOrderDetail, ISalesOrderDetailService _salesOrderDetailService, ISalesOrderService _salesOrderService, IItemService _itemService)
        {
            VHasSalesOrder(salesOrderDetail, _salesOrderService);
            if (!isValid(salesOrderDetail)) { return salesOrderDetail; }
            VHasItem(salesOrderDetail, _itemService);
            if (!isValid(salesOrderDetail)) { return salesOrderDetail; }
            VQuantity(salesOrderDetail);
            if (!isValid(salesOrderDetail)) { return salesOrderDetail; }
            VPrice(salesOrderDetail);
            if (!isValid(salesOrderDetail)) { return salesOrderDetail; }
            VUniqueSalesOrderDetail(salesOrderDetail, _salesOrderDetailService, _itemService);
            return salesOrderDetail;
        }

        public SalesOrderDetail VUpdateObject(SalesOrderDetail salesOrderDetail, ISalesOrderDetailService _salesOrderDetailService, ISalesOrderService _salesOrderService, IItemService _itemService)
        {
            VCreateObject(salesOrderDetail, _salesOrderDetailService, _salesOrderService, _itemService);
            if (!isValid(salesOrderDetail)) { return salesOrderDetail; }
            VHasNotBeenFinished(salesOrderDetail);
            return salesOrderDetail;
        }

        public SalesOrderDetail VDeleteObject(SalesOrderDetail salesOrderDetail)
        {
            VHasNotBeenFinished(salesOrderDetail);
            return salesOrderDetail;
        }

        public SalesOrderDetail VFinishObject(SalesOrderDetail salesOrderDetail)
        {
            VHasNotBeenFinished(salesOrderDetail);
            return salesOrderDetail;
        }

        public SalesOrderDetail VUnfinishObject(SalesOrderDetail salesOrderDetail, ISalesOrderService _salesOrderService, ISalesOrderDetailService _salesOrderDetailService, IDeliveryOrderDetailService _deliveryOrderDetailService, IItemService _itemService)
        {
            VSalesOrderHasNotBeenCompleted(salesOrderDetail, _salesOrderService);
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

        public bool ValidFinishObject(SalesOrderDetail salesOrderDetail)
        {
            salesOrderDetail.Errors.Clear();
            VFinishObject(salesOrderDetail);
            return isValid(salesOrderDetail);
        }

        public bool ValidUnfinishObject(SalesOrderDetail salesOrderDetail, ISalesOrderService _salesOrderService, ISalesOrderDetailService _salesOrderDetailService, IDeliveryOrderDetailService _deliveryOrderDetailService, IItemService _itemService)
        {
            salesOrderDetail.Errors.Clear();
            VUnfinishObject(salesOrderDetail, _salesOrderService, _salesOrderDetailService, _deliveryOrderDetailService, _itemService);
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