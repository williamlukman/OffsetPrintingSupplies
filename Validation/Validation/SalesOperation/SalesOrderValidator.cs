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
    public class SalesOrderValidator : ISalesOrderValidator
    {
        public SalesOrder VContact(SalesOrder salesOrder, IContactService _contactService)
        {
            Contact contact = _contactService.GetObjectById(salesOrder.ContactId);
            if (contact == null)
            {
                salesOrder.Errors.Add("Contact", "Tidak boleh tidak ada");
            }
            return salesOrder;
        }

        public SalesOrder VSalesDate(SalesOrder salesOrder)
        {
            /* salesDate is never null
            if (salesOrder.SalesDate == null)
            {
                salesOrder.Errors.Add("Sales Date, "Tidak boleh tidak ada");
            }
            */
            return salesOrder;
        }
        
        public SalesOrder VHasBeenConfirmed(SalesOrder salesOrder)
        {
            if (!salesOrder.IsConfirmed)
            {
                salesOrder.Errors.Add("Generic", "Belum dikonfirmasi");
            }
            return salesOrder;
        }

        public SalesOrder VHasNotBeenConfirmed(SalesOrder salesOrder)
        {
            if (salesOrder.IsConfirmed)
            {
                salesOrder.Errors.Add("Generic", "Tidak boleh sudah dikonfirmasi");
            }
            return salesOrder;
        }

        public SalesOrder VHasSalesOrderDetails(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService)
        {
            IList<SalesOrderDetail> details = _salesOrderDetailService.GetObjectsBySalesOrderId(salesOrder.Id);
            if (!details.Any())
            {
                salesOrder.Errors.Add("SalesOrderDetail", "Tidak boleh tidak ada");
            }
            return salesOrder;
        }

        public SalesOrder VAllDetailsHaveBeenFinished(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService)
        {
            IList<SalesOrderDetail> details = _salesOrderDetailService.GetObjectsBySalesOrderId(salesOrder.Id);
            foreach (var detail in details)
            {
                if (!detail.IsFinished)
                {
                    salesOrder.Errors.Add("Generic", "Detail belum selesai");
                    return salesOrder;
                }
            }
            return salesOrder;
        }

        public SalesOrder VAllDetailsHaveNotBeenFinished(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService)
        {
            IList<SalesOrderDetail> details = _salesOrderDetailService.GetObjectsBySalesOrderId(salesOrder.Id);
            foreach (var detail in details)
            {
                if (detail.IsFinished)
                {
                    salesOrder.Errors.Add("Generic", "Detail sudah selesai");
                    return salesOrder;
                }
            }
            return salesOrder;
        }

        public SalesOrder VCreateObject(SalesOrder salesOrder, IContactService _contactService)
        {
            VContact(salesOrder, _contactService);
            if (!isValid(salesOrder)) { return salesOrder; }
            VSalesDate(salesOrder);
            return salesOrder;
        }

        public SalesOrder VUpdateObject(SalesOrder salesOrder, IContactService _contactService)
        {
            VContact(salesOrder, _contactService);
            if (!isValid(salesOrder)) { return salesOrder; }
            VSalesDate(salesOrder);
            if (!isValid(salesOrder)) { return salesOrder; }
            VHasNotBeenConfirmed(salesOrder);
            return salesOrder;
        }

        public SalesOrder VDeleteObject(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService)
        {
            VHasNotBeenConfirmed(salesOrder);
            return salesOrder;
        }

        public SalesOrder VConfirmObject(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService)
        {
            VHasNotBeenConfirmed(salesOrder);
            if (!isValid(salesOrder)) { return salesOrder; }
            VHasSalesOrderDetails(salesOrder, _salesOrderDetailService);
            return salesOrder;
        }

        public SalesOrder VUnconfirmObject(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService, IDeliveryOrderDetailService _deliveryOrderDetailService, IItemService _itemService)
        {
            VHasBeenConfirmed(salesOrder);
            if (!isValid(salesOrder)) { return salesOrder; }
            VAllDetailsHaveNotBeenFinished(salesOrder, _salesOrderDetailService);
            return salesOrder;
        }

        public SalesOrder VCompleteObject(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService)
        {
            VAllDetailsHaveBeenFinished(salesOrder, _salesOrderDetailService);
            return salesOrder;
        }

        public bool ValidCreateObject(SalesOrder salesOrder, IContactService _contactService)
        {
            VCreateObject(salesOrder, _contactService);
            return isValid(salesOrder);
        }

        public bool ValidUpdateObject(SalesOrder salesOrder, IContactService _contactService)
        {
            salesOrder.Errors.Clear();
            VUpdateObject(salesOrder, _contactService);
            return isValid(salesOrder);
        }

        public bool ValidDeleteObject(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService)
        {
            salesOrder.Errors.Clear();
            VDeleteObject(salesOrder, _salesOrderDetailService);
            return isValid(salesOrder);
        }

        public bool ValidConfirmObject(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService)
        {
            salesOrder.Errors.Clear();
            VConfirmObject(salesOrder, _salesOrderDetailService);
            return isValid(salesOrder);
        }

        public bool ValidUnconfirmObject(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService, IDeliveryOrderDetailService _deliveryOrderDetailService, IItemService _itemService)
        {
            salesOrder.Errors.Clear();
            VUnconfirmObject(salesOrder, _salesOrderDetailService, _deliveryOrderDetailService, _itemService);
            return isValid(salesOrder);
        }

        public bool ValidCompleteObject(SalesOrder salesOrder, ISalesOrderDetailService _salesOrderDetailService)
        {
            salesOrder.Errors.Clear();
            VCompleteObject(salesOrder, _salesOrderDetailService);
            return isValid(salesOrder);
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