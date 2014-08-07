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
    public class PurchaseOrderDetailValidator : IPurchaseOrderDetailValidator
    {
        public PurchaseOrderDetail VHasPurchaseOrder(PurchaseOrderDetail purchaseOrderDetail, IPurchaseOrderService _purchaseOrderService)
        {
            PurchaseOrder po = _purchaseOrderService.GetObjectById(purchaseOrderDetail.PurchaseOrderId);
            if (po == null)
            {
                purchaseOrderDetail.Errors.Add("PurchaseOrder", "Tidak boleh tidak ada");
            }
            return purchaseOrderDetail;
        }

        public PurchaseOrderDetail VHasItem(PurchaseOrderDetail purchaseOrderDetail, IItemService _itemService)
        {
            Item item = _itemService.GetObjectById(purchaseOrderDetail.ItemId);
            if (item == null)
            {
                purchaseOrderDetail.Errors.Add("Item", "Tidak boleh tidak ada");
            }
            return purchaseOrderDetail;
        }

        public PurchaseOrderDetail VQuantity(PurchaseOrderDetail purchaseOrderDetail)
        {
            if (purchaseOrderDetail.Quantity < 0)
            {
                purchaseOrderDetail.Errors.Add("Quantity", "Tidak boleh kurang dari 0");
            }
            return purchaseOrderDetail;
        }

        public PurchaseOrderDetail VPrice(PurchaseOrderDetail purchaseOrderDetail)
        {
            if (purchaseOrderDetail.Price <= 0)
            {
                purchaseOrderDetail.Errors.Add("Price", "Tidak boleh kurang dari atau sama dengan 0");
            }
            return purchaseOrderDetail;
        }

        public PurchaseOrderDetail VUniquePurchaseOrderDetail(PurchaseOrderDetail purchaseOrderDetail, IPurchaseOrderDetailService _purchaseOrderDetails, IItemService _itemService)
        {
            IList<PurchaseOrderDetail> details = _purchaseOrderDetails.GetObjectsByPurchaseOrderId(purchaseOrderDetail.PurchaseOrderId);
            foreach (var detail in details)
            {
                if (detail.ItemId == purchaseOrderDetail.ItemId && detail.Id != purchaseOrderDetail.Id)
                {
                    purchaseOrderDetail.Errors.Add("PurchaseOrderDetail", "Tidak boleh memiliki Sku yang sama dalam 1 Purchase Order");
                    return purchaseOrderDetail;
                }
            }
            return purchaseOrderDetail;
        }

        public PurchaseOrderDetail VHasBeenFinished(PurchaseOrderDetail purchaseOrderDetail)
        {
            if (!purchaseOrderDetail.IsFinished)
            {
                purchaseOrderDetail.Errors.Add("Generic", "Belum selesai");
            }
            return purchaseOrderDetail;
        }

        public PurchaseOrderDetail VHasNotBeenFinished(PurchaseOrderDetail purchaseOrderDetail)
        {
            if (purchaseOrderDetail.IsFinished)
            {
                purchaseOrderDetail.Errors.Add("Generic", "Sudah selesai");
            }
            return purchaseOrderDetail;
        }

        public PurchaseOrderDetail VPurchaseOrderHasNotBeenCompleted(PurchaseOrderDetail purchaseOrderDetail, IPurchaseOrderService _purchaseOrderService)
        {
            PurchaseOrder purchaseOrder = _purchaseOrderService.GetObjectById(purchaseOrderDetail.PurchaseOrderId);
            if (purchaseOrder.IsCompleted)
            {
                purchaseOrderDetail.Errors.Add("Generic", "Purchase order sudah complete");
            }
            return purchaseOrderDetail;
        }

        public PurchaseOrderDetail VHasItemPendingReceival(PurchaseOrderDetail purchaseOrderDetail, IItemService _itemService)
        {
            Item item = _itemService.GetObjectById(purchaseOrderDetail.ItemId);
            if (item.PendingReceival < purchaseOrderDetail.Quantity)
            {
                purchaseOrderDetail.Errors.Add("Item.PendingReceival", "Tidak boleh kurang dari quantity");
            }
            return purchaseOrderDetail;
        }

        public PurchaseOrderDetail VHasNoPurchaseReceivalDetail(PurchaseOrderDetail purchaseOrderDetail, IPurchaseReceivalDetailService _purchaseReceivalDetailService)
        {
            PurchaseReceivalDetail purchaseReceivalDetail = _purchaseReceivalDetailService.GetObjectByPurchaseOrderDetailId(purchaseOrderDetail.Id);
            if (purchaseReceivalDetail != null)
            {
                purchaseOrderDetail.Errors.Add("Generic", "Tidak boleh terasosiasi dengan purchase receival");
            }
            return purchaseOrderDetail;
        }

        public PurchaseOrderDetail VPurchaseReceivalDetailHasBeenFinished(PurchaseOrderDetail purchaseOrderDetail, IPurchaseReceivalDetailService _purchaseReceivalDetailService)
        {
            PurchaseReceivalDetail purchaseReceivalDetail = _purchaseReceivalDetailService.GetObjectByPurchaseOrderDetailId(purchaseOrderDetail.Id);
            if (!purchaseReceivalDetail.IsFinished)
            {
                purchaseOrderDetail.Errors.Add("Generic", "Purchase Receival belum selesai");
            }
            return purchaseOrderDetail;
        }

        public PurchaseOrderDetail VCreateObject(PurchaseOrderDetail purchaseOrderDetail, IPurchaseOrderDetailService _purchaseOrderDetails, IPurchaseOrderService _purchaseOrderService, IItemService _itemService)
        {
            VHasPurchaseOrder(purchaseOrderDetail, _purchaseOrderService);
            if (!isValid(purchaseOrderDetail)) { return purchaseOrderDetail; }
            VHasItem(purchaseOrderDetail, _itemService);
            if (!isValid(purchaseOrderDetail)) { return purchaseOrderDetail; }
            VQuantity(purchaseOrderDetail);
            if (!isValid(purchaseOrderDetail)) { return purchaseOrderDetail; }
            VPrice(purchaseOrderDetail);
            if (!isValid(purchaseOrderDetail)) { return purchaseOrderDetail; }
            VUniquePurchaseOrderDetail(purchaseOrderDetail, _purchaseOrderDetails, _itemService);
            return purchaseOrderDetail;
        }

        public PurchaseOrderDetail VUpdateObject(PurchaseOrderDetail purchaseOrderDetail, IPurchaseOrderDetailService _purchaseOrderDetails, IPurchaseOrderService _purchaseOrderService, IItemService _itemService)
        {
            VCreateObject(purchaseOrderDetail, _purchaseOrderDetails, _purchaseOrderService, _itemService);
            if (!isValid(purchaseOrderDetail)) { return purchaseOrderDetail; }
            VHasNotBeenFinished(purchaseOrderDetail);
            return purchaseOrderDetail;
        }

        public PurchaseOrderDetail VDeleteObject(PurchaseOrderDetail purchaseOrderDetail)
        {
            VHasNotBeenFinished(purchaseOrderDetail);
            return purchaseOrderDetail;
        }

        public PurchaseOrderDetail VFinishObject(PurchaseOrderDetail purchaseOrderDetail)
        {
            VHasNotBeenFinished(purchaseOrderDetail);
            return purchaseOrderDetail;
        }

        public PurchaseOrderDetail VUnfinishObject(PurchaseOrderDetail purchaseOrderDetail, IPurchaseOrderService _purchaseOrderService, IPurchaseOrderDetailService _purchaseOrderDetails, IPurchaseReceivalDetailService _purchaseReceivalDetails, IItemService _itemService)
        {
            VPurchaseOrderHasNotBeenCompleted(purchaseOrderDetail, _purchaseOrderService);
            if (!isValid(purchaseOrderDetail)) { return purchaseOrderDetail; }
            VHasItemPendingReceival(purchaseOrderDetail, _itemService);
            if (!isValid(purchaseOrderDetail)) { return purchaseOrderDetail; }
            VHasNoPurchaseReceivalDetail(purchaseOrderDetail, _purchaseReceivalDetails);
            return purchaseOrderDetail;
        }

        public bool ValidCreateObject(PurchaseOrderDetail purchaseOrderDetail, IPurchaseOrderDetailService _purchaseOrderDetails, IPurchaseOrderService _purchaseOrderService, IItemService _itemService)
        {
            VCreateObject(purchaseOrderDetail, _purchaseOrderDetails, _purchaseOrderService, _itemService);
            return isValid(purchaseOrderDetail);
        }

        public bool ValidUpdateObject(PurchaseOrderDetail purchaseOrderDetail,  IPurchaseOrderDetailService _purchaseOrderDetails, IPurchaseOrderService _purchaseOrderService, IItemService _itemService)
        {
            purchaseOrderDetail.Errors.Clear();
            VUpdateObject(purchaseOrderDetail, _purchaseOrderDetails, _purchaseOrderService, _itemService);
            return isValid(purchaseOrderDetail);
        }

        public bool ValidDeleteObject(PurchaseOrderDetail purchaseOrderDetail)
        {
            purchaseOrderDetail.Errors.Clear();
            VDeleteObject(purchaseOrderDetail);
            return isValid(purchaseOrderDetail);
        }

        public bool ValidFinishObject(PurchaseOrderDetail purchaseOrderDetail)
        {
            purchaseOrderDetail.Errors.Clear();
            VFinishObject(purchaseOrderDetail);
            return isValid(purchaseOrderDetail);
        }

        public bool ValidUnfinishObject(PurchaseOrderDetail purchaseOrderDetail, IPurchaseOrderService _purchaseOrderService, IPurchaseOrderDetailService _purchaseOrderDetailService, IPurchaseReceivalDetailService _purchaseReceivalDetailService, IItemService _itemService)
        {
            purchaseOrderDetail.Errors.Clear();
            VUnfinishObject(purchaseOrderDetail, _purchaseOrderService, _purchaseOrderDetailService, _purchaseReceivalDetailService, _itemService);
            return isValid(purchaseOrderDetail);
        }

        public bool isValid(PurchaseOrderDetail obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(PurchaseOrderDetail obj)
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