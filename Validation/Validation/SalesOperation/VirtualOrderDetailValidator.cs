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
    public class VirtualOrderDetailValidator : IVirtualOrderDetailValidator
    {
        public VirtualOrderDetail VHasVirtualOrder(VirtualOrderDetail virtualOrderDetail, IVirtualOrderService _virtualOrderService)
        {
            VirtualOrder virtualOrder = _virtualOrderService.GetObjectById(virtualOrderDetail.VirtualOrderId);
            if (virtualOrder == null)
            {
                virtualOrderDetail.Errors.Add("VirtualOrderId", "Tidak terasosiasi dengan virtual order");
            }
            return virtualOrderDetail;
        }

        public VirtualOrderDetail VVirtualOrderHasNotBeenConfirmed(VirtualOrderDetail virtualOrderDetail, IVirtualOrderService _virtualOrderService)
        {
            VirtualOrder virtualOrder = _virtualOrderService.GetObjectById(virtualOrderDetail.VirtualOrderId);
            if (virtualOrder.IsConfirmed)
            {
                virtualOrderDetail.Errors.Add("Generic", "Sudah dikonfirmasi");
            }
            return virtualOrderDetail;
        }

        public VirtualOrderDetail VHasItem(VirtualOrderDetail virtualOrderDetail, IItemService _itemService)
        {
            Item item = _itemService.GetObjectById(virtualOrderDetail.ItemId);
            if (item == null)
            {
                virtualOrderDetail.Errors.Add("ItemId", "Tidak terasosiasi dengan item");
            }
            return virtualOrderDetail;
        }

        public VirtualOrderDetail VNonZeroNorNegativeQuantity(VirtualOrderDetail virtualOrderDetail)
        {
            if (virtualOrderDetail.Quantity < 0)
            {
                virtualOrderDetail.Errors.Add("Quantity", "Tidak boleh kurang dari 0");
            }
            return virtualOrderDetail;
        }

        public VirtualOrderDetail VNonNegativePrice(VirtualOrderDetail virtualOrderDetail)
        {
            if (virtualOrderDetail.Price <= 0)
            {
                virtualOrderDetail.Errors.Add("Price", "Tidak boleh kurang dari atau sama dengan 0");
            }
            return virtualOrderDetail;
        }

        public VirtualOrderDetail VUniqueVirtualOrderDetail(VirtualOrderDetail virtualOrderDetail, IVirtualOrderDetailService _virtualOrderDetailService, IItemService _itemService)
        {
            IList<VirtualOrderDetail> details = _virtualOrderDetailService.GetObjectsByVirtualOrderId(virtualOrderDetail.VirtualOrderId);
            foreach (var detail in details)
            {
                if (detail.ItemId == virtualOrderDetail.ItemId && detail.Id != virtualOrderDetail.Id)
                {
                    virtualOrderDetail.Errors.Add("Generic", "Tidak boleh memiliki Sku yang sama dalam 1 Virtual Order");
                    return virtualOrderDetail;
                }
            }
            return virtualOrderDetail;
        }

        public VirtualOrderDetail VHasNotBeenConfirmed(VirtualOrderDetail virtualOrderDetail)
        {
            if (virtualOrderDetail.IsConfirmed)
            {
                virtualOrderDetail.Errors.Add("Generic", "Tidak boleh sudah selesai");
            }
            return virtualOrderDetail;
        }

        public VirtualOrderDetail VHasBeenConfirmed(VirtualOrderDetail virtualOrderDetail)
        {
            if (!virtualOrderDetail.IsConfirmed)
            {
                virtualOrderDetail.Errors.Add("Generic", "Belum selesai");
            }
            return virtualOrderDetail;
        }

        public VirtualOrderDetail VHasItemPendingDelivery(VirtualOrderDetail virtualOrderDetail, IItemService _itemService)
        {
            Item item = _itemService.GetObjectById(virtualOrderDetail.ItemId);
            if (item.PendingDelivery < virtualOrderDetail.Quantity)
            {
                virtualOrderDetail.Errors.Add("Generic", "Tidak boleh kurang dari quantity dari Virtual Order Detail");
            }
            return virtualOrderDetail;
        }

        public VirtualOrderDetail VHasNoTemporaryDeliveryOrderDetail(VirtualOrderDetail virtualOrderDetail, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService)
        {
            IList<TemporaryDeliveryOrderDetail> details = _temporaryDeliveryOrderDetailService.GetObjectsByVirtualOrderDetailId(virtualOrderDetail.Id);
            if (details.Any())
            {
                virtualOrderDetail.Errors.Add("Generic", "Tidak boleh boleh terasosiasi dengan Temporary Delivery Order Detail");
            }
            return virtualOrderDetail;
        }

        public VirtualOrderDetail VHasConfirmationDate(VirtualOrderDetail virtualOrderDetail)
        {
            if (virtualOrderDetail.ConfirmationDate == null)
            {
                virtualOrderDetail.Errors.Add("ConfirmationDate", "Tidak boleh kosong");
            }
            return virtualOrderDetail;
        }

        public VirtualOrderDetail VCreateObject(VirtualOrderDetail virtualOrderDetail, IVirtualOrderDetailService _virtualOrderDetailService, IVirtualOrderService _virtualOrderService, IItemService _itemService)
        {
            VHasVirtualOrder(virtualOrderDetail, _virtualOrderService);
            if (!isValid(virtualOrderDetail)) { return virtualOrderDetail; }
            VVirtualOrderHasNotBeenConfirmed(virtualOrderDetail, _virtualOrderService);
            if (!isValid(virtualOrderDetail)) { return virtualOrderDetail; }
            VHasItem(virtualOrderDetail, _itemService);
            if (!isValid(virtualOrderDetail)) { return virtualOrderDetail; }
            VNonZeroNorNegativeQuantity(virtualOrderDetail);
            if (!isValid(virtualOrderDetail)) { return virtualOrderDetail; }
            VNonNegativePrice(virtualOrderDetail);
            if (!isValid(virtualOrderDetail)) { return virtualOrderDetail; }
            VUniqueVirtualOrderDetail(virtualOrderDetail, _virtualOrderDetailService, _itemService);
            //VQuantityIsLessThanItemQuantity
            return virtualOrderDetail;
        }

        public VirtualOrderDetail VUpdateObject(VirtualOrderDetail virtualOrderDetail, IVirtualOrderDetailService _virtualOrderDetailService, IVirtualOrderService _virtualOrderService, IItemService _itemService)
        {
            VCreateObject(virtualOrderDetail, _virtualOrderDetailService, _virtualOrderService, _itemService);
            if (!isValid(virtualOrderDetail)) { return virtualOrderDetail; }
            VHasNotBeenConfirmed(virtualOrderDetail);
            return virtualOrderDetail;
        }

        public VirtualOrderDetail VDeleteObject(VirtualOrderDetail virtualOrderDetail)
        {
            VHasNotBeenConfirmed(virtualOrderDetail);
            return virtualOrderDetail;
        }

        public VirtualOrderDetail VConfirmObject(VirtualOrderDetail virtualOrderDetail)
        {
            VHasConfirmationDate(virtualOrderDetail);
            if (!isValid(virtualOrderDetail)) { return virtualOrderDetail; }
            VHasNotBeenConfirmed(virtualOrderDetail);
            return virtualOrderDetail;
        }

        public VirtualOrderDetail VUnconfirmObject(VirtualOrderDetail virtualOrderDetail, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService, IItemService _itemService)
        {
            VHasBeenConfirmed(virtualOrderDetail);
            if (!isValid(virtualOrderDetail)) { return virtualOrderDetail; }
            VHasItemPendingDelivery(virtualOrderDetail, _itemService);
            if (!isValid(virtualOrderDetail)) { return virtualOrderDetail; }
            VHasNoTemporaryDeliveryOrderDetail(virtualOrderDetail, _temporaryDeliveryOrderDetailService);
            return virtualOrderDetail;
        }

        public bool ValidCreateObject(VirtualOrderDetail virtualOrderDetail,  IVirtualOrderDetailService _virtualOrderDetailService, IVirtualOrderService _virtualOrderService, IItemService _itemService)
        {
            VCreateObject(virtualOrderDetail, _virtualOrderDetailService, _virtualOrderService, _itemService);
            return isValid(virtualOrderDetail);
        }

        public bool ValidUpdateObject(VirtualOrderDetail virtualOrderDetail,  IVirtualOrderDetailService _virtualOrderDetailService, IVirtualOrderService _virtualOrderService, IItemService _itemService)
        {
            virtualOrderDetail.Errors.Clear();
            VUpdateObject(virtualOrderDetail, _virtualOrderDetailService, _virtualOrderService, _itemService);
            return isValid(virtualOrderDetail);
        }

        public bool ValidDeleteObject(VirtualOrderDetail virtualOrderDetail)
        {
            virtualOrderDetail.Errors.Clear();
            VDeleteObject(virtualOrderDetail);
            return isValid(virtualOrderDetail);
        }

        public bool ValidConfirmObject(VirtualOrderDetail virtualOrderDetail)
        {
            virtualOrderDetail.Errors.Clear();
            VConfirmObject(virtualOrderDetail);
            return isValid(virtualOrderDetail);
        }

        public bool ValidUnconfirmObject(VirtualOrderDetail virtualOrderDetail, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService, IItemService _itemService)
        {
            virtualOrderDetail.Errors.Clear();
            VUnconfirmObject(virtualOrderDetail, _temporaryDeliveryOrderDetailService, _itemService);
            return isValid(virtualOrderDetail);
        }

        public bool isValid(VirtualOrderDetail obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(VirtualOrderDetail obj)
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