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
    public class PurchaseReceivalValidator : IPurchaseReceivalValidator
    {

        public PurchaseReceival VContact(PurchaseReceival purchaseReceival, IContactService _contactService)
        {
            Contact contact = _contactService.GetObjectById(purchaseReceival.ContactId);
            if (contact == null)
            {
                purchaseReceival.Errors.Add("Contact", "Tidak boleh tidak ada");
            }
            return purchaseReceival;
        }

        public PurchaseReceival VReceivalDate(PurchaseReceival purchaseReceival)
        {
            /* receivalDate is never null
            if (purchaseReceival.ReceivalDate == null)
            {
                purchaseReceival.Errors.Add("ReceivalDate", "Tidak boleh tidak ada");
            }
            */
            return purchaseReceival;
        }

        public PurchaseReceival VHasNotBeenConfirmed(PurchaseReceival purchaseReceival)
        {
            if (purchaseReceival.IsConfirmed)
            {
                purchaseReceival.Errors.Add("Generic", "Tidak boleh sudah dikonfirmasi");
            }
            return purchaseReceival;
        }

        public PurchaseReceival VHasBeenConfirmed(PurchaseReceival purchaseReceival)
        {
            if (!purchaseReceival.IsConfirmed)
            {
                purchaseReceival.Errors.Add("Generic", "Belum dikonfirmasi");
            }
            return purchaseReceival;
        }

        public PurchaseReceival VHasPurchaseReceivalDetails(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService)
        {
            IList<PurchaseReceivalDetail> details = _purchaseReceivalDetailService.GetObjectsByPurchaseReceivalId(purchaseReceival.Id);
            if (!details.Any())
            {
                purchaseReceival.Errors.Add("PurchaseReceivalDetail", "Tidak boleh tidak ada");
            }
            return purchaseReceival;
        }

        public PurchaseReceival VAllDetailsHaveBeenFinished(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService)
        {
            IList<PurchaseReceivalDetail> details = _purchaseReceivalDetailService.GetObjectsByPurchaseReceivalId(purchaseReceival.Id);
            foreach (var detail in details)
            {
                if (!detail.IsFinished)
                {
                    purchaseReceival.Errors.Add("PurchaseReceivalDetail", "Belum selesai");
                    return purchaseReceival;
                }
            }
            return purchaseReceival;
        }

        public PurchaseReceival VAllDetailsHaveNotBeenFinished(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService)
        {
            IList<PurchaseReceivalDetail> details = _purchaseReceivalDetailService.GetObjectsByPurchaseReceivalId(purchaseReceival.Id);
            foreach (var detail in details)
            {
                if (detail.IsFinished)
                {
                    purchaseReceival.Errors.Add("PurchaseReceivalDetail", "Sudah selesai");
                    return purchaseReceival;
                }
            }
            return purchaseReceival;
        }

        public PurchaseReceival VCreateObject(PurchaseReceival purchaseReceival, IContactService _contactService)
        {
            VContact(purchaseReceival, _contactService);
            if (!isValid(purchaseReceival)) { return purchaseReceival; }
            VReceivalDate(purchaseReceival);
            return purchaseReceival;
        }

        public PurchaseReceival VUpdateObject(PurchaseReceival purchaseReceival, IContactService _contactService)
        {
            VCreateObject(purchaseReceival, _contactService);
            if (!isValid(purchaseReceival)) { return purchaseReceival; }
            VHasNotBeenConfirmed(purchaseReceival);
            return purchaseReceival;
        }

        public PurchaseReceival VDeleteObject(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService)
        {
            VHasNotBeenConfirmed(purchaseReceival);
            return purchaseReceival;
        }

        public PurchaseReceival VConfirmObject(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService)
        {
            VHasNotBeenConfirmed(purchaseReceival);
            if (!isValid(purchaseReceival)) { return purchaseReceival; }
            VHasPurchaseReceivalDetails(purchaseReceival, _purchaseReceivalDetailService);
            return purchaseReceival;
        }

        public PurchaseReceival VUnconfirmObject(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService, IItemService _itemService)
        {
            VHasBeenConfirmed(purchaseReceival);
            if (!isValid(purchaseReceival)) { return purchaseReceival; }
            VAllDetailsHaveNotBeenFinished(purchaseReceival, _purchaseReceivalDetailService);
            return purchaseReceival;
        }

        public PurchaseReceival VCompleteObject(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService)
        {
            VAllDetailsHaveBeenFinished(purchaseReceival, _purchaseReceivalDetailService);
            return purchaseReceival;
        }

        public bool ValidCreateObject(PurchaseReceival purchaseReceival, IContactService _contactService)
        {
            VCreateObject(purchaseReceival, _contactService);
            return isValid(purchaseReceival);
        }

        public bool ValidUpdateObject(PurchaseReceival purchaseReceival, IContactService _contactService)
        {
            purchaseReceival.Errors.Clear();
            VUpdateObject(purchaseReceival, _contactService);
            return isValid(purchaseReceival);
        }

        public bool ValidDeleteObject(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService)
        {
            purchaseReceival.Errors.Clear();
            VDeleteObject(purchaseReceival, _purchaseReceivalDetailService);
            return isValid(purchaseReceival);
        }

        public bool ValidConfirmObject(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService)
        {
            purchaseReceival.Errors.Clear();
            VConfirmObject(purchaseReceival, _purchaseReceivalDetailService);
            return isValid(purchaseReceival);
        }

        public bool ValidUnconfirmObject(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService, IItemService _itemService)
        {
            purchaseReceival.Errors.Clear();
            VUnconfirmObject(purchaseReceival, _purchaseReceivalDetailService, _itemService);
            return isValid(purchaseReceival);
        }

        public bool ValidCompleteObject(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService)
        {
            purchaseReceival.Errors.Clear();
            VCompleteObject(purchaseReceival, _purchaseReceivalDetailService);
            return isValid(purchaseReceival);
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