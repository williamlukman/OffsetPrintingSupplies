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
    public class DeliveryOrderDetailValidator : IDeliveryOrderDetailValidator
    {
        public DeliveryOrderDetail VHasDeliveryOrder(DeliveryOrderDetail dod, IDeliveryOrderService _prs)
        {
            DeliveryOrder pr = _prs.GetObjectById(dod.DeliveryOrderId);
            if (pr == null)
            {
                dod.Errors.Add("DeliveryOrder", "Tidak boleh tidak ada");
            }
            return dod;
        }

        public DeliveryOrderDetail VHasItem(DeliveryOrderDetail dod, IItemService _is)
        {
            Item item = _is.GetObjectById(dod.ItemId);
            if (item == null)
            {
                dod.Errors.Add("Item", "Tidak boleh tidak ada");
            }
            return dod;
        }

        public DeliveryOrderDetail VContact(DeliveryOrderDetail dod, IDeliveryOrderService _prs, ISalesOrderService _sos, ISalesOrderDetailService _sods, IContactService _cs)
        {
            DeliveryOrder pr = _prs.GetObjectById(dod.DeliveryOrderId);
            SalesOrderDetail sod = _sods.GetObjectById(dod.SalesOrderDetailId);
            if (sod == null)
            {
                dod.Errors.Add("SalesOrderDetail", "Tidak boleh tidak ada");
                return dod;
            }
            SalesOrder so = _sos.GetObjectById(sod.SalesOrderId);
            if (so.ContactId != pr.ContactId)
            {
                dod.Errors.Add("Contact", "Tidak boleh merupakan kustomer yang berbeda dengan Sales Order");
            }
            return dod;
        }

        public DeliveryOrderDetail VQuantityCreate(DeliveryOrderDetail dod, ISalesOrderDetailService _sods)
        {
            SalesOrderDetail sod = _sods.GetObjectById(dod.SalesOrderDetailId);
            if (dod.Quantity <= 0)
            {
                dod.Errors.Add("Quantity", "Tidak boleh kurang dari atau sama dengan 0");
            }
            if (dod.Quantity > sod.Quantity)
            {
                dod.Errors.Add("Quantity", "Tidak boleh lebih dari Sales Order quantity");
            }
            return dod;
        }

        public DeliveryOrderDetail VQuantityUpdate(DeliveryOrderDetail dod, ISalesOrderDetailService _sods)
        {
            VQuantityCreate(dod, _sods);
            return dod;
        }

        public DeliveryOrderDetail VQuantityUnconfirm(DeliveryOrderDetail dod, IItemService _is)
        {
            Item item = _is.GetObjectById(dod.ItemId);
            if (item.PendingDelivery < 0)
            {
                dod.Errors.Add("Item.PendingDelivery", "Tidak boleh kurang dari 0");
            }
            if (item.Ready < 0)
            {
                dod.Errors.Add("Item.Ready", "Tidak boleh kurang dari 0");
            }
            return dod;
        }

        public DeliveryOrderDetail VHasSalesOrderDetail(DeliveryOrderDetail dod, ISalesOrderDetailService _sods)
        {
            SalesOrderDetail sod = _sods.GetObjectById(dod.SalesOrderDetailId);
            if (sod == null)
            {
                dod.Errors.Add("SalesOrderDetail", "Tidak boleh tidak ada");
            }
            return dod;
        }

        public DeliveryOrderDetail VHasItemQuantity(DeliveryOrderDetail dod, IItemService _is)
        {
            Item item = _is.GetObjectById(dod.ItemId);
            if (item.Ready - dod.Quantity < 0)
            {
                dod.Errors.Add("Item.Ready", "Tidak boleh kurang dari quantity Delivery Order");
            }
            return dod;
        }

        public DeliveryOrderDetail VUniqueSOD(DeliveryOrderDetail dod, IDeliveryOrderDetailService _dods, IItemService _is)
        {
            IList<DeliveryOrderDetail> details = _dods.GetObjectsByDeliveryOrderId(dod.DeliveryOrderId);
            foreach (var detail in details)
            {
                if (detail.SalesOrderDetailId == dod.SalesOrderDetailId && detail.Id != dod.Id)
                {
                    dod.Errors.Add("SalesOrderDetail", "Tidak boleh memiliki lebih dari 2 Delivery Order Detail");
                    return dod;
                }
            }
            return dod;
        }

        public DeliveryOrderDetail VIsConfirmed(DeliveryOrderDetail dod)
        {
            if (dod.IsConfirmed)
            {
                dod.Errors.Add("DeliveryOrderDetail", "Tidak boleh sudah dikonfirmasi.");
            }
            return dod;
        }

        public DeliveryOrderDetail VCreateObject(DeliveryOrderDetail dod, IDeliveryOrderDetailService _dods, IDeliveryOrderService _prs,
                                                    ISalesOrderDetailService _sods, ISalesOrderService _sos, IItemService _is, IContactService _cs)
        {
            VHasDeliveryOrder(dod, _prs);
            if (!isValid(dod)) { return dod; }
            VHasItem(dod, _is);
            if (!isValid(dod)) { return dod; }
            VContact(dod, _prs, _sos, _sods, _cs);
            if (!isValid(dod)) { return dod; }
            VQuantityCreate(dod, _sods);
            if (!isValid(dod)) { return dod; }
            VUniqueSOD(dod, _dods, _is);
            return dod;
        }

        public DeliveryOrderDetail VUpdateObject(DeliveryOrderDetail dod, IDeliveryOrderDetailService _dods, IDeliveryOrderService _prs,
                                                    ISalesOrderDetailService _sods, ISalesOrderService _sos, IItemService _is, IContactService _cs)
        {
            VHasDeliveryOrder(dod, _prs);
            if (!isValid(dod)) { return dod; }
            VHasItem(dod, _is);
            if (!isValid(dod)) { return dod; }
            VContact(dod, _prs, _sos, _sods, _cs);
            if (!isValid(dod)) { return dod; }
            VQuantityUpdate(dod, _sods);
            if (!isValid(dod)) { return dod; }
            VUniqueSOD(dod, _dods, _is);
            if (!isValid(dod)) { return dod; }
            VIsConfirmed(dod);
            return dod;
        }

        public DeliveryOrderDetail VDeleteObject(DeliveryOrderDetail dod)
        {
            VIsConfirmed(dod);
            return dod;
        }

        public DeliveryOrderDetail VConfirmObject(DeliveryOrderDetail dod, IItemService _is)
        {
            VIsConfirmed(dod);
            if (!isValid(dod)) { return dod; }
            VHasItemQuantity(dod, _is);
            return dod;
        }

        public DeliveryOrderDetail VUnconfirmObject(DeliveryOrderDetail dod, IDeliveryOrderDetailService _dods, IItemService _is)
        {
            VQuantityUnconfirm(dod, _is);
            return dod;
        }

        public bool ValidCreateObject(DeliveryOrderDetail dod, IDeliveryOrderDetailService _dods, IDeliveryOrderService _prs,
                                                    ISalesOrderDetailService _sods, ISalesOrderService _sos, IItemService _is, IContactService _cs)
        {
            VCreateObject(dod, _dods, _prs, _sods, _sos, _is, _cs);
            return isValid(dod);
        }

        public bool ValidUpdateObject(DeliveryOrderDetail dod, IDeliveryOrderDetailService _dods, IDeliveryOrderService _prs,
                                                    ISalesOrderDetailService _sods, ISalesOrderService _sos, IItemService _is, IContactService _cs)
        {
            dod.Errors.Clear();
            VUpdateObject(dod, _dods, _prs, _sods, _sos, _is, _cs);
            return isValid(dod);
        }

        public bool ValidDeleteObject(DeliveryOrderDetail dod)
        {
            dod.Errors.Clear();
            VDeleteObject(dod);
            return isValid(dod);
        }

        public bool ValidConfirmObject(DeliveryOrderDetail dod, IItemService _is)
        {
            dod.Errors.Clear();
            VConfirmObject(dod, _is);
            return isValid(dod);
        }

        public bool ValidUnconfirmObject(DeliveryOrderDetail dod, IDeliveryOrderDetailService _dods, IItemService _is)
        {
            dod.Errors.Clear();
            VUnconfirmObject(dod, _dods, _is);
            return isValid(dod);
        }

        public bool isValid(DeliveryOrderDetail obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(DeliveryOrderDetail obj)
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