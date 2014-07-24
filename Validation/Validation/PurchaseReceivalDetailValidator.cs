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
    public class PurchaseReceivalDetailValidator : IPurchaseReceivalDetailValidator
    {
        public PurchaseReceivalDetail VHasPurchaseReceival(PurchaseReceivalDetail prd, IPurchaseReceivalService _prs)
        {
            PurchaseReceival pr = _prs.GetObjectById(prd.PurchaseReceivalId);
            if (pr == null)
            {
                prd.Errors.Add("PurchaseReceival", "Tidak boleh tidak ada");
            }
            return prd;
        }

        public PurchaseReceivalDetail VHasItem(PurchaseReceivalDetail prd, IItemService _is)
        {
            Item item = _is.GetObjectById(prd.ItemId);
            if (item == null)
            {
                prd.Errors.Add("Item", "Tidak boleh tidak ada");
            }
            return prd;
        }

        public PurchaseReceivalDetail VContact(PurchaseReceivalDetail prd, IPurchaseReceivalService _prs, IPurchaseOrderService _pos, IPurchaseOrderDetailService _pods, IContactService _cs)
        {
            PurchaseReceival pr = _prs.GetObjectById(prd.PurchaseReceivalId);
            PurchaseOrderDetail pod = _pods.GetObjectById(prd.PurchaseOrderDetailId);
            if (pod == null)
            {
                prd.Errors.Add("PurchaseOrderDetail", "Tidak boleh tidak ada");
                return prd;
            }
            PurchaseOrder po = _pos.GetObjectById(pod.PurchaseOrderId);
            if (po.ContactId != pr.ContactId)
            {
                prd.Errors.Add("Contact", "Tidak boleh merupakan kustomer yang berbeda dengan Purchase Order");
            }
            return prd;
        }

        public PurchaseReceivalDetail VQuantityCreate(PurchaseReceivalDetail prd, IPurchaseOrderDetailService _pods)
        {
            PurchaseOrderDetail pod = _pods.GetObjectById(prd.PurchaseOrderDetailId);
            if (pod == null)
            {
                prd.Errors.Add("PurchaseOrderDetail", "Tidak boleh tidak ada");
                return prd;
            }
            if (prd.Quantity <= 0)
            {
                prd.Errors.Add("Quantity", "Tidak boleh kurang dari atau sama dengan 0");
                return prd;
            }
            if (prd.Quantity > pod.Quantity)
            {
                prd.Errors.Add("Quantity", "Tidak boleh lebih dari quantity dari Purchase Order Detail");
                return prd;
            }
            return prd;
        }

        public PurchaseReceivalDetail VQuantityUpdate(PurchaseReceivalDetail prd, IPurchaseOrderDetailService _pods)
        {
            PurchaseOrderDetail pod = _pods.GetObjectById(prd.PurchaseOrderDetailId);
            if (prd.Quantity <= 0)
            {
                prd.Errors.Add("Quantity", "Tidak boleh kurang dari atau sama dengan 0");
            }
            if (prd.Quantity > pod.Quantity)
            {
                prd.Errors.Add("Quantity", "Tidak boleh lebih besar dari quantity dari Purchase Order Detail");
            }
            return prd;
        }

        public PurchaseReceivalDetail VHasPurchaseOrderDetail(PurchaseReceivalDetail prd, IPurchaseOrderDetailService _pods)
        {
            PurchaseOrderDetail pod = _pods.GetObjectById(prd.PurchaseOrderDetailId);
            if (pod == null)
            {
                prd.Errors.Add("PurchaseOrderDetail", "Tidak boleh tidak ada");
            }
            return prd;
        }
        
        public PurchaseReceivalDetail VUniquePOD(PurchaseReceivalDetail prd, IPurchaseReceivalDetailService _prds, IItemService _is)
        {
            IList<PurchaseReceivalDetail> details = _prds.GetObjectsByPurchaseReceivalId(prd.PurchaseReceivalId);
            foreach (var detail in details)
            {
                if (detail.PurchaseOrderDetailId == prd.PurchaseOrderDetailId && detail.Id != prd.Id)
                {
                    prd.Errors.Add("PurchaseOrderDetail", "Tidak boleh memiliki lebih dari 2 Purchase Receival Detail");
                }
            }
            return prd;
        }

        public PurchaseReceivalDetail VIsConfirmed(PurchaseReceivalDetail prd)
        {
            if (prd.IsConfirmed)
            {
                prd.Errors.Add("IsConfirmed", "Tidak boleh sudah dikonfirmasi");
            }
            return prd;
        }

        public PurchaseReceivalDetail VHasItemQuantity(PurchaseReceivalDetail prd, IItemService _is)
        {
            Item item = _is.GetObjectById(prd.ItemId);
            if (item.PendingReceival < 0)
            {
                prd.Errors.Add("Item.PendingReceival", "Tidak boleh kurang dari 0");
            }
            if (item.Ready < prd.Quantity)
            {
                prd.Errors.Add("Item.Ready", "Tidak boleh kurang dari quantity dari Purchase Receival Detail");
            }
            return prd;
        }

        public PurchaseReceivalDetail VCreateObject(PurchaseReceivalDetail prd, IPurchaseReceivalDetailService _prds, IPurchaseReceivalService _prs,
                                                    IPurchaseOrderDetailService _pods, IPurchaseOrderService _pos, IItemService _is, IContactService _cs)
        {
            VHasPurchaseReceival(prd, _prs);
            if (!isValid(prd)) return prd;
            VHasItem(prd, _is);
            if (!isValid(prd)) return prd;
            VContact(prd, _prs, _pos, _pods, _cs);
            if (!isValid(prd)) return prd;
            VQuantityCreate(prd, _pods);
            if (!isValid(prd)) return prd;
            VUniquePOD(prd, _prds, _is);
            return prd;
        }

        public PurchaseReceivalDetail VUpdateObject(PurchaseReceivalDetail prd, IPurchaseReceivalDetailService _prds, IPurchaseReceivalService _prs,
                                                    IPurchaseOrderDetailService _pods, IPurchaseOrderService _pos, IItemService _is, IContactService _cs)
        {
            VHasPurchaseReceival(prd, _prs);
            if (!isValid(prd)) return prd;
            VHasItem(prd, _is);
            if (!isValid(prd)) return prd;
            VContact(prd, _prs, _pos, _pods, _cs);
            if (!isValid(prd)) return prd;
            VQuantityUpdate(prd, _pods);
            if (!isValid(prd)) return prd;
            VUniquePOD(prd, _prds, _is);
            if (!isValid(prd)) return prd;
            VIsConfirmed(prd);
            return prd;
        }

        public PurchaseReceivalDetail VDeleteObject(PurchaseReceivalDetail prd)
        {
            VIsConfirmed(prd);
            return prd;
        }

        public PurchaseReceivalDetail VConfirmObject(PurchaseReceivalDetail prd)
        {
            VIsConfirmed(prd);
            return prd;
        }

        public PurchaseReceivalDetail VUnconfirmObject(PurchaseReceivalDetail prd, IPurchaseReceivalDetailService _prds, IItemService _is)
        {
            VHasItemQuantity(prd, _is);
            return prd;
        }

        public bool ValidCreateObject(PurchaseReceivalDetail prd, IPurchaseReceivalDetailService _prds, IPurchaseReceivalService _prs,
                                                    IPurchaseOrderDetailService _pods, IPurchaseOrderService _pos, IItemService _is, IContactService _cs)
        {
            VCreateObject(prd, _prds, _prs, _pods, _pos, _is, _cs);
            return isValid(prd);
        }

        public bool ValidUpdateObject(PurchaseReceivalDetail prd, IPurchaseReceivalDetailService _prds, IPurchaseReceivalService _prs,
                                                    IPurchaseOrderDetailService _pods, IPurchaseOrderService _pos, IItemService _is, IContactService _cs)
        {
            prd.Errors.Clear();
            VUpdateObject(prd, _prds, _prs, _pods, _pos, _is, _cs);
            return isValid(prd);
        }

        public bool ValidDeleteObject(PurchaseReceivalDetail prd)
        {
            prd.Errors.Clear();
            VDeleteObject(prd);
            return isValid(prd);
        }

        public bool ValidConfirmObject(PurchaseReceivalDetail prd)
        {
            prd.Errors.Clear();
            VConfirmObject(prd);
            return isValid(prd);
        }

        public bool ValidUnconfirmObject(PurchaseReceivalDetail prd, IPurchaseReceivalDetailService _prds, IItemService _is)
        {
            prd.Errors.Clear();
            VUnconfirmObject(prd, _prds, _is);
            return isValid(prd);
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