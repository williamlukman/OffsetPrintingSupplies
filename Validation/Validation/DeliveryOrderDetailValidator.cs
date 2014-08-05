using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Core.Interface.Validation;
using Core.DomainModel;
using Core.Interface.Service;
using Core.Constants;

namespace Validation.Validation
{
    public class DeliveryOrderDetailValidator : IDeliveryOrderDetailValidator
    {
        public DeliveryOrderDetail VHasDeliveryOrder(DeliveryOrderDetail deliveryOrderDetail, IDeliveryOrderService _purchaseReceivalService)
        {
            DeliveryOrder pr = _purchaseReceivalService.GetObjectById(deliveryOrderDetail.DeliveryOrderId);
            if (pr == null)
            {
                deliveryOrderDetail.Errors.Add("DeliveryOrder", "Tidak boleh tidak ada");
            }
            return deliveryOrderDetail;
        }

        public DeliveryOrderDetail VHasItem(DeliveryOrderDetail deliveryOrderDetail, IItemService _itemService)
        {
            Item item = _itemService.GetObjectById(deliveryOrderDetail.ItemId);
            if (item == null)
            {
                deliveryOrderDetail.Errors.Add("Item", "Tidak boleh tidak ada");
            }
            return deliveryOrderDetail;
        }

        public DeliveryOrderDetail VCustomer(DeliveryOrderDetail deliveryOrderDetail, IDeliveryOrderService _purchaseReceivalService, ISalesOrderService _salesOrderService, ISalesOrderDetailService _salesOrderDetailService, ICustomerService _customerService)
        {
            DeliveryOrder pr = _purchaseReceivalService.GetObjectById(deliveryOrderDetail.DeliveryOrderId);
            SalesOrderDetail sod = _salesOrderDetailService.GetObjectById(deliveryOrderDetail.SalesOrderDetailId);
            if (sod == null)
            {
                deliveryOrderDetail.Errors.Add("SalesOrderDetail", "Tidak boleh tidak ada");
                return deliveryOrderDetail;
            }
            SalesOrder so = _salesOrderService.GetObjectById(sod.SalesOrderId);
            if (so.CustomerId != pr.CustomerId)
            {
                deliveryOrderDetail.Errors.Add("Customer", "Tidak boleh merupakan kustomer yang berbeda dengan Sales Order");
            }
            return deliveryOrderDetail;
        }

        public DeliveryOrderDetail VQuantityCreate(DeliveryOrderDetail deliveryOrderDetail, ISalesOrderDetailService _salesOrderDetailService)
        {
            SalesOrderDetail sod = _salesOrderDetailService.GetObjectById(deliveryOrderDetail.SalesOrderDetailId);
            if (deliveryOrderDetail.Quantity <= 0)
            {
                deliveryOrderDetail.Errors.Add("Quantity", "Tidak boleh kurang dari atau sama dengan 0");
            }
            if (deliveryOrderDetail.Quantity > sod.Quantity)
            {
                deliveryOrderDetail.Errors.Add("Quantity", "Tidak boleh lebih dari Sales Order quantity");
            }
            return deliveryOrderDetail;
        }

        public DeliveryOrderDetail VQuantityUpdate(DeliveryOrderDetail deliveryOrderDetail, ISalesOrderDetailService _salesOrderDetailService)
        {
            VQuantityCreate(deliveryOrderDetail, _salesOrderDetailService);
            return deliveryOrderDetail;
        }

        public DeliveryOrderDetail VQuantityUnfinish(DeliveryOrderDetail deliveryOrderDetail, IItemService _itemService)
        {
            Item item = _itemService.GetObjectById(deliveryOrderDetail.ItemId);
            if (item.PendingDelivery < 0)
            {
                deliveryOrderDetail.Errors.Add("Item.PendingDelivery", "Tidak boleh kurang dari 0");
            }
            if (item.Quantity < 0)
            {
                deliveryOrderDetail.Errors.Add("item.Quantity", "Tidak boleh kurang dari 0");
            }
            return deliveryOrderDetail;
        }

        public DeliveryOrderDetail VHasSalesOrderDetail(DeliveryOrderDetail deliveryOrderDetail, ISalesOrderDetailService _salesOrderDetailService)
        {
            SalesOrderDetail sod = _salesOrderDetailService.GetObjectById(deliveryOrderDetail.SalesOrderDetailId);
            if (sod == null)
            {
                deliveryOrderDetail.Errors.Add("SalesOrderDetail", "Tidak boleh tidak ada");
            }
            return deliveryOrderDetail;
        }

