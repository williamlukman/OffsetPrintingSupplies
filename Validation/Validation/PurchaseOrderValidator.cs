using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Core.Interface.Validation;
using Core.DomainModel;
using Core.Interface.Service;
using Core.Constants;

namespace Validation.Validation
{
    public class PurchaseOrderValidator : IPurchaseOrderValidator
    {
        public PurchaseOrder VCustomer(PurchaseOrder purchaseOrder, ICustomerService _customerService)
        {
            Customer c = _customerService.GetObjectById(purchaseOrder.CustomerId);
            if (c == null)
            {
                purchaseOrder.Errors.Add("Customer", "Tidak boleh tidak ada");
            }
            return purchaseOrder;
        }

        public PurchaseOrder VPurchaseDate(PurchaseOrder purchaseOrder)
        {
            /* purchaseDate is never null
            if (purchaseOrder.PurchaseDate == null)
            {
                purchaseOrder.Errors.Add("PurchaseDate", "Tidak boleh tidak ada");
            }
            */
            return purchaseOrder;
        }

        public PurchaseOrder VHasBeenConfirmed(PurchaseOrder purchaseOrder)
        {
            if (!purchaseOrder.IsConfirmed)
            {
                purchaseOrder.Errors.Add("PurchaseOrder", "Belum dikonfirmasi");
            }
            return purchaseOrder;
        }

        public PurchaseOrder VHasNotBeenConfirmed(PurchaseOrder purchaseOrder)
        {
            if (purchaseOrder.IsConfirmed)
            {
                purchaseOrder.Errors.Add("PurchaseOrder", "Tidak boleh sudah dikonfirmasi");
            }
            return purchaseOrder;
        }

        public PurchaseOrder VHasPurchaseOrderDetails(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService)
        {
            IList<PurchaseOrderDetail> details = _purchaseOrderDetailService.GetObjectsByPurchaseOrderId(purchaseOrder.Id);
            if (!details.Any())
            {
                purchaseOrder.Errors.Add("PurchaseOrder", "Tidak boleh memilik Purchase Order Details");
            }
            return purchaseOrder;
        }

        public PurchaseOrder VAllDetailsHaveBeenFinished(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService)
        {
            IList<PurchaseOrderDetail> details = _purchaseOrderDetailService.GetObjectsByPurchaseOrderId(purchaseOrder.Id);
            foreach(var detail in details)
            {
                if (!detail.IsFinished)
                {
                    purchaseOrder.Errors.Add("Generic", "Purchase Order Detail belum selesai");
                    return purchaseOrder;
                }
            }
            return purchaseOrder;
        }

        public PurchaseOrder VAllDetailsHaveNotBeenFinished(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService)
        {
            IList<PurchaseOrderDetail> details = _purchaseOrderDetailService.GetObjectsByPurchaseOrderId(purchaseOrder.Id);
            foreach (var detail in details)
            {
                if (detail.IsFinished)
                {
                    purchaseOrder.Errors.Add("Generic", "Purchase Order Detail sudah selesai");
                    return purchaseOrder;
                }
            }
            return purchaseOrder;
        }

        public PurchaseOrder VCreateObject(PurchaseOrder purchaseOrder, ICustomerService _customerService)
        {
            VCustomer(purchaseOrder, _customerService);
            if (!isValid(purchaseOrder)) { return purchaseOrder; }
            VPurchaseDate(purchaseOrder);
            return purchaseOrder;
        }

        public PurchaseOrder VUpdateObject(PurchaseOrder purchaseOrder, ICustomerService _customerService)
        {
            VCreateObject(purchaseOrder, _customerService);
            if (!isValid(purchaseOrder)) { return purchaseOrder; }
            VHasNotBeenConfirmed(purchaseOrder);
            return purchaseOrder;
        }

        public PurchaseOrder VDeleteObject(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService)
        {
            VHasNotBeenConfirmed(purchaseOrder);
            return purchaseOrder;
        }

        public PurchaseOrder VConfirmObject(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService)
        {
            VHasNotBeenConfirmed(purchaseOrder);
            if (!isValid(purchaseOrder)) { return purchaseOrder; }
            VHasPurchaseOrderDetails(purchaseOrder, _purchaseOrderDetailService);            
            return purchaseOrder;
        }

        public PurchaseOrder VUnconfirmObject(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService, IPurchaseReceivalDetailService _purchaseReceivalDetailService, IItemService _itemService)
        {
            VHasBeenConfirmed(purchaseOrder);
            if (!isValid(purchaseOrder)) { return purchaseOrder; }
            VAllDetailsHaveNotBeenFinished(purchaseOrder, _purchaseOrderDetailService);
            return purchaseOrder;
        }

        public PurchaseOrder VCompleteObject(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService)
        {
            VAllDetailsHaveBeenFinished(purchaseOrder, _purchaseOrderDetailService);
            return purchaseOrder;
        }

        public bool ValidCreateObject(PurchaseOrder purchaseOrder, ICustomerService _customerService)
        {
            VCreateObject(purchaseOrder, _customerService);
            return isValid(purchaseOrder);
        }

        public bool ValidUpdateObject(PurchaseOrder purchaseOrder, ICustomerService _customerService)
        {
            purchaseOrder.Errors.Clear();
            VUpdateObject(purchaseOrder, _customerService);
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

        public bool ValidUnconfirmObject(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService, IPurchaseReceivalDetailService _purchaseReceivalDetailService, IItemService _itemService)
        {
            purchaseOrder.Errors.Clear();
            VUnconfirmObject(purchaseOrder, _purchaseOrderDetailService, _purchaseReceivalDetailService, _itemService);
            return isValid(purchaseOrder);
        }

        public bool ValidCompleteObject(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService)
        {
            purchaseOrder.Errors.Clear();
            VCompleteObject(purchaseOrder, _purchaseOrderDetailService);
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