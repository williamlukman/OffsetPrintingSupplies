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
    public class SalesOrderDetailValidator : ISalesOrderDetailValidator
    {
        public SalesOrderDetail VHasSalesOrder(SalesOrderDetail sod, ISalesOrderService _sos)
        {
            SalesOrder so = _sos.GetObjectById(sod.SalesOrderId);
            if (so == null)
            {
                sod.Errors.Add("SalesOrder", "Tidak boleh tidak ada");
            }
            return sod;
        }

        public SalesOrderDetail VHasItem(SalesOrderDetail sod, IItemService _is)
        {
            Item item = _is.GetObjectById(sod.ItemId);
            if (item == null)
            {
                sod.Errors.Add("Item", "Tidak boleh tidak ada");
            }
            return sod;
        }

        public SalesOrderDetail VQuantity(SalesOrderDetail sod)
        {
            if (sod.Quantity < 0)
            {
                sod.Errors.Add("Quantity", "Tidak boleh kurang dari 0");
            }
            return sod;
        }

        public SalesOrderDetail VPrice(SalesOrderDetail sod)
        {
            if (sod.Price <= 0)
            {
                sod.Errors.Add("Price", "Tidak boleh kurang dari atau sama dengan 0");
            }
            return sod;
        }

        public SalesOrderDetail VUniqueSOD(SalesOrderDetail sod, ISalesOrderDetailService _sods, IItemService _is)
        {
            IList<SalesOrderDetail> details = _sods.GetObjectsBySalesOrderId(sod.SalesOrderId);
            foreach (var detail in details)
            {
                if (detail.ItemId == sod.ItemId && detail.Id != sod.Id)
                {
                    sod.Errors.Add("SalesOrderDetail", "Tidak boleh memiliki Sku yang sama dalam 1 Sales Order");
                    return sod;
                }
            }
            return sod;
        }

        public SalesOrderDetail VIsConfirmed(SalesOrderDetail sod)
        {
            if (sod.IsConfirmed)
            {
                sod.Errors.Add("IsConfirmed", "Tidak boleh sudah dikonfirmasi");
            }
            return sod;
        }

        public SalesOrderDetail VHasItemPendingDelivery(SalesOrderDetail sod, IItemService _is)
        {
            Item item = _is.GetObjectById(sod.ItemId);
            if (item.PendingDelivery < sod.Quantity)
            {
                sod.Errors.Add("Item.PendingDelivery", "Tidak boleh kurang dari quantity dari Sales Order Detail");
            }
            return sod;
        }

        public SalesOrderDetail VConfirmedDeliveryOrder(SalesOrderDetail sod, IDeliveryOrderDetailService _dods)
        {
            DeliveryOrderDetail dod = _dods.GetObjectBySalesOrderDetailId(sod.Id);
            if (dod == null)
            {
                return sod;
            }
            if (dod.IsConfirmed)
            {
                sod.Errors.Add("IsConfirmed", "Tidak boleh sudah dikonfirmasi");
            }
            return sod;
        }

        public SalesOrderDetail VCreateObject(SalesOrderDetail sod, ISalesOrderDetailService _sods, ISalesOrderService _sos, IItemService _is)
        {
            VHasSalesOrder(sod, _sos);
            if (!isValid(sod)) { return sod; }
            VHasItem(sod, _is);
            if (!isValid(sod)) { return sod; }
            VQuantity(sod);
            if (!isValid(sod)) { return sod; }
            VPrice(sod);
            if (!isValid(sod)) { return sod; }
            VUniqueSOD(sod, _sods, _is);
            return sod;
        }

        public SalesOrderDetail VUpdateObject(SalesOrderDetail sod, ISalesOrderDetailService _sods, ISalesOrderService _sos, IItemService _is)
        {
            VHasSalesOrder(sod, _sos);
            if (!isValid(sod)) { return sod; }
            VHasItem(sod, _is);
            if (!isValid(sod)) { return sod; }
            VQuantity(sod);
            if (!isValid(sod)) { return sod; }
            VPrice(sod);
            if (!isValid(sod)) { return sod; }
            VUniqueSOD(sod, _sods, _is);
            if (!isValid(sod)) { return sod; }
            VIsConfirmed(sod);
            return sod;
        }

        public SalesOrderDetail VDeleteObject(SalesOrderDetail sod)
        {
            VIsConfirmed(sod);
            return sod;
        }

        public SalesOrderDetail VConfirmObject(SalesOrderDetail sod)
        {
            VIsConfirmed(sod);
            return sod;
        }

        public SalesOrderDetail VUnconfirmObject(SalesOrderDetail sod, ISalesOrderDetailService _sods, IDeliveryOrderDetailService _dods, IItemService _is)
        {
            VHasItemPendingDelivery(sod, _is);
            if (!isValid(sod)) { return sod; }
            VConfirmedDeliveryOrder(sod, _dods);
            return sod;
        }

        public bool ValidCreateObject(SalesOrderDetail sod,  ISalesOrderDetailService _sods, ISalesOrderService _sos, IItemService _is)
        {
            VCreateObject(sod, _sods, _sos, _is);
            return isValid(sod);
        }

        public bool ValidUpdateObject(SalesOrderDetail sod,  ISalesOrderDetailService _sods, ISalesOrderService _sos, IItemService _is)
        {
            sod.Errors.Clear();
            VUpdateObject(sod, _sods, _sos, _is);
            return isValid(sod);
        }

        public bool ValidDeleteObject(SalesOrderDetail sod)
        {
            sod.Errors.Clear();
            VDeleteObject(sod);
            return isValid(sod);
        }

        public bool ValidConfirmObject(SalesOrderDetail sod)
        {
            sod.Errors.Clear();
            VConfirmObject(sod);
            return isValid(sod);
        }

        public bool ValidUnconfirmObject(SalesOrderDetail sod, ISalesOrderDetailService _sods, IDeliveryOrderDetailService _dods, IItemService _is)
        {
            sod.Errors.Clear();
            VUnconfirmObject(sod, _sods, _dods, _is);
            return isValid(sod);
        }

        public bool isValid(SalesOrderDetail obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(SalesOrderDetail obj)
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