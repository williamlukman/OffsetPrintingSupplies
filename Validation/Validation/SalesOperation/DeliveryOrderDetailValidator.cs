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
    public class DeliveryOrderDetailValidator : IDeliveryOrderDetailValidator
    {
        public DeliveryOrderDetail VHasDeliveryOrder(DeliveryOrderDetail deliveryOrderDetail, IDeliveryOrderService _purchaseReceivalService)
        {
            DeliveryOrder deliveryOrder = _purchaseReceivalService.GetObjectById(deliveryOrderDetail.DeliveryOrderId);
            if (deliveryOrder == null)
            {
                deliveryOrderDetail.Errors.Add("DeliveryOrderId", "Tidak boleh tidak ada");
            }
            return deliveryOrderDetail;
        }

        public DeliveryOrderDetail VDeliveryOrderHasNotBeenConfirmed(DeliveryOrderDetail deliveryOrderDetail, IDeliveryOrderService _purchaseReceivalService)
        {
            DeliveryOrder deliveryOrder = _purchaseReceivalService.GetObjectById(deliveryOrderDetail.DeliveryOrderId);
            if (deliveryOrder.IsConfirmed)
            {
                deliveryOrderDetail.Errors.Add("Generic", "Sudah dikonfirmasi");
            }
            return deliveryOrderDetail;
        }

        public DeliveryOrderDetail VHasItem(DeliveryOrderDetail deliveryOrderDetail, IItemService _itemService)
        {
            Item item = _itemService.GetObjectById(deliveryOrderDetail.ItemId);
            if (item == null)
            {
                deliveryOrderDetail.Errors.Add("ItemId", "Tidak boleh tidak ada");
            }
            return deliveryOrderDetail;
        }


        public DeliveryOrderDetail VNonNegativeQuantity(DeliveryOrderDetail deliveryOrderDetail)
        {
            if (deliveryOrderDetail.Quantity <= 0)
            {
                deliveryOrderDetail.Errors.Add("Quantity", "Tidak boleh negatif");
            }
            return deliveryOrderDetail;
        }

        public DeliveryOrderDetail VHasSalesOrderDetail(DeliveryOrderDetail deliveryOrderDetail, ISalesOrderDetailService _salesOrderDetailService)
        {
            SalesOrderDetail salesOrderDetail = _salesOrderDetailService.GetObjectById(deliveryOrderDetail.SalesOrderDetailId);
            if (salesOrderDetail == null)
            {
                deliveryOrderDetail.Errors.Add("SalesOrderDetailId", "Tidak boleh tidak ada");
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
                    deliveryOrderDetail.Errors.Add("Generic", "Tidak boleh memiliki lebih dari 2 Delivery Order Detail");
                    return deliveryOrderDetail;
                }
            }
            return deliveryOrderDetail;
        }

        public DeliveryOrderDetail VSalesOrderDetailHasBeenConfirmed(DeliveryOrderDetail deliveryOrderDetail, ISalesOrderDetailService _salesOrderDetailService)
        {
            SalesOrderDetail salesOrderDetail = _salesOrderDetailService.GetObjectById(deliveryOrderDetail.SalesOrderDetailId);
            if (!salesOrderDetail.IsConfirmed)
            {
                deliveryOrderDetail.Errors.Add("Generic", "Sales Order Detail belum dikonfirmasi");
            }
            return deliveryOrderDetail;
        }

        public DeliveryOrderDetail VQuantityOfDeliveryOrderDetailsIsLessThanOrEqualSalesOrderDetail(DeliveryOrderDetail deliveryOrderDetail,
                                     IDeliveryOrderDetailService _deliveryOrderDetailService, ISalesOrderDetailService _salesOrderDetailService, bool CaseCreate)
        {
            SalesOrderDetail salesOrderDetail = _salesOrderDetailService.GetObjectById(deliveryOrderDetail.SalesOrderDetailId);
            IList<DeliveryOrderDetail> details = _deliveryOrderDetailService.GetObjectsBySalesOrderDetailId(deliveryOrderDetail.SalesOrderDetailId);

            int totalDeliveryQuantity = 0;
            foreach (var detail in details)
            {
                if (!detail.IsConfirmed)
                {
                    totalDeliveryQuantity += detail.Quantity;
                }
            }
            if (CaseCreate) { totalDeliveryQuantity += deliveryOrderDetail.Quantity; }
            if (totalDeliveryQuantity > salesOrderDetail.PendingDeliveryQuantity)
            {
                int maxquantity = salesOrderDetail.PendingDeliveryQuantity - totalDeliveryQuantity + deliveryOrderDetail.Quantity;
                deliveryOrderDetail.Errors.Add("Generic", "Quantity maximum adalah " + maxquantity);
            }
            return deliveryOrderDetail;
        }
        
        public DeliveryOrderDetail VDeliveryOrderAndSalesOrderDetailHaveTheSameSalesOrder(DeliveryOrderDetail deliveryOrderDetail, IDeliveryOrderService _purchaseReceivalService, ISalesOrderDetailService _salesOrderDetailService)
        {
            DeliveryOrder deliveryOrder = _purchaseReceivalService.GetObjectById(deliveryOrderDetail.DeliveryOrderId);
            SalesOrderDetail salesOrderDetail = _salesOrderDetailService.GetObjectById(deliveryOrderDetail.SalesOrderDetailId);
            if (deliveryOrder.SalesOrderId != salesOrderDetail.SalesOrderId)
            {
                deliveryOrderDetail.Errors.Add("Generic", "Sales order dari sales order detail dan delivery order tidak sama");
                return deliveryOrderDetail;
            }
            return deliveryOrderDetail;
        }

        public DeliveryOrderDetail VHasServiceCostQuantity(DeliveryOrderDetail deliveryOrderDetail, IServiceCostService _serviceCostService)
        {
            ServiceCost serviceCost = _serviceCostService.GetObjectByItemId(deliveryOrderDetail.ItemId);
            if (serviceCost.Quantity - deliveryOrderDetail.Quantity < 0)
            {
                deliveryOrderDetail.Errors.Add("Generic", "Service quantity kurang dari quantity untuk di kirim");
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

        public DeliveryOrderDetail VHasBeenConfirmed(DeliveryOrderDetail deliveryOrderDetail)
        {
            if (!deliveryOrderDetail.IsConfirmed)
            {
                deliveryOrderDetail.Errors.Add("Generic", "Tidak boleh sudah dikonfirmasi.");
            }
            return deliveryOrderDetail;
        }

        public DeliveryOrderDetail VHasNotBeenConfirmed(DeliveryOrderDetail deliveryOrderDetail)
        {
            if (deliveryOrderDetail.IsConfirmed)
            {
                deliveryOrderDetail.Errors.Add("Generic", "Tidak boleh sudah dikonfirmasi.");
            }
            return deliveryOrderDetail;
        }

        public DeliveryOrderDetail VHasConfirmationDate(DeliveryOrderDetail obj)
        {
            if (obj.ConfirmationDate == null)
            {
                obj.Errors.Add("ConfirmationDate", "Tidak boleh kosong");
            }
            return obj;
        }

        public DeliveryOrderDetail VHasNoSalesInvoiceDetail(DeliveryOrderDetail deliveryOrderDetail, ISalesInvoiceDetailService _salesInvoiceDetailService)
        {
            IList<SalesInvoiceDetail> salesInvoiceDetails = _salesInvoiceDetailService.GetObjectsByDeliveryOrderDetailId(deliveryOrderDetail.Id);
            if (salesInvoiceDetails.Any())
            {
                deliveryOrderDetail.Errors.Add("Generic", "Sudah memiliki sales invoice detail");
            }
            return deliveryOrderDetail;
        }

        public DeliveryOrderDetail VCreateObject(DeliveryOrderDetail deliveryOrderDetail, IDeliveryOrderDetailService _deliveryOrderDetailService,
                                                 IDeliveryOrderService _deliveryOrderService, ISalesOrderDetailService _salesOrderDetailService, IItemService _itemService)
        {
            VHasDeliveryOrder(deliveryOrderDetail, _deliveryOrderService);
            if (!isValid(deliveryOrderDetail)) { return deliveryOrderDetail; }
            VDeliveryOrderHasNotBeenConfirmed(deliveryOrderDetail, _deliveryOrderService);
            if (!isValid(deliveryOrderDetail)) { return deliveryOrderDetail; }
            VHasItem(deliveryOrderDetail, _itemService);
            if (!isValid(deliveryOrderDetail)) { return deliveryOrderDetail; }
            VHasSalesOrderDetail(deliveryOrderDetail, _salesOrderDetailService);
            if (!isValid(deliveryOrderDetail)) { return deliveryOrderDetail; }
            VSalesOrderDetailHasBeenConfirmed(deliveryOrderDetail, _salesOrderDetailService);
            if (!isValid(deliveryOrderDetail)) { return deliveryOrderDetail; }
            VDeliveryOrderAndSalesOrderDetailHaveTheSameSalesOrder(deliveryOrderDetail, _deliveryOrderService, _salesOrderDetailService);
            if (!isValid(deliveryOrderDetail)) { return deliveryOrderDetail; }
            VNonNegativeQuantity(deliveryOrderDetail);
            if (!isValid(deliveryOrderDetail)) { return deliveryOrderDetail; }
            // specific parameter = true for create
            VQuantityOfDeliveryOrderDetailsIsLessThanOrEqualSalesOrderDetail(deliveryOrderDetail, _deliveryOrderDetailService, _salesOrderDetailService, true);
            if (!isValid(deliveryOrderDetail)) { return deliveryOrderDetail; }
            VUniqueSalesOrderDetail(deliveryOrderDetail, _deliveryOrderDetailService, _itemService);
            return deliveryOrderDetail;
        }

        public DeliveryOrderDetail VUpdateObject(DeliveryOrderDetail deliveryOrderDetail, IDeliveryOrderDetailService _deliveryOrderDetailService,
                                                 IDeliveryOrderService _deliveryOrderService, ISalesOrderDetailService _salesOrderDetailService, IItemService _itemService)
        {
            VHasNotBeenConfirmed(deliveryOrderDetail);
            if (!isValid(deliveryOrderDetail)) { return deliveryOrderDetail; }
            VHasDeliveryOrder(deliveryOrderDetail, _deliveryOrderService);
            if (!isValid(deliveryOrderDetail)) { return deliveryOrderDetail; }
            VDeliveryOrderHasNotBeenConfirmed(deliveryOrderDetail, _deliveryOrderService);
            if (!isValid(deliveryOrderDetail)) { return deliveryOrderDetail; }
            VHasItem(deliveryOrderDetail, _itemService);
            if (!isValid(deliveryOrderDetail)) { return deliveryOrderDetail; }
            VHasSalesOrderDetail(deliveryOrderDetail, _salesOrderDetailService);
            if (!isValid(deliveryOrderDetail)) { return deliveryOrderDetail; }
            VSalesOrderDetailHasBeenConfirmed(deliveryOrderDetail, _salesOrderDetailService);
            if (!isValid(deliveryOrderDetail)) { return deliveryOrderDetail; }
            VDeliveryOrderAndSalesOrderDetailHaveTheSameSalesOrder(deliveryOrderDetail, _deliveryOrderService, _salesOrderDetailService);
            if (!isValid(deliveryOrderDetail)) { return deliveryOrderDetail; }
            VNonNegativeQuantity(deliveryOrderDetail);
            if (!isValid(deliveryOrderDetail)) { return deliveryOrderDetail; }
            // specific parameter = false for non create function
            VQuantityOfDeliveryOrderDetailsIsLessThanOrEqualSalesOrderDetail(deliveryOrderDetail, _deliveryOrderDetailService, _salesOrderDetailService, false);
            if (!isValid(deliveryOrderDetail)) { return deliveryOrderDetail; }
            VUniqueSalesOrderDetail(deliveryOrderDetail, _deliveryOrderDetailService, _itemService);
            return deliveryOrderDetail;
        }

        public DeliveryOrderDetail VDeleteObject(DeliveryOrderDetail deliveryOrderDetail)
        {
            VHasNotBeenConfirmed(deliveryOrderDetail);
            return deliveryOrderDetail;
        }

        public DeliveryOrderDetail VConfirmObject(DeliveryOrderDetail deliveryOrderDetail, IDeliveryOrderService _deliveryOrderService,
                                                  IDeliveryOrderDetailService _deliveryOrderDetailService,  ISalesOrderDetailService _salesOrderDetailService,
                                                  IItemService _itemService, IWarehouseItemService _warehouseItemService, IServiceCostService _serviceCostService)
        {
            VHasConfirmationDate(deliveryOrderDetail);
            if (!isValid(deliveryOrderDetail)) { return deliveryOrderDetail; }
            VHasNotBeenConfirmed(deliveryOrderDetail);
            if (!isValid(deliveryOrderDetail)) { return deliveryOrderDetail; }
            SalesOrderDetail salesOrderDetail = _salesOrderDetailService.GetObjectById(deliveryOrderDetail.SalesOrderDetailId);
            if (salesOrderDetail.IsService)
            {
                VHasServiceCostQuantity(deliveryOrderDetail, _serviceCostService);
            }
            else
            {
                VHasItemQuantity(deliveryOrderDetail, _deliveryOrderService, _itemService, _warehouseItemService);
            }
            if (!isValid(deliveryOrderDetail)) { return deliveryOrderDetail; }
            VQuantityOfDeliveryOrderDetailsIsLessThanOrEqualSalesOrderDetail(deliveryOrderDetail, _deliveryOrderDetailService, _salesOrderDetailService, false);
            return deliveryOrderDetail;
        }

        public DeliveryOrderDetail VUnconfirmObject(DeliveryOrderDetail deliveryOrderDetail, ISalesInvoiceDetailService _salesInvoiceDetailService)
        {
            VHasNoSalesInvoiceDetail(deliveryOrderDetail, _salesInvoiceDetailService);
            return deliveryOrderDetail;
        }

        public bool ValidCreateObject(DeliveryOrderDetail deliveryOrderDetail, IDeliveryOrderDetailService _deliveryOrderDetailService,
                                      IDeliveryOrderService _deliveryOrderService, ISalesOrderDetailService _salesOrderDetailService, IItemService _itemService)
        {
            VCreateObject(deliveryOrderDetail, _deliveryOrderDetailService, _deliveryOrderService, _salesOrderDetailService, _itemService);
            return isValid(deliveryOrderDetail);
        }

        public bool ValidUpdateObject(DeliveryOrderDetail deliveryOrderDetail, IDeliveryOrderDetailService _deliveryOrderDetailService,
                                      IDeliveryOrderService _deliveryOrderService, ISalesOrderDetailService _salesOrderDetailService, IItemService _itemService)
        {
            deliveryOrderDetail.Errors.Clear();
            VUpdateObject(deliveryOrderDetail, _deliveryOrderDetailService, _deliveryOrderService, _salesOrderDetailService, _itemService);
            return isValid(deliveryOrderDetail);
        }

        public bool ValidDeleteObject(DeliveryOrderDetail deliveryOrderDetail)
        {
            deliveryOrderDetail.Errors.Clear();
            VDeleteObject(deliveryOrderDetail);
            return isValid(deliveryOrderDetail);
        }

        public bool ValidConfirmObject(DeliveryOrderDetail deliveryOrderDetail, IDeliveryOrderService _deliveryOrderService,
                                       IDeliveryOrderDetailService _deliveryOrderDetailService, ISalesOrderDetailService _salesOrderDetailService,
                                       IItemService _itemService, IWarehouseItemService _warehouseItemService, IServiceCostService _serviceCostService)
        {
            deliveryOrderDetail.Errors.Clear();
            VConfirmObject(deliveryOrderDetail, _deliveryOrderService, _deliveryOrderDetailService,
                           _salesOrderDetailService, _itemService, _warehouseItemService, _serviceCostService);
            return isValid(deliveryOrderDetail);
        }

        public bool ValidUnconfirmObject(DeliveryOrderDetail deliveryOrderDetail, ISalesInvoiceDetailService _salesInvoiceDetailService) 
        {
            deliveryOrderDetail.Errors.Clear();
            VUnconfirmObject(deliveryOrderDetail, _salesInvoiceDetailService);
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