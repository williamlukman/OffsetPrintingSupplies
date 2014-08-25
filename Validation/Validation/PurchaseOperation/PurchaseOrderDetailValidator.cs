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
            PurchaseOrder purchaseOrder = _purchaseOrderService.GetObjectById(purchaseOrderDetail.PurchaseOrderId);
            if (purchaseOrder == null)
            {
                purchaseOrderDetail.Errors.Add("PurchaseOrderId", "Tidak terasosiasi dengan purchase order");
            }
            return purchaseOrderDetail;
        }

        public PurchaseOrderDetail VPurchaseOrderHasNotBeenConfirmed(PurchaseOrderDetail purchaseOrderDetail, IPurchaseOrderService _purchaseOrderService)
        {
            PurchaseOrder purchaseOrder = _purchaseOrderService.GetObjectById(purchaseOrderDetail.PurchaseOrderId);
            if (purchaseOrder.IsConfirmed)
            {
                purchaseOrderDetail.Errors.Add("Generic", "Sudah terkonfirmasi");
            }
            return purchaseOrderDetail;
        }

        public PurchaseOrderDetail VHasItem(PurchaseOrderDetail purchaseOrderDetail, IItemService _itemService)
        {
            Item item = _itemService.GetObjectById(purchaseOrderDetail.ItemId);
            if (item == null)
            {
                purchaseOrderDetail.Errors.Add("ItemId", "Tidak terasosiasi dengan item");
            }
            return purchaseOrderDetail;
        }

        public PurchaseOrderDetail VNonZeroNorNegativeQuantity(PurchaseOrderDetail purchaseOrderDetail)
        {
            if (purchaseOrderDetail.Quantity < 0)
            {
                purchaseOrderDetail.Errors.Add("Quantity", "Tidak boleh kurang dari 0");
            }
            return purchaseOrderDetail;
        }

        public PurchaseOrderDetail VNonNegativePrice(PurchaseOrderDetail purchaseOrderDetail)
        {
            if (purchaseOrderDetail.Price <= 0)
            {
                purchaseOrderDetail.Errors.Add("Price", "Tidak boleh kurang dari atau sama dengan 0");
            }
            return purchaseOrderDetail;
        }

        public PurchaseOrderDetail VUniquePurchaseOrderDetail(PurchaseOrderDetail purchaseOrderDetail, IPurchaseOrderDetailService _purchaseOrderDetailService, IItemService _itemService)
        {
            IList<PurchaseOrderDetail> details = _purchaseOrderDetailService.GetObjectsByPurchaseOrderId(purchaseOrderDetail.PurchaseOrderId);
            foreach (var detail in details)
            {
                if (detail.ItemId == purchaseOrderDetail.ItemId && detail.Id != purchaseOrderDetail.Id)
                {
                    purchaseOrderDetail.Errors.Add("Generic", "Detail tidak boleh memiliki Sku yang sama dalam 1 Purchase Order");
                    return purchaseOrderDetail;
                }
            }
            return purchaseOrderDetail;
        }

        public PurchaseOrderDetail VHasConfirmationDate(PurchaseOrderDetail purchaseOrderDetail)
        {
            if (purchaseOrderDetail.ConfirmationDate == null)
            {
                purchaseOrderDetail.Errors.Add("ConfirmationDate", "Tidak boleh kosong");
            }
            return purchaseOrderDetail;
        }

        public PurchaseOrderDetail VHasBeenConfirmed(PurchaseOrderDetail purchaseOrderDetail)
        {
            if (!purchaseOrderDetail.IsConfirmed)
            {
                purchaseOrderDetail.Errors.Add("Generic", "Belum selesai");
            }
            return purchaseOrderDetail;
        }

        public PurchaseOrderDetail VHasNotBeenConfirmed(PurchaseOrderDetail purchaseOrderDetail)
        {
            if (purchaseOrderDetail.IsConfirmed)
            {
                purchaseOrderDetail.Errors.Add("Generic", "Sudah selesai");
            }
            return purchaseOrderDetail;
        }

        public PurchaseOrderDetail VHasItemPendingReceival(PurchaseOrderDetail purchaseOrderDetail, IItemService _itemService)
        {
            Item item = _itemService.GetObjectById(purchaseOrderDetail.ItemId);
            if (item.PendingReceival < purchaseOrderDetail.Quantity)
            {
                purchaseOrderDetail.Errors.Add("Generic", "Item pending receival tidak boleh kurang dari quantity");
            }
            return purchaseOrderDetail;
        }

        public PurchaseOrderDetail VHasNoPurchaseReceivalDetail(PurchaseOrderDetail purchaseOrderDetail, IPurchaseReceivalDetailService _purchaseReceivalDetailService)
        {
            IList<PurchaseReceivalDetail> purchaseReceivalDetails = _purchaseReceivalDetailService.GetObjectsByPurchaseOrderDetailId(purchaseOrderDetail.Id);
            if (purchaseReceivalDetails.Any())
            {
                purchaseOrderDetail.Errors.Add("Generic", "Tidak boleh terasosiasi dengan purchase receival");
            }
            return purchaseOrderDetail;
        }

        public PurchaseOrderDetail VCreateObject(PurchaseOrderDetail purchaseOrderDetail, IPurchaseOrderDetailService _purchaseOrderDetailService, IPurchaseOrderService _purchaseOrderService, IItemService _itemService)
        {
            VHasPurchaseOrder(purchaseOrderDetail, _purchaseOrderService);
            if (!isValid(purchaseOrderDetail)) { return purchaseOrderDetail; }
            VPurchaseOrderHasNotBeenConfirmed(purchaseOrderDetail, _purchaseOrderService);
            if (!isValid(purchaseOrderDetail)) { return purchaseOrderDetail; }
            VHasItem(purchaseOrderDetail, _itemService);
            if (!isValid(purchaseOrderDetail)) { return purchaseOrderDetail; }
            VNonZeroNorNegativeQuantity(purchaseOrderDetail);
            if (!isValid(purchaseOrderDetail)) { return purchaseOrderDetail; }
            VNonNegativePrice(purchaseOrderDetail);
            if (!isValid(purchaseOrderDetail)) { return purchaseOrderDetail; }
            VUniquePurchaseOrderDetail(purchaseOrderDetail, _purchaseOrderDetailService, _itemService);
            return purchaseOrderDetail;
        }

        public PurchaseOrderDetail VUpdateObject(PurchaseOrderDetail purchaseOrderDetail, IPurchaseOrderDetailService _purchaseOrderDetailService, IPurchaseOrderService _purchaseOrderService, IItemService _itemService)
        {
            VCreateObject(purchaseOrderDetail, _purchaseOrderDetailService, _purchaseOrderService, _itemService);
            if (!isValid(purchaseOrderDetail)) { return purchaseOrderDetail; }
            VHasNotBeenConfirmed(purchaseOrderDetail);
            return purchaseOrderDetail;
        }

        public PurchaseOrderDetail VDeleteObject(PurchaseOrderDetail purchaseOrderDetail)
        {
            VHasNotBeenConfirmed(purchaseOrderDetail);
            return purchaseOrderDetail;
        }

        public PurchaseOrderDetail VConfirmObject(PurchaseOrderDetail purchaseOrderDetail)
        {
            VHasConfirmationDate(purchaseOrderDetail);
            if (!isValid(purchaseOrderDetail)) { return purchaseOrderDetail; }
            VHasNotBeenConfirmed(purchaseOrderDetail);
            return purchaseOrderDetail;
        }

        public PurchaseOrderDetail VUnconfirmObject(PurchaseOrderDetail purchaseOrderDetail, IPurchaseReceivalDetailService _purchaseReceivalDetailService, IItemService _itemService)
        {
            VHasConfirmationDate(purchaseOrderDetail);
            if (!isValid(purchaseOrderDetail)) { return purchaseOrderDetail; }
            VHasItemPendingReceival(purchaseOrderDetail, _itemService);
            if (!isValid(purchaseOrderDetail)) { return purchaseOrderDetail; }
            VHasNoPurchaseReceivalDetail(purchaseOrderDetail, _purchaseReceivalDetailService);
            return purchaseOrderDetail;
        }

        public bool ValidCreateObject(PurchaseOrderDetail purchaseOrderDetail, IPurchaseOrderDetailService _purchaseOrderDetailService, IPurchaseOrderService _purchaseOrderService, IItemService _itemService)
        {
            VCreateObject(purchaseOrderDetail, _purchaseOrderDetailService, _purchaseOrderService, _itemService);
            return isValid(purchaseOrderDetail);
        }

        public bool ValidUpdateObject(PurchaseOrderDetail purchaseOrderDetail,  IPurchaseOrderDetailService _purchaseOrderDetailService, IPurchaseOrderService _purchaseOrderService, IItemService _itemService)
        {
            purchaseOrderDetail.Errors.Clear();
            VUpdateObject(purchaseOrderDetail, _purchaseOrderDetailService, _purchaseOrderService, _itemService);
            return isValid(purchaseOrderDetail);
        }

        public bool ValidDeleteObject(PurchaseOrderDetail purchaseOrderDetail)
        {
            purchaseOrderDetail.Errors.Clear();
            VDeleteObject(purchaseOrderDetail);
            return isValid(purchaseOrderDetail);
        }

        public bool ValidConfirmObject(PurchaseOrderDetail purchaseOrderDetail)
        {
            purchaseOrderDetail.Errors.Clear();
            VConfirmObject(purchaseOrderDetail);
            return isValid(purchaseOrderDetail);
        }

        public bool ValidUnconfirmObject(PurchaseOrderDetail purchaseOrderDetail, IPurchaseReceivalDetailService _purchaseReceivalDetailService, IItemService _itemService)
        {
            purchaseOrderDetail.Errors.Clear();
            VUnconfirmObject(purchaseOrderDetail, _purchaseReceivalDetailService, _itemService);
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