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
        public PurchaseReceival VHasWarehouse(PurchaseReceival purchaseReceival, IWarehouseService _warehouseService)
        {
            Warehouse warehouse = _warehouseService.GetObjectById(purchaseReceival.WarehouseId);
            if (warehouse == null)
            {
                purchaseReceival.Errors.Add("WarehouseId", "Tidak terasosiasi dengan warehouse");
            }
            return purchaseReceival;
        }

        public PurchaseReceival VHasPurchaseOrder(PurchaseReceival purchaseReceival, IPurchaseOrderService _purchaseOrderService)
        {
            PurchaseOrder purchaseOrder = _purchaseOrderService.GetObjectById(purchaseReceival.PurchaseOrderId);
            if (purchaseOrder == null)
            {
                purchaseReceival.Errors.Add("PurchaseOrderId", "Tidak terasosiasi dengan purchase order");
            }
            return purchaseReceival;
        }

        public PurchaseReceival VHasReceivalDate(PurchaseReceival purchaseReceival)
        {
            if (purchaseReceival.ReceivalDate == null)
            {
                purchaseReceival.Errors.Add("ReceivalDate", "Tidak boleh tidak ada");
            }
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

        public PurchaseReceival VPurchaseOrderHasBeenConfirmed(PurchaseReceival purchaseReceival, IPurchaseOrderService _purchaseOrderService)
        {
            PurchaseOrder purchaseOrder = _purchaseOrderService.GetObjectById(purchaseReceival.PurchaseOrderId);
            if (!purchaseOrder.IsConfirmed)
            {
                purchaseReceival.Errors.Add("Generic", "Purchase order belum di konfirmasi");
            }
            return purchaseReceival;
        }

        public PurchaseReceival VHasPurchaseReceivalDetails(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService)
        {
            IList<PurchaseReceivalDetail> details = _purchaseReceivalDetailService.GetObjectsByPurchaseReceivalId(purchaseReceival.Id);
            if (!details.Any())
            {
                purchaseReceival.Errors.Add("Generic", "Tidak memiliki purchase receival detail");
            }
            return purchaseReceival;
        }

        public PurchaseReceival VHasNoPurchaseReceivalDetail(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService)
        {
            IList<PurchaseReceivalDetail> details = _purchaseReceivalDetailService.GetObjectsByPurchaseReceivalId(purchaseReceival.Id);
            if (details.Any())
            {
                purchaseReceival.Errors.Add("Generic", "Masih memiliki purchase receival detail");
            }
            return purchaseReceival;
        }

        public PurchaseReceival VHasConfirmationDate(PurchaseReceival obj)
        {
            if (obj.ConfirmationDate == null)
            {
                obj.Errors.Add("ConfirmationDate", "Tidak boleh kosong");
            }
            return obj;
        }

        public PurchaseReceival VHasNoPurchaseInvoice(PurchaseReceival purchaseReceival, IPurchaseInvoiceService _purchaseInvoiceService)
        {
            IList<PurchaseInvoice> purchaseInvoices = _purchaseInvoiceService.GetObjectsByPurchaseReceivalId(purchaseReceival.Id);
            if (purchaseInvoices.Any())
            {
                purchaseReceival.Errors.Add("Generic", "Sudah memiliki purhase invoice");
            }
            return purchaseReceival;
        }

        public PurchaseReceival VCreateObject(PurchaseReceival purchaseReceival, IPurchaseOrderService _purchaseOrderService, IWarehouseService _warehouseService)
        {
            VHasWarehouse(purchaseReceival, _warehouseService);
            if (!isValid(purchaseReceival)) { return purchaseReceival; }
            VHasPurchaseOrder(purchaseReceival, _purchaseOrderService);
            if (!isValid(purchaseReceival)) { return purchaseReceival; }
            VHasReceivalDate(purchaseReceival);
            if (!isValid(purchaseReceival)) { return purchaseReceival; }
            VPurchaseOrderHasBeenConfirmed(purchaseReceival, _purchaseOrderService);
            return purchaseReceival;
        }

        public PurchaseReceival VUpdateObject(PurchaseReceival purchaseReceival, IPurchaseOrderService _purchaseOrderService, IWarehouseService _warehouseService)
        {
            VCreateObject(purchaseReceival, _purchaseOrderService, _warehouseService);
            if (!isValid(purchaseReceival)) { return purchaseReceival; }
            VHasNotBeenConfirmed(purchaseReceival);
            return purchaseReceival;
        }

        public PurchaseReceival VDeleteObject(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService)
        {
            VHasNotBeenConfirmed(purchaseReceival);
            if (!isValid(purchaseReceival)) { return purchaseReceival; }
            VHasNoPurchaseReceivalDetail(purchaseReceival, _purchaseReceivalDetailService);
            return purchaseReceival;
        }

        public PurchaseReceival VConfirmObject(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService)
        {
            VHasConfirmationDate(purchaseReceival);
            if (!isValid(purchaseReceival)) { return purchaseReceival; }
            VHasNotBeenConfirmed(purchaseReceival);
            if (!isValid(purchaseReceival)) { return purchaseReceival; }
            VHasPurchaseReceivalDetails(purchaseReceival, _purchaseReceivalDetailService);
            return purchaseReceival;
        }

        public PurchaseReceival VUnconfirmObject(PurchaseReceival purchaseReceival, IPurchaseInvoiceService _purchaseInvoiceService)
        {
            VHasBeenConfirmed(purchaseReceival);
            if (!isValid(purchaseReceival)) { return purchaseReceival; }
            VHasNoPurchaseInvoice(purchaseReceival, _purchaseInvoiceService);
            return purchaseReceival;
        }

        public bool ValidCreateObject(PurchaseReceival purchaseReceival, IPurchaseOrderService _purchaseOrderService, IWarehouseService _warehouseService)
        {
            VCreateObject(purchaseReceival, _purchaseOrderService, _warehouseService);
            return isValid(purchaseReceival);
        }

        public bool ValidUpdateObject(PurchaseReceival purchaseReceival, IPurchaseOrderService _purchaseOrderService, IWarehouseService _warehouseService)
        {
            purchaseReceival.Errors.Clear();
            VUpdateObject(purchaseReceival, _purchaseOrderService, _warehouseService);
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

        public bool ValidUnconfirmObject(PurchaseReceival purchaseReceival, IPurchaseInvoiceService _purchaseInvoiceService)
        {
            purchaseReceival.Errors.Clear();
            VUnconfirmObject(purchaseReceival, _purchaseInvoiceService);
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