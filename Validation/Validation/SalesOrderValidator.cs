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
    public class SalesOrderValidator : ISalesOrderValidator
    {
        public SalesOrder VContact(SalesOrder so, IContactService _cs)
        {
            Contact c = _cs.GetObjectById(so.ContactId);
            if (c == null)
            {
                so.Errors.Add("Contact", "Tidak boleh tidak ada");
            }
            return so;
        }

        public SalesOrder VSalesDate(SalesOrder so)
        {
            /* salesDate is never null
            if (so.SalesDate == null)
            {
                so.Errors.Add("Sales Date, "Tidak boleh tidak ada");
            }
            */
            return so;
        }

        public SalesOrder VIsConfirmed(SalesOrder so)
        {
            if (so.IsConfirmed)
            {
                so.Errors.Add("IsConfirmed", "Tidak boleh sudah dikonfirmasi");
            }
            return so;
        }

        public SalesOrder VHasSalesOrderDetails(SalesOrder so, ISalesOrderDetailService _sods)
        {
            IList<SalesOrderDetail> details = _sods.GetObjectsBySalesOrderId(so.Id);
            if (!details.Any())
            {
                so.Errors.Add("SalesOrderDetail", "Tidak boleh tidak ada");
            }
            return so;
        }

        public SalesOrder VCreateObject(SalesOrder so, IContactService _cs)
        {
            VContact(so, _cs);
            if (!isValid(so)) { return so; }
            VSalesDate(so);
            return so;
        }

        public SalesOrder VUpdateObject(SalesOrder so, IContactService _cs)
        {
            VContact(so, _cs);
            if (!isValid(so)) { return so; }
            VSalesDate(so);
            if (!isValid(so)) { return so; }
            VIsConfirmed(so);
            return so;
        }

        public SalesOrder VDeleteObject(SalesOrder so, ISalesOrderDetailService _sods)
        {
            VIsConfirmed(so);
            return so;
        }

        public SalesOrder VConfirmObject(SalesOrder so, ISalesOrderDetailService _sods)
        {
            VIsConfirmed(so);
            if (!isValid(so)) { return so; }
            VHasSalesOrderDetails(so, _sods);
            if (isValid(so))
            {
                IList<SalesOrderDetail> details = _sods.GetObjectsBySalesOrderId(so.Id);
                ISalesOrderDetailValidator detailvalidator = new SalesOrderDetailValidator();
                foreach (var detail in details)
                {
                    detailvalidator.VConfirmObject(detail);
                    foreach (var error in detail.Errors)
                    {
                        so.Errors.Add(error.Key, error.Value);
                    }
                    if (!isValid(so)) { return so; }
                }
            }
            return so;
        }

        public SalesOrder VUnconfirmObject(SalesOrder so, ISalesOrderDetailService _sods, IDeliveryOrderDetailService _dods, IItemService _is)
        {
            if (isValid(so))
            {
                IList<SalesOrderDetail> details = _sods.GetObjectsBySalesOrderId(so.Id);
                foreach (var detail in details)
                {
                    if (!_sods.GetValidator().ValidUnconfirmObject(detail, _sods, _dods, _is))
                    {
                        foreach (var error in detail.Errors)
                        {
                            so.Errors.Add(error.Key, error.Value);
                        }
                        if (!isValid(so)) { return so; }
                    }
                }
            }
            return so;
        }

        public bool ValidCreateObject(SalesOrder so, IContactService _cs)
        {
            VCreateObject(so, _cs);
            return isValid(so);
        }

        public bool ValidUpdateObject(SalesOrder so, IContactService _cs)
        {
            so.Errors.Clear();
            VUpdateObject(so, _cs);
            return isValid(so);
        }

        public bool ValidDeleteObject(SalesOrder so, ISalesOrderDetailService _sods)
        {
            so.Errors.Clear();
            VDeleteObject(so, _sods);
            return isValid(so);
        }

        public bool ValidConfirmObject(SalesOrder so, ISalesOrderDetailService _sods)
        {
            so.Errors.Clear();
            VConfirmObject(so, _sods);
            return isValid(so);
        }

        public bool ValidUnconfirmObject(SalesOrder so, ISalesOrderDetailService _sods, IDeliveryOrderDetailService _dods, IItemService _is)
        {
            so.Errors.Clear();
            VUnconfirmObject(so, _sods, _dods, _is);
            return isValid(so);
        }

        public bool isValid(SalesOrder obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(SalesOrder obj)
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