        public DeliveryOrderDetail VHasItemQuantity(DeliveryOrderDetail deliveryOrderDetail, IDeliveryOrderService _deliveryOrderService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            DeliveryOrder deliveryOrder = _deliveryOrderService.GetObjectById(deliveryOrderDetail.DeliveryOrderId);
            WarehouseItem warehouseItem = _warehouseItemService.FindOrCreateObject(deliveryOrder.WarehouseId, deliveryOrderDetail.ItemId);
            Item item = _itemService.GetObjectById(deliveryOrderDetail.ItemId);
            if (item.Quantity - deliveryOrderDetail.Quantity < 0)
            {
                deliveryOrderDetail.Errors.Add("Generic", "Item quantity kurang dari quantity untuk di kirim");
            }
            else if (warehouseItem.Quantity - deliveryOrderDetail.Quantity < 0)
            {
                deliveryOrderDetail.Errors.Add("Generic", "WarehouseItem quantity kurang dari quantity untuk di kirim");
            }
            return deliveryOrderDetail;
        }

        public DeliveryOrderDetail VUniqueSalesOrderDetail(DeliveryOrderDetail deliveryOrderDetail, IDeliveryOrderDetailService _deliveryOrderDetailService, IItemService _itemService)
        {
            IList<DeliveryOrderDetail> details = _deliveryOrderDetailService.GetObjectsByDeliveryOrderId(deliveryOrderDetail.DeliveryOrderId);
            foreach (var detail in details)
            {
                if (detail.SalesOrderDetailId == deliveryOrderDetail.SalesOrderDetailId && detail.Id != deliveryOrderDetail.Id)
                {
                    deliveryOrderDetail.Errors.Add("SalesOrderDetail", "Tidak boleh memiliki lebih dari 2 Delivery Order Detail");
                    return deliveryOrderDetail;
                }
            }
            return deliveryOrderDetail;
        }

        public DeliveryOrderDetail VHasBeenFinished(DeliveryOrderDetail deliveryOrderDetail)
        {
            if (!deliveryOrderDetail.IsFinished)
            {
                deliveryOrderDetail.Errors.Add("Generic", "Tidak boleh sudah dikonfirmasi.");
            }
            return deliveryOrderDetail;
        }

        public DeliveryOrderDetail VHasNotBeenFinished(DeliveryOrderDetail deliveryOrderDetail)
        {
            if (deliveryOrderDetail.IsFinished)
            {
                deliveryOrderDetail.Errors.Add("Generic", "Tidak boleh sudah dikonfirmasi.");
            }
            return deliveryOrderDetail;
        }

        public DeliveryOrderDetail VDeliveryOrderHasNotBeenCompleted(DeliveryOrderDetail deliveryOrderDetail, IDeliveryOrderService _deliveryOrderService)
        {
            DeliveryOrder deliveryOrder = _deliveryOrderService.GetObjectById(deliveryOrderDetail.DeliveryOrderId);
            if (deliveryOrder.IsCompleted)
            {
                deliveryOrderDetail.Errors.Add("Generic", "Delivery order sudah complete");
            }
            return deliveryOrderDetail;
        }

        public DeliveryOrderDetail VCreateObject(DeliveryOrderDetail deliveryOrderDetail, IDeliveryOrderDetailService _deliveryOrderDetailService, IDeliveryOrderService _deliveryOrderService,
                                                    ISalesOrderDetailService _salesOrderDetailService, ISalesOrderService _salesOrderService, IItemService _itemService, ICustomerService _customerService)
        {
            VHasDeliveryOrder(deliveryOrderDetail, _deliveryOrderService);
            if (!isValid(deliveryOrderDetail)) { return deliveryOrderDetail; }
            VHasItem(deliveryOrderDetail, _itemService);
            if (!isValid(deliveryOrderDetail)) { return deliveryOrderDetail; }
            VCustomer(deliveryOrderDetail, _deliveryOrderService, _salesOrderService, _salesOrderDetailService, _customerService);
            if (!isValid(deliveryOrderDetail)) { return deliveryOrderDetail; }
            VQuantityCreate(deliveryOrderDetail, _salesOrderDetailService);
            if (!isValid(deliveryOrderDetail)) { return deliveryOrderDetail; }
            VUniqueSalesOrderDetail(deliveryOrderDetail, _deliveryOrderDetailService, _itemService);
            return deliveryOrderDetail;
        }

