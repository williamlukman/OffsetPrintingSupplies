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
    public class PurchaseReceivalDetailValidator : IPurchaseReceivalDetailValidator
    {
        public PurchaseReceivalDetail VHasPurchaseReceival(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseReceivalService _purchaseReceivalService)
        {
            PurchaseReceival purchaseReceival = _purchaseReceivalService.GetObjectById(purchaseReceivalDetail.PurchaseReceivalId);
            if (purchaseReceival == null)
            {
                purchaseReceivalDetail.Errors.Add("PurchaseReceivalId", "Tidak terasosiasi dengan purchase receival");
            }
            return purchaseReceivalDetail;
        }

        public PurchaseReceivalDetail VPurchaseReceivalHasNotBeenConfirmed(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseReceivalService _purchaseReceivalService)
        {
            PurchaseReceival purchaseReceival = _purchaseReceivalService.GetObjectById(purchaseReceivalDetail.PurchaseReceivalId);
            if (purchaseReceival.IsConfirmed)
            {
                purchaseReceivalDetail.Errors.Add("Generic", "Sudah dikonfirmasi");
            }
            return purchaseReceivalDetail;
        }

        public PurchaseReceivalDetail VHasItem(PurchaseReceivalDetail purchaseReceivalDetail, IItemService _itemService)
        {
            Item item = _itemService.GetObjectById(purchaseReceivalDetail.ItemId);
            if (item == null)
            {
                purchaseReceivalDetail.Errors.Add("ItemId", "Tidak terasosiasi dengan item");
            }
            return purchaseReceivalDetail;
        }

        public PurchaseReceivalDetail VNonNegativeQuantity(PurchaseReceivalDetail purchaseReceivalDetail)
        {
            if (purchaseReceivalDetail.Quantity < 0)
            {
                purchaseReceivalDetail.Errors.Add("Quantity", "Tidak boleh negatif");
            }
            return purchaseReceivalDetail;
        }

        public PurchaseReceivalDetail VHasPurchaseOrderDetail(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseOrderDetailService _purchaseOrderDetailService)
        {
            PurchaseOrderDetail purchaseOrderDetail = _purchaseOrderDetailService.GetObjectById(purchaseReceivalDetail.PurchaseOrderDetailId);
            if (purchaseOrderDetail == null)
            {
                purchaseReceivalDetail.Errors.Add("PurchaseOrderDetailId", "Tidak terasosiasi dengan purchase order detail");
            }
            return purchaseReceivalDetail;
        }

        public PurchaseReceivalDetail VUniquePurchaseOrderDetail(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseReceivalDetailService _purchaseReceivalDetailService, IItemService _itemService)
        {
            IList<PurchaseReceivalDetail> details = _purchaseReceivalDetailService.GetObjectsByPurchaseReceivalId(purchaseReceivalDetail.PurchaseReceivalId);
            foreach (var detail in details)
            {
                if (detail.PurchaseOrderDetailId == purchaseReceivalDetail.PurchaseOrderDetailId && detail.Id != purchaseReceivalDetail.Id)
                {
                    purchaseReceivalDetail.Errors.Add("Generic", "Tidak boleh memiliki lebih dari 2 Purchase Receival Detail dalam satu purchase receival");
                }
            }
            return purchaseReceivalDetail;
        }

        public PurchaseReceivalDetail VPurchaseOrderDetailHasBeenConfirmed(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseOrderDetailService _purchaseOrderDetailService)
        {
            PurchaseOrderDetail purchaseOrderDetail = _purchaseOrderDetailService.GetObjectById(purchaseReceivalDetail.PurchaseOrderDetailId);
            if (!purchaseOrderDetail.IsConfirmed)
            {
                purchaseReceivalDetail.Errors.Add("Generic", "Purchase Order Detail belum dikonfirmasi");
            }
            return purchaseReceivalDetail;
        }

        public PurchaseReceivalDetail VPurchaseReceivalAndPurchaseOrderDetailHaveTheSamePurchaseOrder(PurchaseReceivalDetail purchaseReceivalDetail,
                                                       IPurchaseReceivalService _purchaseReceivalService, IPurchaseOrderDetailService _purchaseOrderDetailService)
        {
            PurchaseOrderDetail purchaseOrderDetail = _purchaseOrderDetailService.GetObjectById(purchaseReceivalDetail.PurchaseOrderDetailId);
            PurchaseReceival purchaseReceival = _purchaseReceivalService.GetObjectById(purchaseReceivalDetail.PurchaseReceivalId);
            if (purchaseOrderDetail.PurchaseOrderId != purchaseReceival.PurchaseOrderId)
            {
                purchaseReceivalDetail.Errors.Add("Generic", "Purchase receival dan purchase order detail memiliki purchase order yang berbeda");
            }
            return purchaseReceivalDetail;
        }

        public PurchaseReceivalDetail VQuantityOfPurchaseReceivalDetailsIsLessThanOrEqualPurchaseOrderDetail(PurchaseReceivalDetail purchaseReceivalDetail,
                                                        IPurchaseReceivalDetailService _purchaseReceivalDetailService, IPurchaseOrderDetailService _purchaseOrderDetailService, bool CaseCreate)
        {
            PurchaseOrderDetail purchaseOrderDetail = _purchaseOrderDetailService.GetObjectById(purchaseReceivalDetail.PurchaseOrderDetailId);
            IList<PurchaseReceivalDetail> details = _purchaseReceivalDetailService.GetObjectsByPurchaseOrderDetailId(purchaseReceivalDetail.PurchaseOrderDetailId);

            decimal totalReceivalQuantity = 0;
            foreach (var detail in details)
            {
                if (!detail.IsConfirmed)
                {
                    totalReceivalQuantity += detail.Quantity;
                }
            }
            if (CaseCreate) { totalReceivalQuantity += purchaseReceivalDetail.Quantity; }
            if (totalReceivalQuantity > purchaseOrderDetail.PendingReceivalQuantity)
            {
                decimal maxquantity = purchaseOrderDetail.PendingReceivalQuantity - totalReceivalQuantity + purchaseReceivalDetail.Quantity;
                purchaseReceivalDetail.Errors.Add("Generic", "Quantity maximum adalah " + maxquantity);
            }
            return purchaseReceivalDetail;
        }

        public PurchaseReceivalDetail VHasNotBeenConfirmed(PurchaseReceivalDetail purchaseReceivalDetail)
        {
            if (purchaseReceivalDetail.IsConfirmed)
            {
                purchaseReceivalDetail.Errors.Add("Generic", "Tidak boleh sudah selesai");
            }
            return purchaseReceivalDetail;
        }

        public PurchaseReceivalDetail VHasBeenConfirmed(PurchaseReceivalDetail purchaseReceivalDetail)
        {
            if (!purchaseReceivalDetail.IsConfirmed)
            {
                purchaseReceivalDetail.Errors.Add("Generic", "Belum selesai");
            }
            return purchaseReceivalDetail;
        }

        public PurchaseReceivalDetail VHasItemQuantity(PurchaseReceivalDetail purchaseReceivalDetail, IItemService _itemService)
        {
            Item item = _itemService.GetObjectById(purchaseReceivalDetail.ItemId);
            if (item.PendingReceival < 0)
            {
                purchaseReceivalDetail.Errors.Add("Generic", "Item pendingReceival tidak boleh kurang dari 0");
            }
            if (item.Quantity < purchaseReceivalDetail.Quantity)
            {
                purchaseReceivalDetail.Errors.Add("Generic", "item quantity tidak boleh kurang dari quantity dari Purchase Receival Detail");
            }
            return purchaseReceivalDetail;
        }

        public PurchaseReceivalDetail VHasConfirmationDate(PurchaseReceivalDetail purchaseReceivalDetail)
        {
            if (purchaseReceivalDetail.ConfirmationDate == null)
            {
                purchaseReceivalDetail.Errors.Add("ConfirmationDate", "Tidak boleh kosong");
            }
            return purchaseReceivalDetail;
        }

        public PurchaseReceivalDetail VHasNoPurchaseInvoiceDetail(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseInvoiceDetailService _purchaseInvoiceDetailService)
        {
            IList<PurchaseInvoiceDetail> purchaseInvoiceDetails = _purchaseInvoiceDetailService.GetObjectsByPurchaseReceivalDetailId(purchaseReceivalDetail.Id);
            if (purchaseInvoiceDetails.Any())
            {
                purchaseReceivalDetail.Errors.Add("Generic", "Sudah memiliki purchase invoice detail");
            }
            return purchaseReceivalDetail;
        }
        
        public PurchaseReceivalDetail VCreateObject(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseReceivalDetailService _purchaseReceivalDetailService,
                                                    IPurchaseReceivalService _purchaseReceivalService, IPurchaseOrderDetailService _purchaseOrderDetailService, IItemService _itemService)
        {
            VHasPurchaseReceival(purchaseReceivalDetail, _purchaseReceivalService);
            if (!isValid(purchaseReceivalDetail)) return purchaseReceivalDetail;
            VPurchaseReceivalHasNotBeenConfirmed(purchaseReceivalDetail, _purchaseReceivalService);
            if (!isValid(purchaseReceivalDetail)) return purchaseReceivalDetail;
            VHasItem(purchaseReceivalDetail, _itemService);
            if (!isValid(purchaseReceivalDetail)) return purchaseReceivalDetail;
            VHasPurchaseOrderDetail(purchaseReceivalDetail, _purchaseOrderDetailService);
            if (!isValid(purchaseReceivalDetail)) return purchaseReceivalDetail;
            VPurchaseOrderDetailHasBeenConfirmed(purchaseReceivalDetail, _purchaseOrderDetailService);
            if (!isValid(purchaseReceivalDetail)) return purchaseReceivalDetail;
            VPurchaseReceivalAndPurchaseOrderDetailHaveTheSamePurchaseOrder(purchaseReceivalDetail, _purchaseReceivalService, _purchaseOrderDetailService);
            if (!isValid(purchaseReceivalDetail)) return purchaseReceivalDetail;
            VNonNegativeQuantity(purchaseReceivalDetail);
            if (!isValid(purchaseReceivalDetail)) return purchaseReceivalDetail;
            // specific parameter for create function
            VQuantityOfPurchaseReceivalDetailsIsLessThanOrEqualPurchaseOrderDetail(purchaseReceivalDetail, _purchaseReceivalDetailService, _purchaseOrderDetailService, true);
            if (!isValid(purchaseReceivalDetail)) return purchaseReceivalDetail;
            VUniquePurchaseOrderDetail(purchaseReceivalDetail, _purchaseReceivalDetailService, _itemService);
            return purchaseReceivalDetail;
        }

        public PurchaseReceivalDetail VUpdateObject(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseReceivalDetailService _purchaseReceivalDetailService,
                                                    IPurchaseReceivalService _purchaseReceivalService, IPurchaseOrderDetailService _purchaseOrderDetailService, IItemService _itemService)
        {
            VHasPurchaseReceival(purchaseReceivalDetail, _purchaseReceivalService);
            if (!isValid(purchaseReceivalDetail)) return purchaseReceivalDetail;
            VPurchaseReceivalHasNotBeenConfirmed(purchaseReceivalDetail, _purchaseReceivalService);
            if (!isValid(purchaseReceivalDetail)) return purchaseReceivalDetail;
            VHasItem(purchaseReceivalDetail, _itemService);
            if (!isValid(purchaseReceivalDetail)) return purchaseReceivalDetail;
            VHasPurchaseOrderDetail(purchaseReceivalDetail, _purchaseOrderDetailService);
            if (!isValid(purchaseReceivalDetail)) return purchaseReceivalDetail;
            VPurchaseOrderDetailHasBeenConfirmed(purchaseReceivalDetail, _purchaseOrderDetailService);
            if (!isValid(purchaseReceivalDetail)) return purchaseReceivalDetail;
            VPurchaseReceivalAndPurchaseOrderDetailHaveTheSamePurchaseOrder(purchaseReceivalDetail, _purchaseReceivalService, _purchaseOrderDetailService);
            if (!isValid(purchaseReceivalDetail)) return purchaseReceivalDetail;
            VNonNegativeQuantity(purchaseReceivalDetail);
            if (!isValid(purchaseReceivalDetail)) return purchaseReceivalDetail;
            // specific parameter for update function
            VQuantityOfPurchaseReceivalDetailsIsLessThanOrEqualPurchaseOrderDetail(purchaseReceivalDetail, _purchaseReceivalDetailService, _purchaseOrderDetailService, false);
            if (!isValid(purchaseReceivalDetail)) return purchaseReceivalDetail;
            VUniquePurchaseOrderDetail(purchaseReceivalDetail, _purchaseReceivalDetailService, _itemService);
            if (!isValid(purchaseReceivalDetail)) return purchaseReceivalDetail;
            VHasNotBeenConfirmed(purchaseReceivalDetail);
            return purchaseReceivalDetail;
        }

        public PurchaseReceivalDetail VDeleteObject(PurchaseReceivalDetail purchaseReceivalDetail)
        {
            VHasNotBeenConfirmed(purchaseReceivalDetail);
            return purchaseReceivalDetail;
        }

        public PurchaseReceivalDetail VConfirmObject(PurchaseReceivalDetail purchaseReceivalDetail,
                                                     IPurchaseReceivalDetailService _purchaseReceivalDetailService,
                                                     IPurchaseOrderDetailService _purchaseOrderDetailService)
        {
            VHasConfirmationDate(purchaseReceivalDetail);
            if (!isValid(purchaseReceivalDetail)) { return purchaseReceivalDetail; }
            VHasNotBeenConfirmed(purchaseReceivalDetail);
            if (!isValid(purchaseReceivalDetail)) { return purchaseReceivalDetail; }
            VQuantityOfPurchaseReceivalDetailsIsLessThanOrEqualPurchaseOrderDetail(purchaseReceivalDetail, _purchaseReceivalDetailService, _purchaseOrderDetailService, false);
            return purchaseReceivalDetail;
        }

        public PurchaseReceivalDetail VUnconfirmObject(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseInvoiceDetailService _purchaseInvoiceDetailService,
                                                       IItemService _itemService)
        {
            VHasItemQuantity(purchaseReceivalDetail, _itemService);
            if (!isValid(purchaseReceivalDetail)) { return purchaseReceivalDetail; }
            VHasNoPurchaseInvoiceDetail(purchaseReceivalDetail, _purchaseInvoiceDetailService);
            return purchaseReceivalDetail;
        }

        public bool ValidCreateObject(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseReceivalDetailService _purchaseReceivalDetailService,
                               IPurchaseReceivalService _purchaseReceivalService, IPurchaseOrderDetailService _purchaseOrderDetailService, IItemService _itemService)
        {
            VCreateObject(purchaseReceivalDetail, _purchaseReceivalDetailService, _purchaseReceivalService, _purchaseOrderDetailService, _itemService);
            return isValid(purchaseReceivalDetail);
        }

        public bool ValidUpdateObject(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseReceivalDetailService _purchaseReceivalDetailService,
                               IPurchaseReceivalService _purchaseReceivalService, IPurchaseOrderDetailService _purchaseOrderDetailService, IItemService _itemService)
        {
            purchaseReceivalDetail.Errors.Clear();
            VUpdateObject(purchaseReceivalDetail, _purchaseReceivalDetailService, _purchaseReceivalService, _purchaseOrderDetailService, _itemService);
            return isValid(purchaseReceivalDetail);
        }

        public bool ValidDeleteObject(PurchaseReceivalDetail purchaseReceivalDetail)
        {
            purchaseReceivalDetail.Errors.Clear();
            VDeleteObject(purchaseReceivalDetail);
            return isValid(purchaseReceivalDetail);
        }

        public bool ValidConfirmObject(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseReceivalDetailService _purchaseReceivalDetailService,
                                       IPurchaseOrderDetailService _purchaseOrderDetailService)
        {
            purchaseReceivalDetail.Errors.Clear();
            VConfirmObject(purchaseReceivalDetail, _purchaseReceivalDetailService, _purchaseOrderDetailService);
            return isValid(purchaseReceivalDetail);
        }

        public bool ValidUnconfirmObject(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseInvoiceDetailService _purchaseInvoiceDetailService, IItemService _itemService)
        {
            purchaseReceivalDetail.Errors.Clear();
            VUnconfirmObject(purchaseReceivalDetail, _purchaseInvoiceDetailService, _itemService);
            return isValid(purchaseReceivalDetail);
        }

        public bool isValid(PurchaseReceivalDetail obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(PurchaseReceivalDetail obj)
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