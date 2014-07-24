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
    public class PurchaseOrderValidator : IPurchaseOrderValidator
    {
        public PurchaseOrder VContact(PurchaseOrder po, IContactService _cs)
        {
            Contact c = _cs.GetObjectById(po.ContactId);
            if (c == null)
            {
                po.Errors.Add("Contact", "Tidak boleh tidak ada");
            }
            return po;
        }

        public PurchaseOrder VPurchaseDate(PurchaseOrder po)
        {
            /* purchaseDate is never null
            if (po.PurchaseDate == null)
            {
                po.Errors.Add("PurchaseDate", "Tidak boleh tidak ada");
            }
            */
            return po;
        }

        public PurchaseOrder VIsConfirmed(PurchaseOrder po)
        {
            if (po.IsConfirmed)
            {
                po.Errors.Add("PurchaseOrder", "Tidak boleh sudah dikonfirmasi");
            }
            return po;
        }

        public PurchaseOrder VHasPurchaseOrderDetails(PurchaseOrder po, IPurchaseOrderDetailService _pods)
        {
            IList<PurchaseOrderDetail> details = _pods.GetObjectsByPurchaseOrderId(po.Id);
            if (!details.Any())
            {
                po.Errors.Add("PurchaseOrder", "Tidak boleh memilik Purchase Order Details");
            }
            return po;
        }

        public PurchaseOrder VCreateObject(PurchaseOrder po, IContactService _cs)
        {
            VContact(po, _cs);
            if (!isValid(po)) { return po; }
            VPurchaseDate(po);
            return po;
        }

        public PurchaseOrder VUpdateObject(PurchaseOrder po, IContactService _cs)
        {
            VContact(po, _cs);
            if (!isValid(po)) { return po; }
            VPurchaseDate(po);
            if (!isValid(po)) { return po; }
            VIsConfirmed(po);
            return po;
        }

        public PurchaseOrder VDeleteObject(PurchaseOrder po, IPurchaseOrderDetailService _pods)
        {
            VIsConfirmed(po);
            return po;
        }

        public PurchaseOrder VConfirmObject(PurchaseOrder po, IPurchaseOrderDetailService _pods)
        {
            VIsConfirmed(po);
            if (!isValid(po)) { return po; }
            VHasPurchaseOrderDetails(po, _pods);
            if (isValid(po))
            {
                IList<PurchaseOrderDetail> details = _pods.GetObjectsByPurchaseOrderId(po.Id);
                IPurchaseOrderDetailValidator detailvalidator = new PurchaseOrderDetailValidator();
                foreach (var detail in details)
                {
                    detailvalidator.VConfirmObject(detail);
                    foreach (var error in detail.Errors)
                    {
                        po.Errors.Add(error.Key, error.Value);
                    }
                    if (!isValid(po)) { return po; }
                }
            }
            return po;
        }

        public PurchaseOrder VUnconfirmObject(PurchaseOrder po, IPurchaseOrderDetailService _pods, IPurchaseReceivalDetailService _prds, IItemService _is)
        {
            if (isValid(po))
            {
                IList<PurchaseOrderDetail> details = _pods.GetObjectsByPurchaseOrderId(po.Id);
                foreach (var detail in details)
                {
                    if (!_pods.GetValidator().ValidUnconfirmObject(detail, _pods, _prds, _is))
                    {
                        foreach (var error in detail.Errors)
                        {
                            po.Errors.Add(error.Key, error.Value);
                        }
                        if (!isValid(po)) { return po; }
                    }
                }
            }

            return po;
        }

        public bool ValidCreateObject(PurchaseOrder po, IContactService _cs)
        {
            VCreateObject(po, _cs);
            return isValid(po);
        }

        public bool ValidUpdateObject(PurchaseOrder po, IContactService _cs)
        {
            po.Errors.Clear();
            VUpdateObject(po, _cs);
            return isValid(po);
        }

        public bool ValidDeleteObject(PurchaseOrder po, IPurchaseOrderDetailService _pods)
        {
            po.Errors.Clear();
            VDeleteObject(po, _pods);
            return isValid(po);
        }

        public bool ValidConfirmObject(PurchaseOrder po, IPurchaseOrderDetailService _pods)
        {
            po.Errors.Clear();
            VConfirmObject(po, _pods);
            return isValid(po);
        }

        public bool ValidUnconfirmObject(PurchaseOrder po, IPurchaseOrderDetailService _pods, IPurchaseReceivalDetailService _prds, IItemService _is)
        {
            po.Errors.Clear();
            VUnconfirmObject(po, _pods, _prds, _is);
            return isValid(po);
        }

        public bool isValid(PurchaseOrder obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(PurchaseOrder obj)
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