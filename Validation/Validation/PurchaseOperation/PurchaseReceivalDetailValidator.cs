using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Text.RegularExpressions;
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
            PurchaseReceival pr = _purchaseReceivalService.GetObjectById(purchaseReceivalDetail.PurchaseReceivalId);
            if (pr == null)
            {
                purchaseReceivalDetail.Errors.Add("PurchaseReceival", "Tidak boleh tidak ada");
            }
            return purchaseReceivalDetail;
        }

        public PurchaseReceivalDetail VHasItem(PurchaseReceivalDetail purchaseReceivalDetail, IItemService _itemService)
        {
            Item item = _itemService.GetObjectById(purchaseReceivalDetail.ItemId);
            if (item == null)
            {
                purchaseReceivalDetail.Errors.Add("Item", "Tidak boleh tidak ada");
            }
            return purchaseReceivalDetail;
        }

        public PurchaseReceivalDetail VCustomer(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseReceivalService _purchaseReceivalService, IPurchaseOrderService _purchaseOrderService, IPurchaseOrderDetailService _purchaseOrderDetailService, ICustomerService _customerService)
        {
            PurchaseReceival pr = _purchaseReceivalService.GetObjectById(purchaseReceivalDetail.PurchaseReceivalId);
            PurchaseOrderDetail pod = _purchaseOrderDetailService.GetObjectById(purchaseReceivalDetail.PurchaseOrderDetailId);
            if (pod == null)
            {
                purchaseReceivalDetail.Errors.Add("PurchaseOrderDetail", "Tidak boleh tidak ada");
                return purchaseReceivalDetail;
            }
            PurchaseOrder po = _purchaseOrderService.GetObjectById(pod.PurchaseOrderId);
            if (po.CustomerId != pr.CustomerId)
            {
                purchaseReceivalDetail.Errors.Add("Customer", "Tidak boleh merupakan kustomer yang berbeda dengan Purchase Order");
            }
            return purchaseReceivalDetail;
        }

        public PurchaseReceivalDetail VQuantityCreate(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseOrderDetailService _purchaseOrderDetailService)
        {
            PurchaseOrderDetail pod = _purchaseOrderDetailService.GetObjectById(purchaseReceivalDetail.PurchaseOrderDetailId);
            if (pod == null)
            {
                purchaseReceivalDetail.Errors.Add("PurchaseOrderDetail", "Tidak boleh tidak ada");
                return purchaseReceivalDetail;
            }
            if (purchaseReceivalDetail.Quantity <= 0)
            {
                purchaseReceivalDetail.Errors.Add("Quantity", "Tidak boleh kurang dari atau sama dengan 0");
                return purchaseReceivalDetail;
            }
            if (purchaseReceivalDetail.Quantity > pod.Quantity)
            {
                purchaseReceivalDetail.Errors.Add("Quantity", "Tidak boleh lebih dari quantity dari Purchase Order Detail");
                return purchaseReceivalDetail;
            }
            return purchaseReceivalDetail;
        }

        public PurchaseReceivalDetail VQuantityUpdate(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseOrderDetailService _purchaseOrderDetailService)
        {
            PurchaseOrderDetail pod = _purchaseOrderDetailService.GetObjectById(purchaseReceivalDetail.PurchaseOrderDetailId);
            if (purchaseReceivalDetail.Quantity <= 0)
            {
                purchaseReceivalDetail.Errors.Add("Quantity", "Tidak boleh kurang dari atau sama dengan 0");
            }
            if (purchaseReceivalDetail.Quantity > pod.Quantity)
            {
                purchaseReceivalDetail.Errors.Add("Quantity", "Tidak boleh lebih besar dari quantity dari Purchase Order Detail");
            }
            return purchaseReceivalDetail;
        }

        public PurchaseReceivalDetail VHasPurchaseOrderDetail(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseOrderDetailService _purchaseOrderDetailService)
        {
            PurchaseOrderDetail pod = _purchaseOrderDetailService.GetObjectById(purchaseReceivalDetail.PurchaseOrderDetailId);
            if (pod == null)
            {
                purchaseReceivalDetail.Errors.Add("PurchaseOrderDetail", "Tidak boleh tidak ada");
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
                    purchaseReceivalDetail.Errors.Add("PurchaseOrderDetail", "Tidak boleh memiliki lebih dari 2 Purchase Receival Detail");
                }
            }
            return purchaseReceivalDetail;
        }

        public PurchaseReceivalDetail VHasNotBeenFinished(PurchaseReceivalDetail purchaseReceivalDetail)
        {
            if (purchaseReceivalDetail.IsFinished)
            {
                purchaseReceivalDetail.Errors.Add("Generic", "Tidak boleh sudah selesai");
            }
            return purchaseReceivalDetail;
        }

        public PurchaseReceivalDetail VHasBeenFinished(PurchaseReceivalDetail purchaseReceivalDetail)
        {
            if (!purchaseReceivalDetail.IsFinished)
            {
                purchaseReceivalDetail.Errors.Add("Generic", "Belum selesai");
            }
            return purchaseReceivalDetail;
        }

        public PurchaseReceivalDetail VPurchaseReceivalHasNotBeenCompleted(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseReceivalService _purchaseReceivalService)
        {
            PurchaseReceival purchaseReceival = _purchaseReceivalService.GetObjectById(purchaseReceivalDetail.PurchaseReceivalId);
            if (purchaseReceival.IsCompleted)
            {
                purchaseReceivalDetail.Errors.Add("Generic", "Purchase receival sudah complete");
            }
            return purchaseReceivalDetail;
        }

        public PurchaseReceivalDetail VHasItemQuantity(PurchaseReceivalDetail purchaseReceivalDetail, IItemService _itemService)
        {
            Item item = _itemService.GetObjectById(purchaseReceivalDetail.ItemId);
            if (item.PendingReceival < 0)
            {
                purchaseReceivalDetail.Errors.Add("Item.PendingReceival", "Tidak boleh kurang dari 0");
            }
            if (item.Quantity < purchaseReceivalDetail.Quantity)
            {
                purchaseReceivalDetail.Errors.Add("item.Quantity", "Tidak boleh kurang dari quantity dari Purchase Receival Detail");
            }
            return purchaseReceivalDetail;
        }

        public PurchaseReceivalDetail VCreateObject(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseReceivalDetailService _purchaseReceivalDetailService, IPurchaseReceivalService _purchaseReceivalService,
                                                    IPurchaseOrderDetailService _purchaseOrderDetailService, IPurchaseOrderService _purchaseOrderService, IItemService _itemService, ICustomerService _customerService)
        {
            VHasPurchaseReceival(purchaseReceivalDetail, _purchaseReceivalService);
            if (!isValid(purchaseReceivalDetail)) return purchaseReceivalDetail;
            VHasItem(purchaseReceivalDetail, _itemService);
            if (!isValid(purchaseReceivalDetail)) return purchaseReceivalDetail;
            VCustomer(purchaseReceivalDetail, _purchaseReceivalService, _purchaseOrderService, _purchaseOrderDetailService, _customerService);
            if (!isValid(purchaseReceivalDetail)) return purchaseReceivalDetail;
            VQuantityCreate(purchaseReceivalDetail, _purchaseOrderDetailService);
            if (!isValid(purchaseReceivalDetail)) return purchaseReceivalDetail;
            VUniquePurchaseOrderDetail(purchaseReceivalDetail, _purchaseReceivalDetailService, _itemService);
            return purchaseReceivalDetail;
        }

        public PurchaseReceivalDetail VUpdateObject(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseReceivalDetailService _purchaseReceivalDetailService, IPurchaseReceivalService _purchaseReceivalService,
                                                    IPurchaseOrderDetailService _purchaseOrderDetailService, IPurchaseOrderService _purchaseOrderService, IItemService _itemService, ICustomerService _customerService)
        {
            VCreateObject(purchaseReceivalDetail, _purchaseReceivalDetailService, _purchaseReceivalService, _purchaseOrderDetailService, _purchaseOrderService, _itemService, _customerService);
            if (!isValid(purchaseReceivalDetail)) return purchaseReceivalDetail;
            VHasNotBeenFinished(purchaseReceivalDetail);
            return purchaseReceivalDetail;
        }

        public PurchaseReceivalDetail VDeleteObject(PurchaseReceivalDetail purchaseReceivalDetail)
        {
            VHasNotBeenFinished(purchaseReceivalDetail);
            return purchaseReceivalDetail;
        }

        public PurchaseReceivalDetail VFinishObject(PurchaseReceivalDetail purchaseReceivalDetail)
        {
            VHasNotBeenFinished(purchaseReceivalDetail);
            return purchaseReceivalDetail;
        }

        public PurchaseReceivalDetail VUnfinishObject(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseReceivalService _purchaseReceivalService, IPurchaseReceivalDetailService _purchaseReceivalDetailService, IItemService _itemService)
        {
            VPurchaseReceivalHasNotBeenCompleted(purchaseReceivalDetail, _purchaseReceivalService);
            if (!isValid(purchaseReceivalDetail)) return purchaseReceivalDetail;
            VHasItemQuantity(purchaseReceivalDetail, _itemService);
            return purchaseReceivalDetail;
        }

        public bool ValidCreateObject(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseReceivalDetailService _purchaseReceivalDetailService, IPurchaseReceivalService _purchaseReceivalService,
                                                    IPurchaseOrderDetailService _purchaseOrderDetailService, IPurchaseOrderService _purchaseOrderService, IItemService _itemService, ICustomerService _customerService)
        {
            VCreateObject(purchaseReceivalDetail, _purchaseReceivalDetailService, _purchaseReceivalService, _purchaseOrderDetailService, _purchaseOrderService, _itemService, _customerService);
            return isValid(purchaseReceivalDetail);
        }

        public bool ValidUpdateObject(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseReceivalDetailService _purchaseReceivalDetailService, IPurchaseReceivalService _purchaseReceivalService,
                                                    IPurchaseOrderDetailService _purchaseOrderDetailService, IPurchaseOrderService _purchaseOrderService, IItemService _itemService, ICustomerService _customerService)
        {
            purchaseReceivalDetail.Errors.Clear();
            VUpdateObject(purchaseReceivalDetail, _purchaseReceivalDetailService, _purchaseReceivalService, _purchaseOrderDetailService, _purchaseOrderService, _itemService, _customerService);
            return isValid(purchaseReceivalDetail);
        }

        public bool ValidDeleteObject(PurchaseReceivalDetail purchaseReceivalDetail)
        {
            purchaseReceivalDetail.Errors.Clear();
            VDeleteObject(purchaseReceivalDetail);
            return isValid(purchaseReceivalDetail);
        }

        public bool ValidFinishObject(PurchaseReceivalDetail purchaseReceivalDetail)
        {
            purchaseReceivalDetail.Errors.Clear();
            VFinishObject(purchaseReceivalDetail);
            return isValid(purchaseReceivalDetail);
        }

        public bool ValidUnfinishObject(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseReceivalService _purchaseReceivalService, IPurchaseReceivalDetailService _purchaseReceivalDetailService, IItemService _itemService)
        {
            purchaseReceivalDetail.Errors.Clear();
            VUnfinishObject(purchaseReceivalDetail, _purchaseReceivalService, _purchaseReceivalDetailService, _itemService);
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