        public DeliveryOrderDetail VUpdateObject(DeliveryOrderDetail deliveryOrderDetail, IDeliveryOrderDetailService _deliveryOrderDetailService, IDeliveryOrderService _deliveryOrderService,
                                                    ISalesOrderDetailService _salesOrderDetailService, ISalesOrderService _salesOrderService, IItemService _itemService, ICustomerService _customerService)
        {
            VHasNotBeenFinished(deliveryOrderDetail);
            if (!isValid(deliveryOrderDetail)) { return deliveryOrderDetail; }
            VCreateObject(deliveryOrderDetail, _deliveryOrderDetailService, _deliveryOrderService, _salesOrderDetailService, _salesOrderService, _itemService, _customerService);
            return deliveryOrderDetail;
        }

        public DeliveryOrderDetail VDeleteObject(DeliveryOrderDetail deliveryOrderDetail)
        {
            VHasNotBeenFinished(deliveryOrderDetail);
            return deliveryOrderDetail;
        }

        public DeliveryOrderDetail VFinishObject(DeliveryOrderDetail deliveryOrderDetail, IDeliveryOrderService _deliveryOrderService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            VHasNotBeenFinished(deliveryOrderDetail);
            if (!isValid(deliveryOrderDetail)) { return deliveryOrderDetail; }
            VHasItemQuantity(deliveryOrderDetail, _deliveryOrderService, _itemService, _warehouseItemService);
            return deliveryOrderDetail;
        }

        public DeliveryOrderDetail VUnfinishObject(DeliveryOrderDetail deliveryOrderDetail, IDeliveryOrderService _deliveryOrderService, IDeliveryOrderDetailService _deliveryOrderDetailService, IItemService _itemService)
        {
            VQuantityUnfinish(deliveryOrderDetail, _itemService);
            if (!isValid(deliveryOrderDetail)) { return deliveryOrderDetail; }
            VDeliveryOrderHasNotBeenCompleted(deliveryOrderDetail, _deliveryOrderService);
            return deliveryOrderDetail;
        }

        public bool ValidCreateObject(DeliveryOrderDetail deliveryOrderDetail, IDeliveryOrderDetailService _deliveryOrderDetailService, IDeliveryOrderService _purchaseReceivalService,
                                                    ISalesOrderDetailService _salesOrderDetailService, ISalesOrderService _salesOrderService, IItemService _itemService, ICustomerService _customerService)
        {
            VCreateObject(deliveryOrderDetail, _deliveryOrderDetailService, _purchaseReceivalService, _salesOrderDetailService, _salesOrderService, _itemService, _customerService);
            return isValid(deliveryOrderDetail);
        }

        public bool ValidUpdateObject(DeliveryOrderDetail deliveryOrderDetail, IDeliveryOrderDetailService _deliveryOrderDetailService, IDeliveryOrderService _purchaseReceivalService,
                                                    ISalesOrderDetailService _salesOrderDetailService, ISalesOrderService _salesOrderService, IItemService _itemService, ICustomerService _customerService)
        {
            deliveryOrderDetail.Errors.Clear();
            VUpdateObject(deliveryOrderDetail, _deliveryOrderDetailService, _purchaseReceivalService, _salesOrderDetailService, _salesOrderService, _itemService, _customerService);
            return isValid(deliveryOrderDetail);
        }

        public bool ValidDeleteObject(DeliveryOrderDetail deliveryOrderDetail)
        {
            deliveryOrderDetail.Errors.Clear();
            VDeleteObject(deliveryOrderDetail);
            return isValid(deliveryOrderDetail);
        }

        public bool ValidFinishObject(DeliveryOrderDetail deliveryOrderDetail, IDeliveryOrderService _deliveryOrderService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            deliveryOrderDetail.Errors.Clear();
            VFinishObject(deliveryOrderDetail, _deliveryOrderService, _itemService, _warehouseItemService);
            return isValid(deliveryOrderDetail);
        }

        public bool ValidUnfinishObject(DeliveryOrderDetail deliveryOrderDetail, IDeliveryOrderService _deliveryOrderService, IDeliveryOrderDetailService _deliveryOrderDetailService, IItemService _itemService)
        {
            deliveryOrderDetail.Errors.Clear();
            VUnfinishObject(deliveryOrderDetail, _deliveryOrderService, _deliveryOrderDetailService, _itemService);
            return isValid(deliveryOrderDetail);
        }

        public bool isValid(DeliveryOrderDetail obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(DeliveryOrderDetail obj)
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