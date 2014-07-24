using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interface.Validation;
using Core.DomainModel;
using Core.Interface.Service;
using Core.Constant;

namespace Validation.Validation
{
    public class DeliveryOrderValidator : IDeliveryOrderValidator
    {

        public DeliveryOrder VContact(DeliveryOrder d, IContactService _cs)
        {
            Contact c = _cs.GetObjectById(d.ContactId);
            if (c == null)
            {
                d.Errors.Add("Contact", "Tidak boleh tidak ada");
            }
            return d;
        }

        public DeliveryOrder VDeliveryDate(DeliveryOrder d)
        {
            /* deliveryDate is never null
            if (d.DeliveryDate == null)
            {
                d.Errors.Add("DeliveryDate", "Tidak boleh kosong");
            }
            */
            return d;
        }

        public DeliveryOrder VIsConfirmed(DeliveryOrder d)
        {
            if (d.IsConfirmed)
            {
                d.Errors.Add("IsConfirmed", "Tidak boleh sudah dikonfirmasi");
            }
            return d;
        }

        public DeliveryOrder VHasDeliveryOrderDetails(DeliveryOrder d, IDeliveryOrderDetailService _dods)
        {
            IList<DeliveryOrderDetail> details = _dods.GetObjectsByDeliveryOrderId(d.Id);
            if (!details.Any())
            {
                d.Errors.Add("DeliveryOrderDetail", "Tidak boleh tidak ada");
            }
            return d;
        }

        public DeliveryOrder VHasItemQuantity(DeliveryOrder d, IDeliveryOrderDetailService _dods, IItemService _is)
        {
            IList<DeliveryOrderDetail> details = _dods.GetObjectsByDeliveryOrderId(d.Id);
            foreach (var detail in details)
            {
                Item item = _is.GetObjectById(detail.ItemId);
                if (item.Ready < 0)
                {
                    d.Errors.Add ("Item.Ready", "Tidak boleh kurang dari 0");
                }
            }
            return d;
        }

        public DeliveryOrder VCreateObject(DeliveryOrder d, IContactService _cs)
        {
            VContact(d, _cs);
            if (!isValid(d)) { return d; }
            VDeliveryDate(d);
            return d;
        }

        public DeliveryOrder VUpdateObject(DeliveryOrder d, IContactService _cs)
        {
            VContact(d, _cs);
            if (!isValid(d)) { return d; }
            VDeliveryDate(d);
            if (!isValid(d)) { return d; }
            VIsConfirmed(d);
            return d;
        }

        public DeliveryOrder VDeleteObject(DeliveryOrder d, IDeliveryOrderDetailService _dods)
        {
            VIsConfirmed(d);
            return d;
        }

        public DeliveryOrder VConfirmObject(DeliveryOrder d, IDeliveryOrderDetailService _dods, IItemService _is)
        {
            VIsConfirmed(d);
            if (!isValid(d)) { return d; }
            VHasDeliveryOrderDetails(d, _dods);
            if (isValid(d))
            {
                IList<DeliveryOrderDetail> details = _dods.GetObjectsByDeliveryOrderId(d.Id);
                IDeliveryOrderDetailValidator detailvalidator = new DeliveryOrderDetailValidator();
                foreach (var detail in details)
                {
                    detailvalidator.VConfirmObject(detail, _is);
                    foreach (var error in detail.Errors)
                    {
                        d.Errors.Add(error.Key, error.Value);
                    }
                    if (!isValid(d)) { return d; }
                }
            }
            return d;
        }

        public DeliveryOrder VUnconfirmObject(DeliveryOrder d, IDeliveryOrderDetailService _dods, IItemService _is)
        {
            VHasItemQuantity(d, _dods, _is);
            if (isValid(d))
            {
                IList<DeliveryOrderDetail> details = _dods.GetObjectsByDeliveryOrderId(d.Id);
                foreach (var detail in details)
                {
                    if (!_dods.GetValidator().ValidUnconfirmObject(detail, _dods, _is))
                    {
                        foreach (var error in detail.Errors)
                        {
                            d.Errors.Add(error.Key, error.Value);
                        }
                        if (!isValid(d)) { return d; }
                    }
                }
            }

            return d;
        }

        public bool ValidCreateObject(DeliveryOrder d, IContactService _cs)
        {
            VCreateObject(d, _cs);
            return isValid(d);
        }

        public bool ValidUpdateObject(DeliveryOrder d, IContactService _cs)
        {
            d.Errors.Clear();
            VUpdateObject(d, _cs);
            return isValid(d);
        }

        public bool ValidDeleteObject(DeliveryOrder d, IDeliveryOrderDetailService _dods)
        {
            d.Errors.Clear();
            VDeleteObject(d, _dods);
            return isValid(d);
        }

        public bool ValidConfirmObject(DeliveryOrder d, IDeliveryOrderDetailService _dods, IItemService _is)
        {
            d.Errors.Clear();
            VConfirmObject(d, _dods, _is);
            return isValid(d);
        }

        public bool ValidUnconfirmObject(DeliveryOrder d, IDeliveryOrderDetailService _dods, IItemService _is)
        {
            d.Errors.Clear();
            VUnconfirmObject(d, _dods, _is);
            return isValid(d);
        }

        public bool isValid(DeliveryOrder obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(DeliveryOrder obj)
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