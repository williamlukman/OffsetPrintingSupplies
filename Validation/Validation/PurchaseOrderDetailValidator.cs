using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Core.Interface.Validation;
using Core.DomainModel;
using Core.Interface.Service;
using Core.Constant;

namespace Validation.Validation
{
    public class PurchaseOrderDetailValidator : IPurchaseOrderDetailValidator
    {
        public PurchaseOrderDetail VHasPurchaseOrder(PurchaseOrderDetail pod, IPurchaseOrderService _pos)
        {
            PurchaseOrder po = _pos.GetObjectById(pod.PurchaseOrderId);
            if (po == null)
            {
                pod.Errors.Add("PurchaseOrder", "Tidak boleh tidak ada");
            }
            return pod;
        }

        public PurchaseOrderDetail VHasItem(PurchaseOrderDetail pod, IItemService _is)
        {
            Item item = _is.GetObjectById(pod.ItemId);
            if (item == null)
            {
                pod.Errors.Add("Item", "Tidak boleh tidak ada");
            }
            return pod;
        }

        public PurchaseOrderDetail VQuantity(PurchaseOrderDetail pod)
        {
            if (pod.Quantity < 0)
            {
                pod.Errors.Add("Quantity", "Tidak boleh kurang dari 0");
            }
            return pod;
        }

        public PurchaseOrderDetail VPrice(PurchaseOrderDetail pod)
        {
            if (pod.Price <= 0)
            {
                pod.Errors.Add("Price", "Tidak boleh kurang dari atau sama dengan 0");
            }
            return pod;
        }

        public PurchaseOrderDetail VUniquePOD(PurchaseOrderDetail pod, IPurchaseOrderDetailService _pods, IItemService _is)
        {
            IList<PurchaseOrderDetail> details = _pods.GetObjectsByPurchaseOrderId(pod.PurchaseOrderId);
            foreach (var detail in details)
            {
                if (detail.ItemId == pod.ItemId && detail.Id != pod.Id)
                {
                    pod.Errors.Add("PurchaseOrderDetail", "Tidak boleh memiliki Sku yang sama dalam 1 Purchase Order");
                    return pod;
                }
            }
            return pod;
        }

        public PurchaseOrderDetail VIsConfirmed(PurchaseOrderDetail pod)
        {
            if (pod.IsConfirmed)
            {
                pod.Errors.Add("PurchaseOrderDetail", "Tidak boleh sudah dikonfirmasi");
            }
            return pod;
        }

        public PurchaseOrderDetail VHasItemPendingReceival(PurchaseOrderDetail pod, IItemService _is)
        {
            Item item = _is.GetObjectById(pod.ItemId);
            if (item.PendingReceival < pod.Quantity)
            {
                pod.Errors.Add("Item.PendingReceival", "Tidak boleh kurang dari quantity");
            }
            return pod;
        }

        public PurchaseOrderDetail VConfirmedPurchaseReceival(PurchaseOrderDetail pod, IPurchaseReceivalDetailService _prds)
        {
            PurchaseReceivalDetail prd = _prds.GetObjectByPurchaseOrderDetailId(pod.Id);
            if (prd == null)
            {
                return pod;
            }
            if (prd.IsConfirmed)
            {
                pod.Errors.Add("PurchaseReceival", "Tidak boleh sudah dikonfirmasi");
            }
            return pod;
        }

        public PurchaseOrderDetail VCreateObject(PurchaseOrderDetail pod, IPurchaseOrderDetailService _pods, IPurchaseOrderService _pos, IItemService _is)
        {
            VHasPurchaseOrder(pod, _pos);
            if (!isValid(pod)) { return pod; }
            VHasItem(pod, _is);
            if (!isValid(pod)) { return pod; }
            VQuantity(pod);
            if (!isValid(pod)) { return pod; }
            VPrice(pod);
            if (!isValid(pod)) { return pod; }
            VUniquePOD(pod, _pods, _is);
            return pod;
        }

        public PurchaseOrderDetail VUpdateObject(PurchaseOrderDetail pod, IPurchaseOrderDetailService _pods, IPurchaseOrderService _pos, IItemService _is)
        {
            VHasPurchaseOrder(pod, _pos);
            if (!isValid(pod)) { return pod; }
            VHasItem(pod, _is);
            if (!isValid(pod)) { return pod; }
            VQuantity(pod);
            if (!isValid(pod)) { return pod; }
            VPrice(pod);
            if (!isValid(pod)) { return pod; }
            VUniquePOD(pod, _pods, _is);
            if (!isValid(pod)) { return pod; }
            VIsConfirmed(pod);
            return pod;
        }

        public PurchaseOrderDetail VDeleteObject(PurchaseOrderDetail pod)
        {
            VIsConfirmed(pod);
            return pod;
        }

        public PurchaseOrderDetail VConfirmObject(PurchaseOrderDetail pod)
        {
            VIsConfirmed(pod);
            return pod;
        }

        public PurchaseOrderDetail VUnconfirmObject(PurchaseOrderDetail pod, IPurchaseOrderDetailService _pods, IPurchaseReceivalDetailService _prds, IItemService _is)
        {
            VHasItemPendingReceival(pod, _is);
            if (!isValid(pod)) { return pod; }
            VConfirmedPurchaseReceival(pod, _prds);
            return pod;
        }

        public bool ValidCreateObject(PurchaseOrderDetail pod,  IPurchaseOrderDetailService _pods, IPurchaseOrderService _pos, IItemService _is)
        {
            VCreateObject(pod, _pods, _pos, _is);
            return isValid(pod);
        }

        public bool ValidUpdateObject(PurchaseOrderDetail pod,  IPurchaseOrderDetailService _pods, IPurchaseOrderService _pos, IItemService _is)
        {
            pod.Errors.Clear();
            VUpdateObject(pod, _pods, _pos, _is);
            return isValid(pod);
        }

        public bool ValidDeleteObject(PurchaseOrderDetail pod)
        {
            pod.Errors.Clear();
            VDeleteObject(pod);
            return isValid(pod);
        }

        public bool ValidConfirmObject(PurchaseOrderDetail pod)
        {
            pod.Errors.Clear();
            VConfirmObject(pod);
            return isValid(pod);
        }

        public bool ValidUnconfirmObject(PurchaseOrderDetail pod, IPurchaseOrderDetailService _pods, IPurchaseReceivalDetailService _prds, IItemService _is)
        {
            pod.Errors.Clear();
            VUnconfirmObject(pod, _pods, _prds, _is);
            return isValid(pod);
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