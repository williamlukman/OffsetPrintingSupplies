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
    public class PurchaseReceivalValidator : IPurchaseReceivalValidator
    {

        public PurchaseReceival VContact(PurchaseReceival pr, IContactService _cs)
        {
            Contact c = _cs.GetObjectById(pr.ContactId);
            if (c == null)
            {
                pr.Errors.Add("Contact", "Tidak boleh tidak ada");
            }
            return pr;
        }

        public PurchaseReceival VReceivalDate(PurchaseReceival pr)
        {
            /* receivalDate is never null
            if (pr.ReceivalDate == null)
            {
                pr.Errors.Add("ReceivalDate", "Tidak boleh tidak ada");
            }
            */
            return pr;
        }

        public PurchaseReceival VIsConfirmed(PurchaseReceival pr)
        {
            if (pr.IsConfirmed)
            {
                pr.Errors.Add("IsConfirmed", "Tidak boleh sudah dikonfirmasi");
            }
            return pr;
        }

        public PurchaseReceival VHasPurchaseReceivalDetails(PurchaseReceival pr, IPurchaseReceivalDetailService _prds)
        {
            IList<PurchaseReceivalDetail> details = _prds.GetObjectsByPurchaseReceivalId(pr.Id);
            if (!details.Any())
            {
                pr.Errors.Add("PurchaseReceivalDetail", "Tidak boleh tidak ada");
            }
            return pr;
        }

        public PurchaseReceival VCreateObject(PurchaseReceival pr, IContactService _cs)
        {
            VContact(pr, _cs);
            if (!isValid(pr)) { return pr; }
            VReceivalDate(pr);
            return pr;
        }

        public PurchaseReceival VUpdateObject(PurchaseReceival pr, IContactService _cs)
        {
            VContact(pr, _cs);
            if (!isValid(pr)) { return pr; }
            VReceivalDate(pr);
            if (!isValid(pr)) { return pr; }
            VIsConfirmed(pr);
            return pr;
        }

        public PurchaseReceival VDeleteObject(PurchaseReceival pr, IPurchaseReceivalDetailService _prds)
        {
            VIsConfirmed(pr);
            return pr;
        }

        public PurchaseReceival VConfirmObject(PurchaseReceival pr, IPurchaseReceivalDetailService _prds)
        {
            VIsConfirmed(pr);
            if (!isValid(pr)) { return pr; }
            VHasPurchaseReceivalDetails(pr, _prds);
            if (isValid(pr))
            {
                IList<PurchaseReceivalDetail> details = _prds.GetObjectsByPurchaseReceivalId(pr.Id);
                foreach (var detail in details)
                {
                    if (!_prds.GetValidator().ValidConfirmObject(detail))
                    {
                        foreach (var error in detail.Errors)
                        {
                            pr.Errors.Add(error.Key, error.Value);
                        }
                        if (!isValid(pr)) { return pr; }
                    }
                }
            }
            return pr;
        }

        public PurchaseReceival VUnconfirmObject(PurchaseReceival pr, IPurchaseReceivalDetailService _prds, IItemService _is)
        {            
            if (isValid(pr))
            {
                IList<PurchaseReceivalDetail> details = _prds.GetObjectsByPurchaseReceivalId(pr.Id);
                foreach (var detail in details)
                {
                    if (!_prds.GetValidator().ValidUnconfirmObject(detail, _prds, _is))
                    {
                        foreach (var error in detail.Errors)
                        {
                            pr.Errors.Add(error.Key, error.Value);
                        }
                        if (!isValid(pr)) { return pr; }
                    }
                }
            }

            return pr;
        }

        public bool ValidCreateObject(PurchaseReceival pr, IContactService _cs)
        {
            VCreateObject(pr, _cs);
            return isValid(pr);
        }

        public bool ValidUpdateObject(PurchaseReceival pr, IContactService _cs)
        {
            pr.Errors.Clear();
            VUpdateObject(pr, _cs);
            return isValid(pr);
        }

        public bool ValidDeleteObject(PurchaseReceival pr, IPurchaseReceivalDetailService _prds)
        {
            pr.Errors.Clear();
            VDeleteObject(pr, _prds);
            return isValid(pr);
        }

        public bool ValidConfirmObject(PurchaseReceival pr, IPurchaseReceivalDetailService _prds)
        {
            pr.Errors.Clear();
            VConfirmObject(pr, _prds);
            return isValid(pr);
        }

        public bool ValidUnconfirmObject(PurchaseReceival pr, IPurchaseReceivalDetailService _prds, IItemService _is)
        {
            pr.Errors.Clear();
            VUnconfirmObject(pr, _prds, _is);
            return isValid(pr);
        }

        public bool isValid(PurchaseReceival obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(PurchaseReceival obj)
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