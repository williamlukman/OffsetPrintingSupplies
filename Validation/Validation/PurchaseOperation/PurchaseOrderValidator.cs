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
    public class PurchaseOrderValidator : IPurchaseOrderValidator
    {
        public PurchaseOrder VHasUniqueNomorSurat(PurchaseOrder purchaseOrder, IPurchaseOrderService _purchaseOrderService)
        {
            IList<PurchaseOrder> duplicates = _purchaseOrderService.GetQueryable().Where(x => x.NomorSurat == purchaseOrder.NomorSurat && x.Id != purchaseOrder.Id && !x.IsDeleted).ToList();
            if (duplicates.Any())
            {
                purchaseOrder.Errors.Add("NomorSurat", "Tidak boleh merupakan duplikasi");
            }
            return purchaseOrder;
        }

        public PurchaseOrder VHasContact(PurchaseOrder purchaseOrder, IContactService _contactService)
        {
            Contact contact = _contactService.GetObjectById(purchaseOrder.ContactId);
            if (contact == null)
            {
                purchaseOrder.Errors.Add("ContactId", "Tidak terasosiasi dengan contact");
            }
            return purchaseOrder;
        }

        public PurchaseOrder VHasPurchaseDate(PurchaseOrder purchaseOrder)
        {
            if (purchaseOrder.PurchaseDate == null)
            {
                purchaseOrder.Errors.Add("PurchaseDate", "Tidak boleh tidak ada");
            }
            return purchaseOrder;
        }

        public PurchaseOrder VHasBeenConfirmed(PurchaseOrder purchaseOrder)
        {
            if (!purchaseOrder.IsConfirmed)
            {
                purchaseOrder.Errors.Add("Generic", "Belum dikonfirmasi");
            }
            return purchaseOrder;
        }

        public PurchaseOrder VHasNotBeenConfirmed(PurchaseOrder purchaseOrder)
        {
            if (purchaseOrder.IsConfirmed)
            {
                purchaseOrder.Errors.Add("Generic", "Tidak boleh sudah dikonfirmasi");
            }
            return purchaseOrder;
        }

        public PurchaseOrder VHasPurchaseOrderDetails(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService)
        {
            IList<PurchaseOrderDetail> details = _purchaseOrderDetailService.GetObjectsByPurchaseOrderId(purchaseOrder.Id);
            if (!details.Any())
            {
                purchaseOrder.Errors.Add("Generic", "Tidak memiliki Purchase Order Details");
            }
            return purchaseOrder;
        }

        public PurchaseOrder VHasNoPurchaseOrderDetail(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService)
        {
            IList<PurchaseOrderDetail> details = _purchaseOrderDetailService.GetObjectsByPurchaseOrderId(purchaseOrder.Id);
            if (details.Any())
            {
                purchaseOrder.Errors.Add("Generic", "Memiliki Purchase Order Details");
            }
            return purchaseOrder;
        }

        public PurchaseOrder VHasNoPurchaseReceival(PurchaseOrder purchaseOrder, IPurchaseReceivalService _purchaseReceivalService)
        {
            IList<PurchaseReceival> purchaseReceivals = _purchaseReceivalService.GetObjectsByPurchaseOrderId(purchaseOrder.Id);
            if (purchaseReceivals.Any())
            {
                purchaseOrder.Errors.Add("Generic", "Memiliki asosiasi dengan purchase receival");
            }
            return purchaseOrder;
        }

        public PurchaseOrder VHasConfirmationDate(PurchaseOrder obj)
        {
            if (obj.ConfirmationDate == null)
            {
                obj.Errors.Add("ConfirmationDate", "Tidak boleh kosong");
            }
            return obj;
        }
        
        public PurchaseOrder VCreateObject(PurchaseOrder purchaseOrder, IPurchaseOrderService _purchaseOrderService, IContactService _contactService)
        {
            VHasUniqueNomorSurat(purchaseOrder, _purchaseOrderService);
            if (!isValid(purchaseOrder)) { return purchaseOrder; }
            VHasContact(purchaseOrder, _contactService);
            if (!isValid(purchaseOrder)) { return purchaseOrder; }
            VHasPurchaseDate(purchaseOrder);
            return purchaseOrder;
        }

        public PurchaseOrder VUpdateObject(PurchaseOrder purchaseOrder, IPurchaseOrderService _purchaseOrderService, IContactService _contactService)
        {
            VCreateObject(purchaseOrder, _purchaseOrderService, _contactService);
            //if (!isValid(purchaseOrder)) { return purchaseOrder; }
            //// Allow Changing AllowEditDetailQtyAfterConfirm anytime
            //VHasNotBeenConfirmed(purchaseOrder);
            return purchaseOrder;
        }

        public PurchaseOrder VDeleteObject(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService)
        {
            VHasNotBeenConfirmed(purchaseOrder);
            if (!isValid(purchaseOrder)) { return purchaseOrder; }
            VHasNoPurchaseOrderDetail(purchaseOrder, _purchaseOrderDetailService);
            return purchaseOrder;
        }

        public PurchaseOrder VConfirmObject(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService)
        {
            VHasConfirmationDate(purchaseOrder);
            if (!isValid(purchaseOrder)) { return purchaseOrder; }
            VHasNotBeenConfirmed(purchaseOrder);
            if (!isValid(purchaseOrder)) { return purchaseOrder; }
            VHasPurchaseOrderDetails(purchaseOrder, _purchaseOrderDetailService);            
            return purchaseOrder;
        }

        public PurchaseOrder VUnconfirmObject(PurchaseOrder purchaseOrder, IPurchaseReceivalService _purchaseReceivalService)
        {
            VHasBeenConfirmed(purchaseOrder);
            if (!isValid(purchaseOrder)) { return purchaseOrder; }
            VHasNoPurchaseReceival(purchaseOrder, _purchaseReceivalService);
            return purchaseOrder;
        }

        public bool ValidCreateObject(PurchaseOrder purchaseOrder, IPurchaseOrderService _purchaseOrderService, IContactService _contactService)
        {
            VCreateObject(purchaseOrder, _purchaseOrderService, _contactService);
            return isValid(purchaseOrder);
        }

        public bool ValidUpdateObject(PurchaseOrder purchaseOrder, IPurchaseOrderService _purchaseOrderService, IContactService _contactService)
        {
            purchaseOrder.Errors.Clear();
            VUpdateObject(purchaseOrder, _purchaseOrderService, _contactService);
            return isValid(purchaseOrder);
        }

        public bool ValidDeleteObject(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService)
        {
            purchaseOrder.Errors.Clear();
            VDeleteObject(purchaseOrder, _purchaseOrderDetailService);
            return isValid(purchaseOrder);
        }

        public bool ValidConfirmObject(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService)
        {
            purchaseOrder.Errors.Clear();
            VConfirmObject(purchaseOrder, _purchaseOrderDetailService);
            return isValid(purchaseOrder);
        }

        public bool ValidUnconfirmObject(PurchaseOrder purchaseOrder, IPurchaseReceivalService _purchaseReceivalService)
        {
            purchaseOrder.Errors.Clear();
            VUnconfirmObject(purchaseOrder, _purchaseReceivalService);
            return isValid(purchaseOrder);
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