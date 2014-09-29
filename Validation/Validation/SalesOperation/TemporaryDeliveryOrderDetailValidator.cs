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
    public class TemporaryDeliveryOrderDetailValidator : ITemporaryDeliveryOrderDetailValidator
    {
        public TemporaryDeliveryOrderDetail VHasTemporaryDeliveryOrder(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail, ITemporaryDeliveryOrderService _temporaryDeliveryOrderService)
        {
            TemporaryDeliveryOrder temporaryDeliveryOrder = _temporaryDeliveryOrderService.GetObjectById(temporaryDeliveryOrderDetail.TemporaryDeliveryOrderId);
            if (temporaryDeliveryOrder == null)
            {
                temporaryDeliveryOrderDetail.Errors.Add("Generic", "TemporaryDeliveryOrder tidak boleh tidak ada");
            }
            return temporaryDeliveryOrderDetail;
        }

        public TemporaryDeliveryOrderDetail VTemporaryDeliveryOrderHasNotBeenConfirmed(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail, ITemporaryDeliveryOrderService _purchaseReceivalService)
        {
            TemporaryDeliveryOrder temporaryDeliveryOrder = _purchaseReceivalService.GetObjectById(temporaryDeliveryOrderDetail.TemporaryDeliveryOrderId);
            if (temporaryDeliveryOrder.IsConfirmed)
            {
                temporaryDeliveryOrderDetail.Errors.Add("Generic", "Sudah dikonfirmasi");
            }
            return temporaryDeliveryOrderDetail;
        }

        public TemporaryDeliveryOrderDetail VHasItem(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail, IItemService _itemService)
        {
            Item item = _itemService.GetObjectById(temporaryDeliveryOrderDetail.ItemId);
            if (item == null)
            {
                temporaryDeliveryOrderDetail.Errors.Add("ItemId", "Tidak boleh tidak ada");
            }
            return temporaryDeliveryOrderDetail;
        }


        public TemporaryDeliveryOrderDetail VNonNegativeQuantity(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail)
        {
            if (temporaryDeliveryOrderDetail.Quantity <= 0)
            {
                temporaryDeliveryOrderDetail.Errors.Add("Quantity", "Tidak boleh negatif");
            }
            return temporaryDeliveryOrderDetail;
        }

        public TemporaryDeliveryOrderDetail VHasVirtualOrSalesOrderDetail(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail,
                   ITemporaryDeliveryOrderService _temporaryDeliveryOrderService, IVirtualOrderDetailService _virtualOrderDetailService,
                   ISalesOrderDetailService _salesOrderDetailService)
        {
            TemporaryDeliveryOrder temporaryDeliveryOrder = _temporaryDeliveryOrderService.GetObjectById(temporaryDeliveryOrderDetail.TemporaryDeliveryOrderId);
            if (temporaryDeliveryOrder.OrderType == Constant.OrderTypeCase.TrialOrder ||
                temporaryDeliveryOrder.OrderType == Constant.OrderTypeCase.SampleOrder)
            {
                VirtualOrderDetail virtualOrderDetail = _virtualOrderDetailService.GetObjectById((int)temporaryDeliveryOrderDetail.VirtualOrderDetailId);
                if (virtualOrderDetail == null)
                {
                    temporaryDeliveryOrderDetail.Errors.Add("Generic", "Virtual Order Detail tidak boleh tidak ada");
                }
            }
            else
            {
                SalesOrderDetail salesOrderDetail = _salesOrderDetailService.GetObjectById((int)temporaryDeliveryOrderDetail.SalesOrderDetailId);
                if (salesOrderDetail == null)
                {
                    temporaryDeliveryOrderDetail.Errors.Add("Generic", "Sales Order Detail Tidak boleh tidak ada");
                }
            }
                return temporaryDeliveryOrderDetail;
        }
        
        public TemporaryDeliveryOrderDetail VUniqueVirtualOrSalesOrderDetail(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService,
                                                                           ITemporaryDeliveryOrderService _temporaryDeliveryOrderService,  IVirtualOrderDetailService _virtualOrderDetailService,
                                                                           ISalesOrderDetailService _salesOrderDetailService, IItemService _itemService)
        {
            TemporaryDeliveryOrder temporaryDeliveryOrder = _temporaryDeliveryOrderService.GetObjectById(temporaryDeliveryOrderDetail.TemporaryDeliveryOrderId);
            IList<TemporaryDeliveryOrderDetail> details = _temporaryDeliveryOrderDetailService.GetObjectsByTemporaryDeliveryOrderId(temporaryDeliveryOrderDetail.TemporaryDeliveryOrderId);
            if (temporaryDeliveryOrder.OrderType == Constant.OrderTypeCase.SampleOrder ||
                temporaryDeliveryOrder.OrderType == Constant.OrderTypeCase.TrialOrder)
            {
                foreach (var detail in details)
                {
                    if (detail.VirtualOrderDetailId == temporaryDeliveryOrderDetail.VirtualOrderDetailId && detail.Id != temporaryDeliveryOrderDetail.Id)
                    {
                        temporaryDeliveryOrderDetail.Errors.Add("Generic", "1 virtual tidak boleh memiliki lebih dari 1 Delivery Order Detail");
                        return temporaryDeliveryOrderDetail;
                    }
                }
            }
            else
            {
                foreach (var detail in details)
                {
                    if (detail.SalesOrderDetailId == temporaryDeliveryOrderDetail.SalesOrderDetailId && detail.Id != temporaryDeliveryOrderDetail.Id)
                    {
                        temporaryDeliveryOrderDetail.Errors.Add("Generic", "1 sales tidak boleh memiliki lebih dari 1 Delivery Order Detail");
                        return temporaryDeliveryOrderDetail;
                    }
                }
            }
            return temporaryDeliveryOrderDetail;
        }

        public TemporaryDeliveryOrderDetail VVirtualOrSalesOrderDetailHasBeenConfirmed(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail, ITemporaryDeliveryOrderService _temporaryDeliveryOrderService,
                                                                                       IVirtualOrderDetailService _virtualOrderDetailService, ISalesOrderDetailService _salesOrderDetailService)
        {
            TemporaryDeliveryOrder temporaryDeliveryOrder = _temporaryDeliveryOrderService.GetObjectById(temporaryDeliveryOrderDetail.TemporaryDeliveryOrderId);
            if (temporaryDeliveryOrder.OrderType == Constant.OrderTypeCase.SampleOrder ||
                temporaryDeliveryOrder.OrderType == Constant.OrderTypeCase.TrialOrder)
            {
                VirtualOrderDetail virtualOrderDetail = _virtualOrderDetailService.GetObjectById((int)temporaryDeliveryOrderDetail.VirtualOrderDetailId);
                if (!virtualOrderDetail.IsConfirmed)
                {
                    temporaryDeliveryOrderDetail.Errors.Add("Generic", "Virtual Order Detail belum dikonfirmasi");
                }
            }
            else
            {
                SalesOrderDetail salesOrderDetail = _salesOrderDetailService.GetObjectById((int) temporaryDeliveryOrderDetail.SalesOrderDetailId);
                if (!salesOrderDetail.IsConfirmed)
                {
                    temporaryDeliveryOrderDetail.Errors.Add("Generic", "Sales Order Detail belum dikonfirmasi");
                }
            }
            return temporaryDeliveryOrderDetail;
        }

        public TemporaryDeliveryOrderDetail VQuantityOfTemporaryDeliveryOrderDetailsIsLessThanOrEqualVirtualOrSalesOrderDetail(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail,
                                     ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService, ITemporaryDeliveryOrderService _temporaryDeliveryOrderService,
                                     IVirtualOrderDetailService _virtualOrderDetailService, ISalesOrderDetailService _salesOrderDetailService, bool CaseCreate)
        {
            TemporaryDeliveryOrder temporaryDeliveryOrder = _temporaryDeliveryOrderService.GetObjectById(temporaryDeliveryOrderDetail.TemporaryDeliveryOrderId);
            if (temporaryDeliveryOrder.OrderType == Constant.OrderTypeCase.SampleOrder ||
                temporaryDeliveryOrder.OrderType == Constant.OrderTypeCase.TrialOrder)
            {
                VirtualOrderDetail virtualOrderDetail = _virtualOrderDetailService.GetObjectById((int)temporaryDeliveryOrderDetail.VirtualOrderDetailId);
                IList<TemporaryDeliveryOrderDetail> details = _temporaryDeliveryOrderDetailService.GetObjectsByVirtualOrderDetailId((int)temporaryDeliveryOrderDetail.VirtualOrderDetailId);

                int totalDeliveryQuantity = 0;
                foreach (var detail in details)
                {
                    if (!detail.IsConfirmed)
                    {
                        totalDeliveryQuantity += detail.Quantity;
                    }
                }
                if (CaseCreate) { totalDeliveryQuantity += temporaryDeliveryOrderDetail.Quantity; }
                if (totalDeliveryQuantity > virtualOrderDetail.PendingDeliveryQuantity)
                {
                    int maxquantity = virtualOrderDetail.PendingDeliveryQuantity - totalDeliveryQuantity + temporaryDeliveryOrderDetail.Quantity;
                    temporaryDeliveryOrderDetail.Errors.Add("Generic", "Quantity maximum adalah " + maxquantity);
                }
            }
            else
            {
                SalesOrderDetail salesOrderDetail = _salesOrderDetailService.GetObjectById((int)temporaryDeliveryOrderDetail.SalesOrderDetailId);
                IList<TemporaryDeliveryOrderDetail> details = _temporaryDeliveryOrderDetailService.GetObjectsBySalesOrderDetailId((int)temporaryDeliveryOrderDetail.SalesOrderDetailId);

                int totalDeliveryQuantity = 0;
                foreach (var detail in details)
                {
                    if (!detail.IsConfirmed)
                    {
                        totalDeliveryQuantity += detail.Quantity;
                    }
                }
                if (CaseCreate) { totalDeliveryQuantity += temporaryDeliveryOrderDetail.Quantity; }
                if (totalDeliveryQuantity > salesOrderDetail.PendingDeliveryQuantity)
                {
                    int maxquantity = salesOrderDetail.PendingDeliveryQuantity - totalDeliveryQuantity + temporaryDeliveryOrderDetail.Quantity;
                    temporaryDeliveryOrderDetail.Errors.Add("Generic", "Quantity maximum adalah " + maxquantity);
                }
            }
            return temporaryDeliveryOrderDetail;
        }

        public TemporaryDeliveryOrderDetail VHasTheSameParentOrder(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail, ITemporaryDeliveryOrderService _temporaryDeliveryOrderService,
                                                                   IDeliveryOrderService _deliveryOrderService, IVirtualOrderDetailService _virtualOrderDetailService,
                                                                   ISalesOrderDetailService _salesOrderDetailService)
        {
            TemporaryDeliveryOrder temporaryDeliveryOrder = _temporaryDeliveryOrderService.GetObjectById(temporaryDeliveryOrderDetail.TemporaryDeliveryOrderId);
            if (temporaryDeliveryOrder.OrderType == Constant.OrderTypeCase.SampleOrder ||
                temporaryDeliveryOrder.OrderType == Constant.OrderTypeCase.TrialOrder)
            {
                VirtualOrderDetail virtualOrderDetail = _virtualOrderDetailService.GetObjectById((int)temporaryDeliveryOrderDetail.VirtualOrderDetailId);
                if (temporaryDeliveryOrder.VirtualOrderId != virtualOrderDetail.VirtualOrderId)
                {
                    temporaryDeliveryOrderDetail.Errors.Add("Generic", "Virtual order dari virtual order detail dan temporary delivery order tidak sama");
                }
            }
            else
            {
                SalesOrderDetail salesOrderDetail = _salesOrderDetailService.GetObjectById((int) temporaryDeliveryOrderDetail.SalesOrderDetailId);
                DeliveryOrder deliveryOrder = _deliveryOrderService.GetObjectById((int) temporaryDeliveryOrder.DeliveryOrderId);
                if (deliveryOrder.SalesOrderId != salesOrderDetail.SalesOrderId)
                {
                    temporaryDeliveryOrderDetail.Errors.Add("Generic", "Sales order dari sales order detail dan delivery order tidak sama");
                }
            }
            return temporaryDeliveryOrderDetail;
        }

        public TemporaryDeliveryOrderDetail VHasItemQuantity(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail, ITemporaryDeliveryOrderService _temporaryDeliveryOrderService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            TemporaryDeliveryOrder temporaryDeliveryOrder = _temporaryDeliveryOrderService.GetObjectById(temporaryDeliveryOrderDetail.TemporaryDeliveryOrderId);
            WarehouseItem warehouseItem = _warehouseItemService.FindOrCreateObject(temporaryDeliveryOrder.WarehouseId, temporaryDeliveryOrderDetail.ItemId);
            Item item = _itemService.GetObjectById(temporaryDeliveryOrderDetail.ItemId);
            if (item.Quantity - temporaryDeliveryOrderDetail.Quantity < 0)
            {
                temporaryDeliveryOrderDetail.Errors.Add("Generic", "Item quantity kurang dari quantity untuk di kirim");
            }
            else if (warehouseItem.Quantity - temporaryDeliveryOrderDetail.Quantity < 0)
            {
                temporaryDeliveryOrderDetail.Errors.Add("Generic", "WarehouseItem quantity kurang dari quantity untuk di kirim");
            }
            return temporaryDeliveryOrderDetail;
        }

        public TemporaryDeliveryOrderDetail VHasBeenConfirmed(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail)
        {
            if (!temporaryDeliveryOrderDetail.IsConfirmed)
            {
                temporaryDeliveryOrderDetail.Errors.Add("Generic", "Tidak boleh sudah dikonfirmasi.");
            }
            return temporaryDeliveryOrderDetail;
        }

        public TemporaryDeliveryOrderDetail VHasNotBeenConfirmed(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail)
        {
            if (temporaryDeliveryOrderDetail.IsConfirmed)
            {
                temporaryDeliveryOrderDetail.Errors.Add("Generic", "Tidak boleh sudah dikonfirmasi.");
            }
            return temporaryDeliveryOrderDetail;
        }

        public TemporaryDeliveryOrderDetail VHasConfirmationDate(TemporaryDeliveryOrderDetail obj)
        {
            if (obj.ConfirmationDate == null)
            {
                obj.Errors.Add("ConfirmationDate", "Tidak boleh kosong");
            }
            return obj;
        }

        public TemporaryDeliveryOrderDetail VCreateObject(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService,
                                                          ITemporaryDeliveryOrderService _temporaryDeliveryOrderService, IVirtualOrderDetailService _virtualOrderDetailService,
                                                          ISalesOrderDetailService _salesOrderDetailService, IDeliveryOrderService _deliveryOrderService, IItemService _itemService)
        {
            VHasTemporaryDeliveryOrder(temporaryDeliveryOrderDetail, _temporaryDeliveryOrderService);
            if (!isValid(temporaryDeliveryOrderDetail)) { return temporaryDeliveryOrderDetail; }
            VTemporaryDeliveryOrderHasNotBeenConfirmed(temporaryDeliveryOrderDetail, _temporaryDeliveryOrderService);
            if (!isValid(temporaryDeliveryOrderDetail)) { return temporaryDeliveryOrderDetail; }
            VHasItem(temporaryDeliveryOrderDetail, _itemService);
            if (!isValid(temporaryDeliveryOrderDetail)) { return temporaryDeliveryOrderDetail; }
            VHasVirtualOrSalesOrderDetail(temporaryDeliveryOrderDetail, _temporaryDeliveryOrderService, _virtualOrderDetailService, _salesOrderDetailService);
            if (!isValid(temporaryDeliveryOrderDetail)) { return temporaryDeliveryOrderDetail; }
            VVirtualOrSalesOrderDetailHasBeenConfirmed(temporaryDeliveryOrderDetail, _temporaryDeliveryOrderService, _virtualOrderDetailService, _salesOrderDetailService);
            if (!isValid(temporaryDeliveryOrderDetail)) { return temporaryDeliveryOrderDetail; }
            VHasTheSameParentOrder(temporaryDeliveryOrderDetail, _temporaryDeliveryOrderService, _deliveryOrderService, _virtualOrderDetailService, _salesOrderDetailService);
            if (!isValid(temporaryDeliveryOrderDetail)) { return temporaryDeliveryOrderDetail; }
            VNonNegativeQuantity(temporaryDeliveryOrderDetail);
            if (!isValid(temporaryDeliveryOrderDetail)) { return temporaryDeliveryOrderDetail; }
            // specific parameter = true for create
            VQuantityOfTemporaryDeliveryOrderDetailsIsLessThanOrEqualVirtualOrSalesOrderDetail(temporaryDeliveryOrderDetail, _temporaryDeliveryOrderDetailService,
                               _temporaryDeliveryOrderService, _virtualOrderDetailService, _salesOrderDetailService, true);
            if (!isValid(temporaryDeliveryOrderDetail)) { return temporaryDeliveryOrderDetail; }
            VUniqueVirtualOrSalesOrderDetail(temporaryDeliveryOrderDetail, _temporaryDeliveryOrderDetailService, _temporaryDeliveryOrderService,
                                             _virtualOrderDetailService, _salesOrderDetailService, _itemService);
            return temporaryDeliveryOrderDetail;
        }

        public TemporaryDeliveryOrderDetail VUpdateObject(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService,
                                                          ITemporaryDeliveryOrderService _temporaryDeliveryOrderService, IVirtualOrderDetailService _virtualOrderDetailService,
                                                          ISalesOrderDetailService _salesOrderDetailService, IDeliveryOrderService _deliveryOrderService, IItemService _itemService)
        {
            VHasNotBeenConfirmed(temporaryDeliveryOrderDetail);
            if (!isValid(temporaryDeliveryOrderDetail)) { return temporaryDeliveryOrderDetail; }
            VHasTemporaryDeliveryOrder(temporaryDeliveryOrderDetail, _temporaryDeliveryOrderService);
            if (!isValid(temporaryDeliveryOrderDetail)) { return temporaryDeliveryOrderDetail; }
            VTemporaryDeliveryOrderHasNotBeenConfirmed(temporaryDeliveryOrderDetail, _temporaryDeliveryOrderService);
            if (!isValid(temporaryDeliveryOrderDetail)) { return temporaryDeliveryOrderDetail; }
            VHasItem(temporaryDeliveryOrderDetail, _itemService);
            if (!isValid(temporaryDeliveryOrderDetail)) { return temporaryDeliveryOrderDetail; }

            VHasVirtualOrSalesOrderDetail(temporaryDeliveryOrderDetail, _temporaryDeliveryOrderService, _virtualOrderDetailService, _salesOrderDetailService);
            if (!isValid(temporaryDeliveryOrderDetail)) { return temporaryDeliveryOrderDetail; }
            VVirtualOrSalesOrderDetailHasBeenConfirmed(temporaryDeliveryOrderDetail, _temporaryDeliveryOrderService, _virtualOrderDetailService, _salesOrderDetailService);
            if (!isValid(temporaryDeliveryOrderDetail)) { return temporaryDeliveryOrderDetail; }
            VHasTheSameParentOrder(temporaryDeliveryOrderDetail, _temporaryDeliveryOrderService, _deliveryOrderService, _virtualOrderDetailService, _salesOrderDetailService);
            if (!isValid(temporaryDeliveryOrderDetail)) { return temporaryDeliveryOrderDetail; }
            VNonNegativeQuantity(temporaryDeliveryOrderDetail);
            if (!isValid(temporaryDeliveryOrderDetail)) { return temporaryDeliveryOrderDetail; }
            // specific parameter = false for non create function
            VQuantityOfTemporaryDeliveryOrderDetailsIsLessThanOrEqualVirtualOrSalesOrderDetail(temporaryDeliveryOrderDetail, _temporaryDeliveryOrderDetailService,
                         _temporaryDeliveryOrderService, _virtualOrderDetailService, _salesOrderDetailService, false);
            if (!isValid(temporaryDeliveryOrderDetail)) { return temporaryDeliveryOrderDetail; }
            VUniqueVirtualOrSalesOrderDetail(temporaryDeliveryOrderDetail, _temporaryDeliveryOrderDetailService, _temporaryDeliveryOrderService,
                                             _virtualOrderDetailService, _salesOrderDetailService, _itemService);
            return temporaryDeliveryOrderDetail;
        }

        public TemporaryDeliveryOrderDetail VDeleteObject(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail)
        {
            VHasNotBeenConfirmed(temporaryDeliveryOrderDetail);
            return temporaryDeliveryOrderDetail;
        }

        public TemporaryDeliveryOrderDetail VConfirmObject(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail, ITemporaryDeliveryOrderService _temporaryDeliveryOrderService,
                                                    ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService, IVirtualOrderDetailService _virtualOrderDetailService,
                                                    ISalesOrderDetailService _salesOrderDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            VHasConfirmationDate(temporaryDeliveryOrderDetail);
            if (!isValid(temporaryDeliveryOrderDetail)) { return temporaryDeliveryOrderDetail; }
            VHasNotBeenConfirmed(temporaryDeliveryOrderDetail);
            if (!isValid(temporaryDeliveryOrderDetail)) { return temporaryDeliveryOrderDetail; }
            VHasItemQuantity(temporaryDeliveryOrderDetail, _temporaryDeliveryOrderService, _itemService, _warehouseItemService);
            if (!isValid(temporaryDeliveryOrderDetail)) { return temporaryDeliveryOrderDetail; }
            VQuantityOfTemporaryDeliveryOrderDetailsIsLessThanOrEqualVirtualOrSalesOrderDetail(temporaryDeliveryOrderDetail, _temporaryDeliveryOrderDetailService, _temporaryDeliveryOrderService,
                                _virtualOrderDetailService, _salesOrderDetailService, false);
            return temporaryDeliveryOrderDetail;
        }

        public TemporaryDeliveryOrderDetail VUnconfirmObject(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail)
        {
            return temporaryDeliveryOrderDetail;
        }

        public bool ValidCreateObject(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService,
                                      ITemporaryDeliveryOrderService _temporaryDeliveryOrderService, IVirtualOrderDetailService _virtualOrderDetailService,
                                      ISalesOrderDetailService _salesOrderDetailService, IDeliveryOrderService _deliveryOrderService, IItemService _itemService)
        {
            VCreateObject(temporaryDeliveryOrderDetail, _temporaryDeliveryOrderDetailService, _temporaryDeliveryOrderService, _virtualOrderDetailService,
                          _salesOrderDetailService, _deliveryOrderService, _itemService);
            return isValid(temporaryDeliveryOrderDetail);
        }

        public bool ValidUpdateObject(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService,
                                      ITemporaryDeliveryOrderService _temporaryDeliveryOrderService, IVirtualOrderDetailService _virtualOrderDetailService, 
                                      ISalesOrderDetailService _salesOrderDetailService, IDeliveryOrderService _deliveryOrderService, IItemService _itemService)
        {
            temporaryDeliveryOrderDetail.Errors.Clear();
            VUpdateObject(temporaryDeliveryOrderDetail, _temporaryDeliveryOrderDetailService, _temporaryDeliveryOrderService, _virtualOrderDetailService, 
                          _salesOrderDetailService, _deliveryOrderService, _itemService);
            return isValid(temporaryDeliveryOrderDetail);
        }

        public bool ValidDeleteObject(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail)
        {
            temporaryDeliveryOrderDetail.Errors.Clear();
            VDeleteObject(temporaryDeliveryOrderDetail);
            return isValid(temporaryDeliveryOrderDetail);
        }

        public bool ValidConfirmObject(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail, ITemporaryDeliveryOrderService _temporaryDeliveryOrderService,
                                       ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService, IVirtualOrderDetailService _virtualOrderDetailService,
                                       ISalesOrderDetailService _salesOrderDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            temporaryDeliveryOrderDetail.Errors.Clear();
            VConfirmObject(temporaryDeliveryOrderDetail, _temporaryDeliveryOrderService, _temporaryDeliveryOrderDetailService, _virtualOrderDetailService,
                           _salesOrderDetailService, _itemService, _warehouseItemService);
            return isValid(temporaryDeliveryOrderDetail);
        }

        public bool ValidUnconfirmObject(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail) 
        {
            temporaryDeliveryOrderDetail.Errors.Clear();
            VUnconfirmObject(temporaryDeliveryOrderDetail);
            return isValid(temporaryDeliveryOrderDetail);
        }

        public bool isValid(TemporaryDeliveryOrderDetail obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(TemporaryDeliveryOrderDetail obj)
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